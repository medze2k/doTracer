using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace doTracer.Loggers
{
    /// <summary>
    /// This interface describes methods that should be implemeneted by Loggers.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Implement the logging of a string value.
        /// </summary>
        /// <param name="value">The string value that should be logged.</param>
        void Log(string value);
        /// <summary>
        /// Implement the flushing operation.
        /// </summary>
        void Flush();
        /// <summary>
        /// Implement the closing operation.
        /// </summary>
        void Close();
    }
}
