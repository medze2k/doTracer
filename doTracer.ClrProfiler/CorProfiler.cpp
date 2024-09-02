// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "CorProfiler.h"
#include "profiler_pal.h"
#include "ModuleInfo.h"
#include "MethodInfo.h"
#include "AssemblyInfo.h"


#define NAME_BUFFER_SIZE 1024

// The instance of the interface.  This is used by the entry and exit
// hooks as well as the shutdown code.
ICorProfilerInfo8* g_corProfilerInfo = NULL;

// List of .NET module IDs to monitor.
std::vector<ModuleID> g_MonitoredModules;

// Boolean to control weather the main module has been reached
// for the first time.
bool g_OwnModuleReached = false;

// Currently executing function ID.
FunctionID LastFunctionID = 0;

// Boolean to control if the method hooked returned.
bool EnterHookFinished = false;

PROFILER_STUB EnterStub(
    FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
    HRESULT hr = S_FALSE;
    bool bIsOwnModule = false;

    // Parse the method metadata.
    MethodInfo methodInfo(g_corProfilerInfo, functionIDOrClientID.functionID, eltInfo);
    hr = methodInfo.Create();
    if (FAILED(hr)) {
        printf("\r\nfailed to create method info");
        return;
    }

    // Have we yet reached the target .net module.
    ModuleID CurrentModule = methodInfo.ModuleId();
    if (std::find(g_MonitoredModules.begin(), g_MonitoredModules.end(),
        CurrentModule) != g_MonitoredModules.end()) {
        bIsOwnModule = true;
        g_OwnModuleReached = true;
    }

    // Always trace the target module calls.
    if (bIsOwnModule) {
        methodInfo.Print(HookType::enterHook);
        return;
    }

    // As long as we did not reach the main module code,
    // we should skip those method calls.
    if (!g_OwnModuleReached) {
        return;
    }

    // Trace the call as long as the previous method finish executing.
    // This achieves L1 filtering of methods.
    if (LastFunctionID == 0 || EnterHookFinished) {

        EnterHookFinished = false;
        LastFunctionID = functionIDOrClientID.functionID;
        methodInfo.Print(HookType::enterHook);
        return;
    }
}

PROFILER_STUB LeaveStub(
    FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
    HRESULT hr = S_FALSE;
    bool bIsOwnModule = false;

    // Parse the method metadata.
    MethodInfo methodInfo(g_corProfilerInfo, functionIDOrClientID.functionID, eltInfo);
    hr = methodInfo.Create();
    if (FAILED(hr)) {
        printf("\r\nfailed to create method info");
        return;
    }

    // Have we yet reached the target .net module.
    ModuleID CurrentModule = methodInfo.ModuleId();
    if (std::find(g_MonitoredModules.begin(), g_MonitoredModules.end(),
        CurrentModule) != g_MonitoredModules.end()) {
        bIsOwnModule = true;
    }

    if (bIsOwnModule) {
        methodInfo.Print(HookType::exitHook);
        return;
    }

    // Skip very method call till our L1 method call returns.
    if (LastFunctionID == 0 || LastFunctionID != functionIDOrClientID.functionID) {
        return;
    }

    EnterHookFinished = true;
    methodInfo.Print(HookType::exitHook);

}

PROFILER_STUB TailcallStub(
    FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
    HRESULT hr = S_FALSE;
    bool IsOwnModule = false;

    MethodInfo methodInfo(g_corProfilerInfo, functionIDOrClientID.functionID, eltInfo);
    hr = methodInfo.Create();
    if (FAILED(hr)) {
        printf("\r\nfailed to create method info");
        return;
    }

    // Have we yet reached the target .net module.
    ModuleID CurrentModule = methodInfo.ModuleId();
    if (std::find(g_MonitoredModules.begin(), g_MonitoredModules.end(),
        CurrentModule) != g_MonitoredModules.end()) {
        IsOwnModule = true;
    }

    if (IsOwnModule) {
        methodInfo.Print(HookType::tailcallHook);
        return;
    }

    // Skip very method call till our L1 method call returns.
    if (LastFunctionID == 0 || LastFunctionID != functionIDOrClientID.functionID) {
        return;
    }

    EnterHookFinished = true;
    methodInfo.Print(HookType::tailcallHook);
}

