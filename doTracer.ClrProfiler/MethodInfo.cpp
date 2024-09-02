#include "MethodInfo.h"
#include "ModuleInfo.h"
#include "AssemblyInfo.h"
#include "ValueReader.h"

const DWORD MethodInfo::GetThreadId() const {

    HRESULT hr = S_FALSE;
    ThreadID ManagedThreadId = 0;
    DWORD dwUnmanagedThreadId = 0;

    // If the current thread is an internal runtime thread or other
    // unmanaged thread, GetCurrentThreadID returns CORPROF_E_NOT_MANAGED_THREAD.

    hr = m_pCorProfilerInfo->GetCurrentThreadID(&ManagedThreadId);
    if (FAILED(hr)) {
        return dwUnmanagedThreadId;
    }

    hr = m_pCorProfilerInfo->GetThreadInfo(ManagedThreadId, &dwUnmanagedThreadId);
    if (FAILED(hr)) {
        return 0;
    }

    return dwUnmanagedThreadId;
}

MethodInfo::MethodInfo(ICorProfilerInfo8* pCorProfilerInfo, FunctionID functionId,
    COR_PRF_ELT_INFO eltInfo) :
    m_pCorProfilerInfo(pCorProfilerInfo),
    m_FunctionId(functionId),
    m_eltInfo(eltInfo),
    m_pMetaDataImport(nullptr),
    m_pSigBlog(nullptr),
    m_AssemblyId(0),
    m_ModuleId(0),
    m_ClassId(0),
    m_MethodAttr(0),
    m_ClassAttr(0),
    m_MethodToken(0),
    m_ArgumentCount(0),
    m_StartIndex(0),
    m_ReturnValue(nullptr),
    m_MethodName(MAX_LENGTH, 0),
    m_ClassName(MAX_LENGTH, 0),
    m_CallingConvetion(IMAGE_CEE_CS_CALLCONV_MAX)
{
    m_Parameters.clear();
}

const std::wstring& MethodInfo::Name() const
{
	return m_MethodName;
}

const std::wstring& MethodInfo::ClassName() const
{
    return m_ClassName;
}

const std::wstring& MethodInfo::ModuleName() const
{
    return m_ModuleName;
}

const std::wstring& MethodInfo::AssemblyName() const
{
    return m_AssemblyName;
}

const ModuleID& MethodInfo::ModuleId() const
{
    return m_ModuleId;
}

const bool MethodInfo::Isstatic() const {
    return IsMdStatic(m_MethodAttr) != 0;
}

HRESULT MethodInfo::Create()
{
    HRESULT hr = S_FALSE;

    //
    // Gets the parent class and metadata token for the specified function.
    //

    hr = m_pCorProfilerInfo->GetFunctionInfo(
        m_FunctionId, &m_ClassId, &m_ModuleId, &m_MethodToken);

    if (FAILED(hr)) {
        return hr;
    }

    //
    // Gets the metadata token and a metadata interface instance.
    //

    hr = m_pCorProfilerInfo->GetTokenAndMetaDataFromFunction(
        m_FunctionId, IID_IMetaDataImport2, (LPUNKNOWN*)&m_pMetaDataImport, NULL);

    if (FAILED(hr)) {
        return hr;
    }

    //
    // Get module and assembly Name.
    //

    ModuleInfo moduleInfo(m_pCorProfilerInfo, m_ModuleId);
    m_ModuleName = moduleInfo.Name();
    m_AssemblyId = moduleInfo.AssemblyId();

    AssemblyInfo assemblyInfo(m_pCorProfilerInfo, m_AssemblyId);
    m_AssemblyName = assemblyInfo.Name();

    //
    // Gets the metadata associated with the method.
    //

    ULONG MethodNameLentgh = 0;
    ULONG SigBlobLength = 0;
    mdTypeDef classTypeDef = 0;

    hr = m_pMetaDataImport->GetMethodProps(
        m_MethodToken, &classTypeDef, &m_MethodName[0], MAX_LENGTH,
        &MethodNameLentgh, &m_MethodAttr, &m_pSigBlog, &SigBlobLength, NULL, NULL);
    if (FAILED(hr)) {
        return hr;
    }
    m_MethodName.resize(MethodNameLentgh);

    //
    // Gets the metadata associated with the class.
    //

    ULONG ClassNameLentgh = 0;

    hr = m_pMetaDataImport->GetTypeDefProps(
        classTypeDef, &m_ClassName[0], MAX_LENGTH, &ClassNameLentgh, &m_ClassAttr, NULL);
    if (FAILED(hr)) {
        return hr;
    }
    m_ClassName.resize(ClassNameLentgh);

    //
    // Get the calling convention.
    //

    m_pSigBlog += CorSigUncompressData(m_pSigBlog, (ULONG*)&m_CallingConvetion);

    //
    // Get arguments count.
    //

    m_pSigBlog += CorSigUncompressData(m_pSigBlog, &m_ArgumentCount);

    if (m_ArgumentCount > 0) {
        m_Parameters.resize(m_ArgumentCount);
    }

    //
    // Get return value.
    //

    hr = ReturnValue();

    //
    // Read arguments names and types.
    //

    hr = ArgumentNames();
    hr = ArgumentTypes();

    return hr;
}

