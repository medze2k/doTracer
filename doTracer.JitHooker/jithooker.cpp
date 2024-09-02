#include "pch.h"

/* Save the Pre-Call Logging function as a global variable when hooking */
LOG_PRE_CALL_CALLBACK preCallLoggerCallbackPtr;
/* Is the Jit compiler Hooked */
BOOL bJitHooked = FALSE;
/* Save Real compileMethod */
COMPILE_METHOD_PROC compileMethod;
/* Save getJit Function */
ULONG_PTR* (__stdcall* getJitProc)();

/* ICorJitCompiler Mock structure */
struct ICorJitCompilerMock
{
    COMPILE_METHOD_PROC compileMethod;
    /* ... Other members are not needed ... */
};


#ifdef TARGET_X86
JHAPI void HookJit32(LOG_PRE_CALL_CALLBACK preCallLoggerCallback)
{
    /* Check if the JIT Compiler was already hooked */
    if (bJitHooked)
        return;
    /* Save the Pre-Call Logger Callback */
    preCallLoggerCallbackPtr = preCallLoggerCallback;
    /* Load Execution Engine (EE) */
    LoadLibrary(TEXT("mscoree.dll"));
    /* Load JIT Library */
    HMODULE hClrJit = LoadLibrary(TEXT("clrjit.dll"));
    if (!hClrJit)
        return;
    /* Obtain the getJit function */
    getJitProc = (ULONG_PTR * (__stdcall*)()) GetProcAddress(hClrJit, "getJit");
    if (getJitProc)
    {
        /* Obtain the address of ICorJitCompiler */
        /* See: https://github.com/dotnet/runtime/blob/b61c6f6fbd5c2b1c4cf24470564e36f2265c21a4/src/coreclr/inc/corjit.h#L179 */
        ICorJitCompilerMock* pCorJitCompiler = (ICorJitCompilerMock*)*((ULONG_PTR*)getJitProc());
        if (pCorJitCompiler)
        {
            /* Hook compileMethod function */
            DWORD OldProtect;
            VirtualProtect(pCorJitCompiler, sizeof(ULONG_PTR), PAGE_READWRITE, &OldProtect);
            compileMethod = pCorJitCompiler->compileMethod;
            pCorJitCompiler->compileMethod = &compileMethodHook;
            VirtualProtect(pCorJitCompiler, sizeof(ULONG_PTR), OldProtect, &OldProtect);
            bJitHooked = TRUE;
        }
    }
}

JHAPI void UnhookJit32()
{

}

#elif TARGET_AMD64

#else
#error "Architecture is undefined. Please define either TARGET_X86 or TARGET_AMD64."
#endif

int __stdcall compileMethodHook(ULONG_PTR classthis, ICorJitInfo* comp,
    CORINFO_METHOD_INFO* info, unsigned flags,
    BYTE** nativeEntry, ULONG* nativeSizeOfCode)
{
    // in case somebody hooks us (x86 only)
#ifdef TARGET_X86
    __asm
    {
        nop
        nop
        nop
        nop
        nop
        nop
        nop
        nop
        nop
        nop
        nop
        nop
        nop
      //int 3 //for debugging only
    }
#endif
    const char* szMethodName = NULL;
    const char* szClassName = NULL;
    CORINFO_SIG_INFO sig;
    int argCount;

    szMethodName = comp->getMethodName(info->ftn, &szClassName);
    comp->getMethodSig(info->ftn, &sig);
    argCount = sig.numArgs;

    preCallLoggerCallbackPtr(szClassName, szMethodName, argCount);
    int nRet = compileMethod(classthis, comp, info, flags, nativeEntry, nativeSizeOfCode);
    return nRet;
}
