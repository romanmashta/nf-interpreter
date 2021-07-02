//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

#pragma once

#ifdef NANOCLRNATIVE_EXPORTS
#define NANOCLRNATIVE_API __declspec(dllexport)
#else
#define NANOCLRNATIVE_API __declspec(dllimport)
#endif

typedef struct NANO_CLR_SETTINGS
{
    // this is the maximum number of context switches that execution engine thread scheduler can handle
    // higher number: more threads and timers can be handled
    unsigned short MaxContextSwitches;

    // set this to TRUE if the default behaviour is for the execution engine to wait for a debugger to be connected
    // when building is set for RTM this configuration is ignored
    bool WaitForDebugger;

    // set this to TRUE if a connection from a debugger is to be awaited after the execution engine terminates
    // this is required for launching a debug session in Visual Studio
    // when building is set for RTM this configuration is ignored
    bool EnterDebuggerLoopAfterExit;
} NANO_CLR_SETTINGS;

typedef HRESULT(__stdcall *ConfigureRuntimeCallback)();
typedef void(__stdcall *DebugPrintCallback)(const char *szText);

extern DebugPrintCallback gDebugPrintCallback;

extern "C" NANOCLRNATIVE_API void NanoClr_Run(NANO_CLR_SETTINGS nanoClrSettings);
extern "C" NANOCLRNATIVE_API HRESULT NanoClr_ReferenceAssembly(const wchar_t *szFile, const CLR_UINT8 *data, size_t size);
extern "C" NANOCLRNATIVE_API HRESULT NanoClr_ReferenceAssemblySet(const CLR_UINT8 *data, size_t size);
extern "C" NANOCLRNATIVE_API HRESULT NanoClr_Resolve();

extern "C" NANOCLRNATIVE_API void NanoClr_SetConfigureCallback(ConfigureRuntimeCallback configureRuntimeCallback);
extern "C" NANOCLRNATIVE_API void NanoClr_SetDebugPrintCallback(DebugPrintCallback debugPrintCallback);