HRESULT MethodInfo::ReturnValue() {

    HRESULT hr = S_OK;

    m_ReturnValue = std::make_shared<ParameterInfo>(m_pMetaDataImport);

    m_pSigBlog = m_ReturnValue->ParseSignature(m_pSigBlog);

    return hr;

}

HRESULT MethodInfo::ArgumentNames() {

    HRESULT hr = S_FALSE;
    ULONG tempArgumentsCount = 0;
    HCORENUM parametersEnum = NULL;
    mdParamDef argumentsTokens[ENUM_ARRAY_SIZE];

    hr = m_pMetaDataImport->EnumParams(
        &parametersEnum, m_MethodToken, argumentsTokens, ENUM_ARRAY_SIZE, &tempArgumentsCount);

    if (FAILED(hr)) {
        return hr;
    }

    // Deal with the "this" hidden parameter for non static method.
    if (m_ArgumentCount + 1 == tempArgumentsCount)
    {
        m_StartIndex++;
    }

    for (ULONG i = 0; i < m_ArgumentCount; ++i) {
        m_Parameters[i] = std::make_shared<ParameterInfo>(m_pMetaDataImport,
            argumentsTokens[i+ m_StartIndex]);
    }

    return hr;
}

HRESULT MethodInfo::ArgumentTypes() {

    for (ULONG i = 0; (m_pSigBlog != NULL) && (i < m_ArgumentCount); ++i)
    {
        m_pSigBlog = m_Parameters[i]->ParseSignature(m_pSigBlog);
    }

    return S_OK;
}

HRESULT MethodInfo::ArgumentValues() {

    ULONG argumentInfoSize = 0;
    COR_PRF_FRAME_INFO frameInfo;

    m_pCorProfilerInfo->GetFunctionEnter3Info(
        m_FunctionId, m_eltInfo, &frameInfo, &argumentInfoSize, NULL);

    byte* pBuffer = new byte[argumentInfoSize];
    m_pCorProfilerInfo->GetFunctionEnter3Info(
        m_FunctionId, m_eltInfo, &frameInfo, &argumentInfoSize,
        (COR_PRF_FUNCTION_ARGUMENT_INFO*)pBuffer);

    COR_PRF_FUNCTION_ARGUMENT_INFO* pArgumentInfo = (COR_PRF_FUNCTION_ARGUMENT_INFO*)pBuffer;

    //if ((m_MethodAttr & mdStatic) == mdStatic)
    //{
    //    printf ("\nmethod is static");

    //    // deal with the "this" hidden parameter for non static method
    //    // ex: show its address (i.e. pArgumentInfo->ranges[0].startAddress)
    //}

    //if (this->Isstatic()) {
    //    printf("\nmethod is static v2");
    //}

    // Deal with the "this" hidden parameter for non static method.
    INT startIndex = 0;
    if (m_ArgumentCount + 1 == pArgumentInfo->numRanges)
    {
        startIndex++;
    }

    for (ULONG i = 0; i < m_ArgumentCount; i++)
    {
        UINT_PTR pStartValue =
            pArgumentInfo->ranges[i + startIndex].startAddress;
        ULONG length =
            pArgumentInfo->ranges[i + startIndex].length;

        //if IsPdIn(m_Parameters[i]->Attributes()) {
        //	printf("Param is _In_");
        //}
        //if IsPdOut(m_Parameters[i]->Attributes()) {
        //	printf("Param is _Out_");
        //}
        //if IsPdOptional(m_Parameters[i]->Attributes()) {
        //	printf("Param is _Optional_");
        //}

        AnyPtr out = std::make_shared<std::any>();
        ValueReader::GetObjectValue(pStartValue, length, m_Parameters[i]->Type(), out);
        m_Parameters[i]->SetParamValue(out);
    }

    return S_OK;
}

VOID MethodInfo::Serialize(HookType hookType) {

    // Get current process ID and thread ID.
    DWORD dwProcessId = GetCurrentProcessId();
    DWORD dwThreadId = this->GetThreadId();
    std::wstring hook = stringifyHookType(hookType);

    auto args = json::array{};
    auto obj = json::object{
        {"hook", ws2s(hook).c_str() },
        {"processId", dwProcessId },
        {"threadId", dwThreadId },
        {"assemblyName", ws2s(m_AssemblyName).c_str() },
        {"moduleName", ws2s(m_ModuleName).c_str() },
        {"className", ws2s(m_ClassName).c_str() },
        {"methodName", ws2s(m_MethodName).c_str() },
        {"args", json::value(args)}
    };

    for (ULONG i = 0; i < m_ArgumentCount; i++) {
        obj["args"].push_back(json::value(m_Parameters[i]->Serialize()));
    }

    std::cout << "\n" << stringify(obj) << std::endl;
}

