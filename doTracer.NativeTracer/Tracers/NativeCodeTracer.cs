using doTracer.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace doTracer.Tracers
{
    /// <summary>
    /// This class implements Native Code Tracing. It can log managed .NET method calls of a .NET application.
    /// </summary>
    public class NativeCodeTracer : ITracer
    {
        private string _applicationPath;
        private ILogger _logger;
        private AppDomain _ad;
        private bool _prepared;
        /// <summary>
        /// Create a Native Code Tracing object.
        /// </summary>
        /// <param name="applicationPath">The path of the .NET application.</param>
        /// <param name="logger">An instance of ILogger.</param>
        public NativeCodeTracer(string applicationPath, ILogger logger)
        {
            _applicationPath = applicationPath;
            _logger = logger;
            _prepared = false;
        }
        /// <summary>
        /// Perpares the tracer. This is important because it jit-compiles logging related methods ahead of time.
        /// </summary>
        public void Prepare()
        {
            if (_prepared)
            {
                return;
            }
            MethodInfo[] methodInfos = typeof(GlobalLogger).GetMethods().ToArray();
            for (int i = 0; i < methodInfos.Length; i++)
            {
                RuntimeHelpers.PrepareMethod(methodInfos[i].MethodHandle);
            }
            //TODO: Find an elegant way to prepare all possible loggers
            MethodInfo stdoutLoggerMethod = typeof(StdoutLogger).GetMethod("Log");
            MethodInfo fileLoggerMethod = typeof(FileLogger).GetMethod("Log");
            RuntimeHelpers.PrepareMethod(stdoutLoggerMethod.MethodHandle);
            RuntimeHelpers.PrepareMethod(fileLoggerMethod.MethodHandle);
            _prepared = true;
        }
        /// <summary>
        /// Start the Native Code Tracing.
        /// </summary>
        public void Start()
        {
            if (!_prepared)
            {
                throw new InvalidOperationException("Can not start tracing when the tracer is unprepared");
            }
            GlobalLogger.Configure(_logger);
            ManagedHookManager mhm = new ManagedHookManager();
            mhm.HookAll();
            _ad = AppDomain.CreateDomain("CustomAppDomain");
            _ad.ExecuteAssembly(_applicationPath);
        }
        /// <summary>
        /// Stop the Native Code Tracing.
        /// </summary>
        public void Stop()
        {
            //TODO: Find a way to kill the app..
        }
    }
}