//
// To keep performance as fast as possible, the Profiling API requires
// that you write the functions using the naked calling convention.
// In essence, your function is inlined right inside the Just In Time
// (JIT) compiler, so you have to handle the function prolog and epilog needs.
//

#ifdef _X86_
#ifdef _WIN32
void __declspec(naked) EnterNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
    __asm
    {
        //  save any registers our code will touch.
        PUSH EAX
        PUSH ECX
        PUSH EDX
                                     // 0x10 = 3 registers + return values.
        PUSH [ESP + 0x10]            // Push the function ID parameter.
        CALL EnterStub  // Call the EnterStub method.

        POP EDX
        POP ECX
        POP EAX
        RET 8
    }
}

void __declspec(naked) LeaveNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
    __asm
    {
        PUSH EAX
        PUSH ECX
        PUSH EDX

        // 0x10 = 3 registers + return values.
        PUSH[ESP + 0x10]             // Push the function ID parameter.
        CALL LeaveStub  // Call the LeaveStub method.

        POP EDX
        POP ECX
        POP EAX
        RET 8
    }
}

void __declspec(naked) TailcallNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{
    __asm
    {
        PUSH EAX
        PUSH ECX
        PUSH EDX

        // 0x10 = 3 registers + return values.
        PUSH[ESP + 0x10]                // Push the function ID parameter.
        CALL TailcallStub  // Call the TailcallStub method.

        POP EDX
        POP ECX
        POP EAX
        RET 8
    }
}
#endif
#elif defined(_AMD64_)
EXTERN_C void EnterNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
EXTERN_C void LeaveNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
EXTERN_C void TailcallNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo);
#endif

CorProfiler::CorProfiler() : refCount(0), corProfilerInfo(nullptr)
{

}

CorProfiler::~CorProfiler()
{
    if (this->corProfilerInfo != nullptr)
    {
        this->corProfilerInfo->Release();
        this->corProfilerInfo = nullptr;
    }
}

HRESULT STDMETHODCALLTYPE CorProfiler::Initialize(IUnknown *pICorProfilerInfoUnk)
{
    DWORD eventMask;
    HRESULT queryInterfaceResult;

    Sleep(7000);

    if (FAILED(InspectCLRVersion(pICorProfilerInfoUnk))) {
        return E_FAIL;
    }

    // Query for the ICorProfilerInfo interface and store the returned
    // interface so that you can request information about the profilee.
    queryInterfaceResult = pICorProfilerInfoUnk->QueryInterface(
        __uuidof(ICorProfilerInfo8), reinterpret_cast<void **>(&this->corProfilerInfo));

    if (FAILED(queryInterfaceResult))
    {
        return E_FAIL;
    }

    g_corProfilerInfo = this->corProfilerInfo;

    // Indicate which items you'd like notifications on.
    eventMask =
        COR_PRF_MONITOR_ENTERLEAVE |        // Call function entry and exit hooks
        COR_PRF_ENABLE_FUNCTION_ARGS |      // Enables argument tracing
        COR_PRF_ENABLE_FUNCTION_RETVAL |    // Enables tracing of return values
        COR_PRF_ENABLE_FRAME_INFO |          // Enables the retrieval of an exact ClassID for a generic function
        // COR_PRF_MONITOR_CLASS_LOADS |
        // COR_PRF_MONITOR_CODE_TRANSITIONS |
        // COR_PRF_MONITOR_ASSEMBLY_LOADS |
        COR_PRF_MONITOR_MODULE_LOADS;


    auto hr = this->corProfilerInfo->SetEventMask(eventMask);
    if (FAILED(hr))
    {
        printf("ERROR: Profiler SetEventMask failed (HRESULT: %d)", hr);
        return E_FAIL;
    }

    // Unlike the other notifications that end up calling our
    // ICorProfilerCallback functions, we need to register three
    // special callbacks to the runtime.
    hr = this->corProfilerInfo->SetEnterLeaveFunctionHooks3WithInfo(
        EnterNaked, LeaveNaked, TailcallNaked);
    if (FAILED(hr))
    {
        printf("ERROR: Profiler SetEnterLeaveFunctionHooks3WithInfo failed (HRESULT: %d)", hr);
        return E_FAIL;
    }

    //
    // Use the ICorProfilerInfo3::SetFunctionIDMapper2 to avoid performance impact
    // if you just want to profile a small subset of methods.
    //

    return hr;
}