VOID MethodInfo::Print(HookType hookType) {

    // Read argument values.
    HRESULT hr = ArgumentValues();
    if (FAILED(hr)) {
        printf("\r\nfailed to read argument values");
        return;
    }

    Serialize(hookType);

    // Get current process ID and thread ID.
    DWORD dwProcessId = GetCurrentProcessId();
    DWORD dwThreadId = this->GetThreadId();

    printf("\r\n\n%ws: 0x%llx [PID:%lu] [TID:%lu] [assembly:%ws] module: %ws class:%ws method:%ws",
        stringifyHookType(hookType).c_str(),
        m_FunctionId, dwProcessId, dwThreadId, m_AssemblyName.c_str(),
        m_ModuleName.c_str(), m_ClassName.c_str(), m_MethodName.c_str());

    for (ULONG i = 0; i < m_ArgumentCount; i++) {

        AnyPtr value = m_Parameters[i]->Value();

        printf("\n|------- %ws(%ws): ", m_Parameters[i]->Name().c_str(),
            m_Parameters[i]->TypeAsString().c_str());

        if (value->type() == typeid(bool)) {
            bool v = std::any_cast<bool>(*value);
            if (v) { printf("true"); }
            else { printf("false"); }
        }
        else if (value->type() == typeid(WCHAR)) {
            WCHAR v = std::any_cast<WCHAR>(*value);
            printf("%C", v);
        }
        else if (value->type() == typeid(INT8)) {
            INT8 v = std::any_cast<INT8>(*value);
            printf("%d", v);
        }
        else if (value->type() == typeid(UCHAR)) {
            UCHAR v = std::any_cast<UCHAR>(*value);
            printf("%d", v);
        }
        else if (value->type() == typeid(INT16)) {
            INT16 v = std::any_cast<INT16>(*value);
            printf("%d", v);
        }
        else if (value->type() == typeid(UINT16)) {
            UINT16 v = std::any_cast<UINT16>(*value);
            printf("%d", v);
        }
        else if (value->type() == typeid(INT32)) {
            INT32 v = std::any_cast<INT32>(*value);
            printf("%d", v);
        }
        else if (value->type() == typeid(UINT32)) {
            UINT32 v = std::any_cast<UINT32>(*value);
            printf("%d", v);
        }
        else if (value->type() == typeid(INT64)) {
            INT64 v = std::any_cast<INT64>(*value);
            printf("%lld", v);
        }
        else if (value->type() == typeid(UINT64)) {
            UINT64 v = std::any_cast<UINT64>(*value);
            printf("%lld", v);
        }
        else if (value->type() == typeid(FLOAT)) {
            FLOAT v = std::any_cast<FLOAT>(*value);
            printf("%f", v);
        }
        else if (value->type() == typeid(DOUBLE)) {
            DOUBLE v = std::any_cast<DOUBLE>(*value);
            printf("%g", v);
        }
        else if (value->type() == typeid(std::wstring)) {
            std::wstring v = std::any_cast<std::wstring>(*value);
            printf("%ws", v.c_str());
        }

        // vectors
        else if (value->type() == typeid(std::shared_ptr<std::vector<AnyPtr>>)) {

            std::shared_ptr<std::vector<AnyPtr>> vec =
                std::any_cast<std::shared_ptr<std::vector<AnyPtr>>>(*value);

            for (const auto& e : *vec) {

                if(e->type() == typeid(WCHAR)) {
                    WCHAR v = std::any_cast<WCHAR>(*e);
                    printf("%C,", v);
                }
                else if (e->type() == typeid(INT8)) {
                    INT8 v = std::any_cast<INT8>(*e);
                    printf("%d,", v);
                }
                else if (value->type() == typeid(UCHAR)) {
                    UCHAR v = std::any_cast<UCHAR>(*e);
                    printf("%d,", v);
                }
                else if (e->type() == typeid(INT16)) {
                    INT16 v = std::any_cast<INT16>(*e);
                    printf("%d,", v);
                }
                else if (e->type() == typeid(UINT16)) {
                    UINT16 v = std::any_cast<UINT16>(*e);
                    printf("%d,", v);
                }
                else if (e->type() == typeid(INT32)) {
                    INT32 v = std::any_cast<INT32>(*e);
                    printf("%d,", v);
                }
                else if (e->type() == typeid(UINT32)) {
                    UINT32 v = std::any_cast<UINT32>(*e);
                    printf("%d,", v);
                }
                else if (e->type() == typeid(INT64)) {
                    INT64 v = std::any_cast<INT64>(*e);
                    printf("%lld,", v);
                }
                else if (e->type() == typeid(UINT64)) {
                    UINT64 v = std::any_cast<UINT64>(*e);
                    printf("%lld,", v);
                }
                else if (e->type() == typeid(FLOAT)) {
                    FLOAT v = std::any_cast<FLOAT>(*e);
                    printf("%f,", v);
                }
                else if (e->type() == typeid(DOUBLE)) {
                    DOUBLE v = std::any_cast<DOUBLE>(*e);
                    printf("%g,", v);
                }
                else if (e->type() == typeid(std::wstring)) {
                    std::wstring v = std::any_cast<std::wstring>(*e);
                    printf("%ws,", v.c_str());
                }
            }
        }
    }

}

MethodInfo::~MethodInfo()
{
    if (m_pMetaDataImport) {
        m_pMetaDataImport->Release();
    }
}