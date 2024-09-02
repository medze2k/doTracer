#include "ModuleInfo.h"


ModuleInfo::ModuleInfo(ICorProfilerInfo* pCorProfilerInfo, ModuleID ModuleId) :
    m_pCorProfilerInfo(pCorProfilerInfo),
    m_moduleId(ModuleId),
    m_assemblyId(0),
    m_ModuleName(MAX_PATH, 0)
{

}

bool ModuleInfo::IsDotNetDll()
{
    std::wstring AssemblyGAC;
    std::wstring AssemblyGACv4;
    std::wstring CurrentModule;
    std::wstring WindowsRoot;
    std::wstring Buffer(MAX_PATH, 0);

    auto result = GetWindowsDirectoryW(&Buffer[0], MAX_PATH);
    _ASSERT_EXPR(result > 0, "GetWindowsDirectoryW failed");
    Buffer.resize(result);

    WindowsRoot.resize(Buffer.size());
    std::transform(Buffer.begin(), Buffer.end(),
       WindowsRoot.begin(), ::tolower);
    //
    // Prior to .NET Framework 4 location for the Global Assembly Cache/
    // The directory below also contains native images.
    //

   AssemblyGAC = WindowsRoot + L"\\assembly\\";

    //
    // Default .NET Framework 4 location for the Global Assembly Cache.
    //

    AssemblyGACv4 = WindowsRoot + L"\\microsoft.net\\assembly\\";

    //
    // Lower case the currently loaded module name and compare.
    //

    CurrentModule.resize(m_ModuleName.size());
    std::transform(m_ModuleName.begin(), m_ModuleName.end(),
        CurrentModule.begin(), ::tolower);

    if ((CurrentModule.rfind(AssemblyGAC, 0) == 0) ||
        (CurrentModule.rfind(AssemblyGACv4, 0) == 0)) {
        return true;
    }

    return false;
}

const std::wstring ModuleInfo::Name()
{
    HRESULT hr = S_FALSE;
    ULONG ModuleNameLength = 0;

    hr = m_pCorProfilerInfo->GetModuleInfo(
        m_moduleId, NULL, MAX_PATH, &ModuleNameLength,
        &m_ModuleName[0], &m_assemblyId);

    if (FAILED(hr)) {
        return std::wstring();
    }

    m_ModuleName.resize(ModuleNameLength);
    return m_ModuleName;
}

const ModuleID& ModuleInfo::ModuleId() const
{
    return m_moduleId;
}

const AssemblyID& ModuleInfo::AssemblyId() const
{
    return m_assemblyId;
}
