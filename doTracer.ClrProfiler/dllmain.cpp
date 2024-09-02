// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "ClassFactory.h"

const IID IID_IUnknown      = { 0x00000000, 0x0000, 0x0000, { 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46 } };
const IID IID_IClassFactory = { 0x00000001, 0x0000, 0x0000, { 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46 } };


BOOL STDMETHODCALLTYPE DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    // Perform actions based on the reason for calling.
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    {
        // Perform process initialization.
        break;
    }
    case DLL_THREAD_ATTACH:
        // Do thread-specific initialization.
        break;

    case DLL_THREAD_DETACH:
        // Do thread-specific cleanup.
        break;

    case DLL_PROCESS_DETACH:
        // Perform any necessary cleanup.
        break;
    }

    return TRUE; // Successful DLL_PROCESS_ATTACH.
}

// Returns a class factory to create an object of the requested type.
_Check_return_
STDAPI DllGetClassObject(_In_ REFCLSID rclsid, _In_ REFIID riid, _Outptr_ LPVOID FAR* ppv)
{
    // {cf0d821e-299b-5307-a3d8-b283c03916dd}
    const GUID CLSID_CorProfiler = { 0xcf0d821e, 0x299b, 0x5307, { 0xa3, 0xd8, 0xb2, 0x83, 0xc0, 0x39, 0x16, 0xdd } };

    if (ppv == nullptr || rclsid != CLSID_CorProfiler)
    {
        return E_FAIL;
    }

    auto factory = new ClassFactory;
    if (factory == nullptr)
    {
        return E_FAIL;
    }

    return factory->QueryInterface(riid, ppv);
}

// Used to determine whether the DLL can be unloaded by OLE.
__control_entrypoint(DllExport)
STDAPI DllCanUnloadNow(void)
{
    return S_OK;
}