HRESULT STDMETHODCALLTYPE CorProfiler::Shutdown()
{
    if (this->corProfilerInfo != nullptr)
    {
        this->corProfilerInfo->Release();
        this->corProfilerInfo = nullptr;
    }

    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AppDomainCreationStarted(AppDomainID appDomainId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AppDomainCreationFinished(AppDomainID appDomainId, HRESULT hrStatus)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AppDomainShutdownStarted(AppDomainID appDomainId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AppDomainShutdownFinished(AppDomainID appDomainId, HRESULT hrStatus)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AssemblyLoadStarted(AssemblyID assemblyId)
{
    AssemblyInfo info(g_corProfilerInfo, assemblyId);
    printf("\r\nAssemblyLoadStarted: %ws", info.Name().c_str());

    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AssemblyLoadFinished(AssemblyID assemblyId, HRESULT hrStatus)
{
    AssemblyInfo info(g_corProfilerInfo, assemblyId);
    printf("\r\nAssemblyLoadFinished: %ws", info.Name().c_str());
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AssemblyUnloadStarted(AssemblyID assemblyId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::AssemblyUnloadFinished(AssemblyID assemblyId, HRESULT hrStatus)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ModuleLoadStarted(ModuleID moduleId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ModuleLoadFinished(ModuleID moduleId, HRESULT hrStatus)
{
    ModuleInfo info(g_corProfilerInfo, moduleId);

    const std::wstring ModuleName = info.Name();

    printf("\r\nModuleLoadFinished: %ws", ModuleName.c_str());

    if (!info.IsDotNetDll()) {
        g_MonitoredModules.push_back(moduleId);
    }
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ModuleUnloadStarted(ModuleID moduleId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ModuleUnloadFinished(ModuleID moduleId, HRESULT hrStatus)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ModuleAttachedToAssembly(ModuleID moduleId, AssemblyID AssemblyId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ClassLoadStarted(ClassID classId)
{
    HRESULT hr;
    ModuleID moduleID = 0;
    mdTypeDef typeDefToken = 0;

    hr = g_corProfilerInfo->GetClassIDInfo(classId, &moduleID, &typeDefToken);

    // Get module name.
    WCHAR moduleName[MAX_LENGTH];
    AssemblyID assemblyID;
    hr = g_corProfilerInfo->GetModuleInfo(moduleID, NULL, MAX_LENGTH, 0, moduleName, &assemblyID);

    // Get Assembly name.
    WCHAR pszName[MAX_LENGTH];
    ULONG nameLen;
    hr = g_corProfilerInfo->GetAssemblyInfo(assemblyID, MAX_LENGTH, &nameLen, pszName, NULL, NULL);

    IMetaDataImport* pIMetaDataImport = NULL;

    hr = g_corProfilerInfo->GetModuleMetaData(moduleID, ofRead, IID_IMetaDataImport,
        (IUnknown**)&pIMetaDataImport);

    WCHAR className[MAX_LENGTH];

    ULONG ulCopiedChars;
    hr = pIMetaDataImport->GetTypeDefProps(typeDefToken, className, MAX_LENGTH,
        &ulCopiedChars, NULL, NULL);

    printf("\r\nClassLoadStarted: %ws : %ws", moduleName, className);

    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ClassLoadFinished(ClassID classId, HRESULT hrStatus)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ClassUnloadStarted(ClassID classId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ClassUnloadFinished(ClassID classId, HRESULT hrStatus)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::FunctionUnloadStarted(FunctionID functionId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::JITCompilationStarted(FunctionID functionId, BOOL fIsSafeToBlock)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::JITCompilationFinished(FunctionID functionId, HRESULT hrStatus, BOOL fIsSafeToBlock)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::JITCachedFunctionSearchStarted(FunctionID functionId, BOOL *pbUseCachedFunction)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::JITCachedFunctionSearchFinished(FunctionID functionId, COR_PRF_JIT_CACHE result)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::JITFunctionPitched(FunctionID functionId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::JITInlining(FunctionID callerId, FunctionID calleeId, BOOL *pfShouldInline)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ThreadCreated(ThreadID threadId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ThreadDestroyed(ThreadID threadId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ThreadAssignedToOSThread(ThreadID managedThreadId, DWORD osThreadId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingClientInvocationStarted()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingClientSendingMessage(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingClientReceivingReply(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingClientInvocationFinished()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingServerReceivingMessage(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingServerInvocationStarted()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingServerInvocationReturned()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RemotingServerSendingReply(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::UnmanagedToManagedTransition(FunctionID functionId, COR_PRF_TRANSITION_REASON reason)
{
    printf("\r\nUnmanaged To Managed Transition: %llx, reason: %d", functionId, reason);
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ManagedToUnmanagedTransition(FunctionID functionId, COR_PRF_TRANSITION_REASON reason)
{
    printf("\r\nManaged To Unmanaged Transition: %llx, reason: %d", functionId, reason);
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RuntimeSuspendFinished()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RuntimeSuspendAborted()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RuntimeResumeStarted()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RuntimeResumeFinished()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RuntimeThreadSuspended(ThreadID threadId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RuntimeThreadResumed(ThreadID threadId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::MovedReferences(ULONG cMovedObjectIDRanges, ObjectID oldObjectIDRangeStart[], ObjectID newObjectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ObjectAllocated(ObjectID objectId, ClassID classId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ObjectsAllocatedByClass(ULONG cClassCount, ClassID classIds[], ULONG cObjects[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ObjectReferences(ObjectID objectId, ClassID classId, ULONG cObjectRefs, ObjectID objectRefIds[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RootReferences(ULONG cRootRefs, ObjectID rootRefIds[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionThrown(ObjectID thrownObjectId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionSearchFunctionEnter(FunctionID functionId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionSearchFunctionLeave()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionSearchFilterEnter(FunctionID functionId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionSearchFilterLeave()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionSearchCatcherFound(FunctionID functionId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionOSHandlerEnter(UINT_PTR __unused)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionOSHandlerLeave(UINT_PTR __unused)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionUnwindFunctionEnter(FunctionID functionId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionUnwindFunctionLeave()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionUnwindFinallyEnter(FunctionID functionId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionUnwindFinallyLeave()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionCatcherEnter(FunctionID functionId, ObjectID objectId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionCatcherLeave()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::COMClassicVTableCreated(ClassID wrappedClassId, REFGUID implementedIID, void *pVTable, ULONG cSlots)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::COMClassicVTableDestroyed(ClassID wrappedClassId, REFGUID implementedIID, void *pVTable)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionCLRCatcherFound()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ExceptionCLRCatcherExecute()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ThreadNameChanged(ThreadID threadId, ULONG cchName, WCHAR name[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::GarbageCollectionStarted(int cGenerations, BOOL generationCollected[], COR_PRF_GC_REASON reason)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::SurvivingReferences(ULONG cSurvivingObjectIDRanges, ObjectID objectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::GarbageCollectionFinished()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::FinalizeableObjectQueued(DWORD finalizerFlags, ObjectID objectID)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::RootReferences2(ULONG cRootRefs, ObjectID rootRefIds[], COR_PRF_GC_ROOT_KIND rootKinds[], COR_PRF_GC_ROOT_FLAGS rootFlags[], UINT_PTR rootIds[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::HandleCreated(GCHandleID handleId, ObjectID initialObjectId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::HandleDestroyed(GCHandleID handleId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::InitializeForAttach(IUnknown *pCorProfilerInfoUnk, void *pvClientData, UINT cbClientData)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ProfilerAttachComplete()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ProfilerDetachSucceeded()
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ReJITCompilationStarted(FunctionID functionId, ReJITID rejitId, BOOL fIsSafeToBlock)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::GetReJITParameters(ModuleID moduleId, mdMethodDef methodId, ICorProfilerFunctionControl *pFunctionControl)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ReJITCompilationFinished(FunctionID functionId, ReJITID rejitId, HRESULT hrStatus, BOOL fIsSafeToBlock)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ReJITError(ModuleID moduleId, mdMethodDef methodId, FunctionID functionId, HRESULT hrStatus)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::MovedReferences2(ULONG cMovedObjectIDRanges, ObjectID oldObjectIDRangeStart[], ObjectID newObjectIDRangeStart[], SIZE_T cObjectIDRangeLength[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::SurvivingReferences2(ULONG cSurvivingObjectIDRanges, ObjectID objectIDRangeStart[], SIZE_T cObjectIDRangeLength[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ConditionalWeakTableElementReferences(ULONG cRootRefs, ObjectID keyRefIds[], ObjectID valueRefIds[], GCHandleID rootIds[])
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::GetAssemblyReferences(const WCHAR *wszAssemblyPath, ICorProfilerAssemblyReferenceProvider *pAsmRefProvider)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::ModuleInMemorySymbolsUpdated(ModuleID moduleId)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::DynamicMethodJITCompilationStarted(FunctionID functionId, BOOL fIsSafeToBlock, LPCBYTE ilHeader, ULONG cbILHeader)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::DynamicMethodJITCompilationFinished(FunctionID functionId, HRESULT hrStatus, BOOL fIsSafeToBlock)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CorProfiler::DynamicMethodUnloaded(FunctionID functionId)
{
    return E_NOTIMPL;
}

/*/////////////////////////////////////////////////////////////////////
// General helper methods common to all profilers.
/////////////////////////////////////////////////////////////////////*/

BOOL STDMETHODCALLTYPE CorProfiler::GetClassAndMethodFromFunctionId(FunctionID uiFunctionId,
    LPWSTR szClass,  UINT uiClassLen, LPWSTR szMethod, UINT uiMethodLen)
{
    // The magic of metadata is how I'll find this information.

    // The return value.
    BOOL bRet = FALSE;

    // The token for the function id.
    mdToken MethodMetaToken = 0;
    // The metadata interface.
    IMetaDataImport* pIMetaDataImport = NULL;

    // Ask ICorProfilerInfo for the metadata interface for this
    // functionID
    HRESULT hr = g_corProfilerInfo->GetTokenAndMetaDataFromFunction(uiFunctionId,
            IID_IMetaDataImport,
            (IUnknown**)&pIMetaDataImport,
            &MethodMetaToken);

    if (SUCCEEDED(hr))
    {
        // The token for the class.
        mdTypeDef ClassMetaToken;
        // The total chars copies.
        ULONG ulCopiedChars;

        // Look up the method information from the metadata.
        hr = pIMetaDataImport->GetMethodProps(MethodMetaToken,
            &ClassMetaToken,
            szMethod,
            uiMethodLen,
            &ulCopiedChars,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL);

        if ((SUCCEEDED(hr)) && (ulCopiedChars < uiMethodLen))
        {
            // Armed with the class meta data token, I can look up the
            // class.
            hr = pIMetaDataImport->GetTypeDefProps(ClassMetaToken,
                szClass,
                uiClassLen,
                &ulCopiedChars,
                NULL,
                NULL);

            if ((SUCCEEDED(hr)) && (ulCopiedChars < uiClassLen))
            {
                bRet = TRUE;
            }
            else
            {
                bRet = FALSE;
            }
        }
        else
        {
            bRet = FALSE;
        }
        pIMetaDataImport->Release();
    }
    else
    {
        bRet = FALSE;
    }

    return (bRet);
}

BOOL STDMETHODCALLTYPE CorProfiler::GetClassFromObjectId(ObjectID uiObjectId,
    LPWSTR szClass, UINT uiClassLen)
{
    // The return value.
    BOOL bRet = FALSE;

    // Get the class meta data token.
    ClassID ClsID;

    HRESULT hr = g_corProfilerInfo->GetClassFromObject(uiObjectId, &ClsID);
    if (SUCCEEDED(hr))
    {

        // Get the class module ID and metadata token.
        ModuleID ModID;
        mdTypeDef ClassMetaToken;

        hr = g_corProfilerInfo->GetClassIDInfo(ClsID,
            &ModID,
            &ClassMetaToken);

        if (SUCCEEDED(hr))
        {
            // Now that I have the module ID, I can ask for the
            // metadata interface.
            IMetaDataImport* pIMetaDataImport = NULL;

            hr = g_corProfilerInfo->
                GetModuleMetaData(ModID,
                    ofRead,
                    IID_IMetaDataImport,
                    (IUnknown**)&pIMetaDataImport);

            if (SUCCEEDED(hr))
            {

                ULONG ulCopiedChars;
                hr = pIMetaDataImport->
                    GetTypeDefProps(ClassMetaToken,
                        szClass,
                        uiClassLen,
                        &ulCopiedChars,
                        NULL,
                        NULL);

                if ((SUCCEEDED(hr)) && (ulCopiedChars < uiClassLen))
                {
                    bRet = TRUE;
                }
                else
                {
                    bRet = FALSE;
                }

                pIMetaDataImport->Release();
            }
            else
            {
                bRet = FALSE;
            }
        }
        else
        {
            bRet = FALSE;
        }
    }
    else
    {
        bRet = FALSE;
    }

    return (bRet);
}

HRESULT STDMETHODCALLTYPE CorProfiler::InspectCLRVersion(IUnknown* corProfilerInfoUnk)
{
    HRESULT hr = S_FALSE;
    IUnknown* tstVerProfilerInfo = nullptr;

    if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo8), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo8 available. Profiling API compatibility: .NET Fx 4.7.2 or later.");
    }
    else if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo7), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo7 available. Profiling API compatibility: .NET Fx 4.6.1 or later.");
    }
    else if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo6), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo6 available. Profiling API compatibility: .NET Fx 4.6 or later.");
    }
    else if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo5), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo5 available. Profiling API compatibility: .NET Fx 4.5.2 or later.");
    }
    else if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo4), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo4 available. Profiling API compatibility: .NET Fx 4.5 or later.");
    }
    else if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo3), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo3 available. Profiling API compatibility: .NET Fx 4.0 or later.");
    }
    else if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo2), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo2 available. Profiling API compatibility: .NET Fx 2.0 or later.");
    }
    else if (S_OK == corProfilerInfoUnk->QueryInterface(__uuidof(ICorProfilerInfo), (void**)&tstVerProfilerInfo))
    {
        printf("ICorProfilerInfo available. Profiling API compatibility: .NET Fx 2 or later.");
    }
    else
    {
        printf("No ICorProfilerInfoXxx available. A valid IUnknown pointer was passed to CorProfilerCallback"
            " for initialization, but QueryInterface(..) did not succeed for any of the known "
            "ICorProfilerInfoXxx ifaces."
            " No compatible Profiling API is available.");
    }

    if (tstVerProfilerInfo) {
        tstVerProfilerInfo->Release();
        tstVerProfilerInfo = nullptr;
        hr = S_OK;
    }

    return hr;
}

DWORD STDMETHODCALLTYPE CorProfiler::GetThreadId() {

    HRESULT hr = S_FALSE;
    ThreadID ManagedThreadId = 0;
    DWORD dwUnmanagedThreadId = 0;

    // If the current thread is an internal runtime thread or other
    // unmanaged thread, GetCurrentThreadID returns CORPROF_E_NOT_MANAGED_THREAD.

    hr = g_corProfilerInfo->GetCurrentThreadID(&ManagedThreadId);
    if (FAILED(hr)) {
        return dwUnmanagedThreadId;
    }

    hr = g_corProfilerInfo->GetThreadInfo(ManagedThreadId, &dwUnmanagedThreadId);
    if (FAILED(hr)) {
        return 0;
    }

    return dwUnmanagedThreadId;
}