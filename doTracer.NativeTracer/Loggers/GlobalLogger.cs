using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doTracer.Loggers
{
    /// <summary>
    /// A configurable Global Logger that is used by Tracers.
    /// </summary>
    public class GlobalLogger
    {
        private static ILogger _logger;
        /// <summary>
        /// Configure the Global Logger by setting an ILogger instance.
        /// </summary>
        /// <param name="logger">An instance of ILogger (see FileLogger or StdoutLogger).</param>
        public static void Configure(ILogger logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Log the class name, method name, argument names and argument values of a .NET method before calling it.
        /// </summary>
        /// <param name="className">The class name of the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="argNames">The argument names.</param>
        /// <param name="argValues">The string values of the arguments.</param>
        /// <exception cref="InvalidOperationException">This exception is thrown when no ILogger instance is configured.</exception>
        public static void LogPreCall(string className, string methodName, string[] argNames, object[] argValues)
        {
            if (_logger == null)
            {
                throw new InvalidOperationException("GlobalLogger is not configured with an ILogger instance.");
            }
            StringBuilder parameters = new StringBuilder();
            for (int i = 0; i < argNames.Length; i++)
            {
                parameters.AppendFormat("{0}={1}", argNames[i], argValues[i]);
                if (i < argNames.Length - 1)
                {
                    parameters.Append(",");
                }
            }
            string log = string.Format("[{0}] : {1}::{2} will be called with parameters {3}.", 
                DateTime.Now.ToString(), className, methodName, parameters.ToString());
            if (_logger is StdoutLogger)
            {
                (_logger as StdoutLogger).Log(log);
            }
            else if (_logger is FileLogger)
            {
                (_logger as FileLogger).Log(log);
            }
            else
            {
                throw new InvalidOperationException("Unknown logger.");
            }
            //NOTE: The line below did not work well maybe due to unknown unprepared (unjitted) code.
            //I needed to cast the interface to the actual object to avoid program crash.
            //_logger.Log(log);
        }
        /// <summary>
        /// Log the class name, method name and return value, if applicable, of a .NET method after calling it.
        /// </summary>
        /// <param name="className">The class name of the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="returnValue">The retuned value from the call, if applicable.</param>
        /// <exception cref="InvalidOperationException">This exception is thrown when no ILogger instance is configured.</exception>
        public static void LogPostCall(string className, string methodName, object returnValue)
        {
            if (_logger == null)
            {
                throw new InvalidOperationException("GlobalLogger is not configured with an ILogger instance.");
            }
            string log = string.Format("[{0}] : {1}::{2} returned value {3}.", DateTime.Now.ToString(), 
                className, methodName, returnValue);
            if (_logger is StdoutLogger)
            {
                (_logger as StdoutLogger).Log(log);
            }
            else if (_logger is FileLogger)
            {
                (_logger as FileLogger).Log(log);
            }
            else
            {
                throw new InvalidOperationException("Unknown logger.");
            }
            //NOTE: The line below did not work well maybe due to unknown unprepared (unjitted) code.
            //I needed to cast the interface to the actual object to avoid program crash.
            //_logger.Log(log);
        }
    }
}
