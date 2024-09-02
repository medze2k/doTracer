#include "AssemblyInfo.h"

AssemblyInfo::AssemblyInfo(ICorProfilerInfo* pCorProfilerInfo, AssemblyID AssemblyId) :
    m_pCorProfilerInfo(pCorProfilerInfo),
    m_assemblyId(AssemblyId),
    m_moduleId(0),
    m_assemblyName(MAX_PATH, 0)
{
}

const std::wstring AssemblyInfo::Name()
{
    HRESULT hr = S_FALSE;
    ULONG AssemblyNameLength = 0;

    hr = m_pCorProfilerInfo->GetAssemblyInfo(
        m_assemblyId, MAX_PATH, &AssemblyNameLength,
        &m_assemblyName[0], NULL, &m_moduleId);

    if (FAILED(hr)) {
        return std::wstring();
    }

    m_assemblyName.resize(AssemblyNameLength);
    return m_assemblyName;
}

const ModuleID& AssemblyInfo::ModuleId() const
{
    return m_moduleId;
}

const AssemblyID& AssemblyInfo::AssemblyId() const
{
    return m_assemblyId;
}

