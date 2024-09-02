using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doTracer.Loggers
{
    /// <summary>
    /// A logger class that can log events to the console.
    /// </summary>
    public class StdoutLogger : ILogger
    {
        /// <summary>
        /// Creates a StdoutLogger object that can be used to log events to the console.
        /// </summary>
        public StdoutLogger()
        {

        }
        /// <summary>
        /// Logs a string value to the console.
        /// </summary>
        /// <param name="value">The string value that will be logged to the console.</param>
        public void Log(string value)
        {
            Console.WriteLine(value);
        }
        /// <summary>
        /// Flush logged events.
        /// </summary>
        public void Flush()
        {
            Console.Out.Flush();
        }
        /// <summary>
        /// Close the logging.
        /// </summary>
        public void Close()
        {
            //No need to close anything, it is just a console.
        }
    }
}
