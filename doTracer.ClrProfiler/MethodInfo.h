#pragma once

#include "cor.h"
#include "corprof.h"
#include "ParameterInfo.h"
#include "JsonEncoder.h"
#include "Utils.h"

#include <string>
#include <memory>
#include <vector>

#define MAX_LENGTH 1024
#define ENUM_ARRAY_SIZE 1024

class MethodInfo;
typedef std::shared_ptr<ParameterInfo> ParamInfoPtr;

class MethodInfo final {

public:
	MethodInfo(ICorProfilerInfo8* pCorProfilerInfo, FunctionID functionId, COR_PRF_ELT_INFO eltInfo);
	const std::wstring& Name() const;
	const std::wstring& ClassName() const;
	const std::wstring& ModuleName() const;
	const std::wstring& AssemblyName() const;
	const ModuleID& ModuleId() const;
	const DWORD GetThreadId() const;
	const bool Isstatic() const;
	VOID Serialize(HookType hookType);
	VOID Print(HookType hookType);
	HRESULT Create();
	HRESULT ArgumentNames();
	HRESULT ArgumentTypes();
	HRESULT ArgumentValues();
	HRESULT ReturnValue();
	~MethodInfo();

private:
	ClassID m_ClassId;
	ModuleID m_ModuleId;
	AssemblyID m_AssemblyId;
	FunctionID m_FunctionId;
	mdToken m_MethodToken;
	std::wstring m_ClassName;
	std::wstring m_MethodName;
	std::wstring m_ModuleName;
	std::wstring m_AssemblyName;
	COR_PRF_ELT_INFO m_eltInfo;
	DWORD m_MethodAttr;
	DWORD m_ClassAttr;
	ULONG m_ArgumentCount;
	INT m_StartIndex;
	PCCOR_SIGNATURE m_pSigBlog;
	ICorProfilerInfo8* m_pCorProfilerInfo;
	IMetaDataImport2* m_pMetaDataImport;
	std::vector<ParamInfoPtr> m_Parameters;
	CorCallingConvention m_CallingConvetion;
	std::shared_ptr<ParameterInfo> m_ReturnValue;
};