# CLR Profiler

### Setup (on .NET 4.6.1+ CLR)

This profiler is also capable of running on the .NET 4.6.1 (or higher) CLR.

```batch
SET COR_PROFILER={cf0d821e-299b-5307-a3d8-b283c03916dd}
SET COR_ENABLE_PROFILING=1
SET COR_PROFILER_PATH=C:\filePath\to\ClrProfiler.dll
YourProgram.exe
```

### Multi module assembly

call "%VS140COMNTOOLS%\vsvars32.bat"
csc /t:module $(ProjectDir)\Stringer.cs

call "%VS140COMNTOOLS%\vsvars32.bat"
csc /addmodule:Stringer.netmodule /t:module $(ProjectDir)\Program.cs

## References

- [Start a journey into the .NET Profiling APIs](https://chnasarre.medium.com/start-a-journey-into-the-net-profiling-apis-40c76e2e36cc)
- [Profiler Stack Walking in the .NET Framework 2.0: Basics and Beyond](https://docs.microsoft.com/en-us/previous-versions/dotnet/articles/bb264782(v=msdn.10)?redirectedfrom=MSDN)
- [Enter, Leave, Tailcall Hooks Part 1: The Basics](https://docs.microsoft.com/en-us/archive/blogs/davbr/enter-leave-tailcall-hooks-part-1-the-basics)
- https://docs.microsoft.com/en-us/archive/blogs/davbr/
- [Sample: A Signature Blob Parser for your Profiler](https://docs.microsoft.com/en-us/archive/blogs/davbr/sample-a-signature-blob-parser-for-your-profiler)