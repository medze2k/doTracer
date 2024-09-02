# doTracer

doTracer is a library for tracing dotnet applications.

## Motivation

While native win32 applications have a plethora of tools that can log the Win32 APIs like API Monitor or Win32Override, .NET binaries lacks such support.

Objectives:

- Be able to trace the first layer of managed code, i.e: methods with arguments of the malware code and not the subsystem DLLs.
- Dump memory buffers of intersting methods like Assembly.Load().
- Help deobfuscating .NET protectors.

### Debugging API

### CLR Profiling API

- The profiling API enables a profiler to change the in-memory MSIL code stream for a routine before it is JIT-compiled. In this manner, the profiler can dynamically add instrumentation code to particular routines that need deeper investigation.
- The profiling API is used by a __profiler DLL__, which is loaded into the same process as the application that is being profiled. The profiler DLL implements a callback interface (`ICorProfilerCallback` in the .NET Framework v1.0 and v1.1, `ICorProfilerCallback2` in v2.0 and later). The CLR calls the methods in that interface to notify the profiler of events in the profiled process. The profiler can call back into the runtime by using the methods in the `ICorProfilerInfo` and `ICorProfilerInfo2` interfaces to obtain information about the state of the profiled application.
- A CLR profiler must be completely unmanaged.
- **Cons**:
  - Need to set 2 environment variables `COR_ENABLE_PROFILING` and `COR_PROFILER`.
  - To use .NET Framework versions 2.0, 3.0, and 3.5 profilers in the .NET Framework 4 and later versions, you must set the `COMPLUS_ProfAPI_ProfilerCompatibilitySetting` environment variable.
  - Prior to .NET Framework v4, we must register the profiler DLL.
### JIT Hooking

### IL Binary Modification

### Native Code Patching

Native code patching is implemented by the class `NativeCodeTracer`. We created a command-line tool *ManagedHookBuilder* that helps us create
automated hooks for methods.
The use cases that are not actually addressed yet are shown below:
* Instance method hooking.
  * We have 2 implementation options.
    * Obtaining the method offsets in the memory area of the managed object.
	* Obtaining the RVA of the method in the native image of `mscorlib.ni.dll`.
* Generic methods hooking.
* Hooking methods returning generic types.
* Hooking methods having generic parameters.

The usage example:

```cs
//Create StdoutLogger that will be used to log events to the Console.
ILogger consoleLogger = new StdoutLogger();
//Create NativeCodeTracer that will be responsible for tracing .NET method calls.
NativeCodeTracer tracer = new NativeCodeTracer(
	Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), @"Desktop\Result\TargetApp.exe"),
	consoleLogger);

//Prepare the tracer. This is required because here we prejit all possible required methods ahead of time to avoid crashes.
tracer.Prepare();
//Start the tracer.
tracer.Start();

Console.WriteLine("Tracing done.");
Console.ReadLine();
```

## References

- [BSidesSF 2017 - Hijacking .NET to Defend PowerShell](https://www.youtube.com/watch?v=YXjIVuX6zQk)
- [[CB17] PowerShell Inside Out: Applied .NET Hacking for Enhanced Visibility](https://www.youtube.com/watch?v=EzpJTeFbe8c)
- [JIT Hook for .NET FW/Core](https://github.com/Elliesaur/TinyJitHook)
- [.NET library for hooking and dumping Clr ](https://github.com/GeorgePlotnikov/ClrAnalyzer)

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
