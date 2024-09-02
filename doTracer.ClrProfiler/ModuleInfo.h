#pragma once

#include <string>
#include <algorithm>

#include "assert.h"
#include "cor.h"
#include "corprof.h"

class ModuleInfo final {

public:
	ModuleInfo(ICorProfilerInfo* pCorProfilerInfo, ModuleID ModuleId);
	bool IsDotNetDll();
	const std::wstring Name();
	const ModuleID& ModuleId() const;
	const AssemblyID& AssemblyId() const;
private:
	ModuleID m_moduleId;
	AssemblyID m_assemblyId;
	std::wstring m_ModuleName;
	ICorProfilerInfo* m_pCorProfilerInfo;
};