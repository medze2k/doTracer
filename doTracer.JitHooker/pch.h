// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

/* Include patched dotnet runtime headers */
#include "framework.h"
#include "corhdr.h"
#include "corinfo.h"
#include "corjit.h"
#include <stdio.h>

/* Dll Export Macro */
#define JHAPI extern "C" __declspec(dllexport)

/* Pre-Call Logging Function Signature */
typedef void(__stdcall* LogPreCallProc)(LPCSTR className, LPCSTR methodName, int argCount);
typedef LogPreCallProc LOG_PRE_CALL_CALLBACK;

/* compileMethod Function Signature */
typedef int(__stdcall* compileMethodProc)(ULONG_PTR classthis, ICorJitInfo* comp,
    CORINFO_METHOD_INFO* info, unsigned flags,
    BYTE** nativeEntry, ULONG* nativeSizeOfCode);
typedef compileMethodProc COMPILE_METHOD_PROC;

/* Pre-declare compileMethodHook Function */
int __stdcall compileMethodHook(ULONG_PTR classthis, ICorJitInfo* comp,
    CORINFO_METHOD_INFO* info, unsigned flags,
    BYTE** nativeEntry, ULONG* nativeSizeOfCode);

#ifdef TARGET_X86
/* Export 32-bit Jit Hooking APIs */
  JHAPI void HookJit32(LOG_PRE_CALL_CALLBACK preCallLoggerCallback);
  JHAPI void UnhookJit32();

#elif TARGET_AMD64
/* Export 64-bit Jit Hooking APIs */
  JHAPI void HookJit64(LOG_PRE_CALL_CALLBACK preCallLoggerCallback);
  JHAPI void UnhookJit64();

#else
/* Raise an error due to undefined architecture */
  #error "Architecture is undefined. Please define either TARGET_X86 or TARGET_AMD64."
#endif

#endif //PCH_H
