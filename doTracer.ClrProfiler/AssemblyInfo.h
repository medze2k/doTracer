#pragma once

#include <string>

#include "cor.h"
#include "corprof.h"

class AssemblyInfo final {

public:
	AssemblyInfo(ICorProfilerInfo* pCorProfilerInfo, AssemblyID assemblyId);
	const std::wstring Name();
	const ModuleID& ModuleId() const;
	const AssemblyID& AssemblyId() const;
private:
	ModuleID m_moduleId;
	AssemblyID m_assemblyId;
	std::wstring m_assemblyName;
	ICorProfilerInfo* m_pCorProfilerInfo;
};