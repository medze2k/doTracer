using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace doTracer.Loggers
{
    /// <summary>
    /// This class is responsible for logging event to a file.
    /// </summary>
    public class FileLogger : ILogger
    {
        private StreamWriter _streamWriter;
        /// <summary>
        /// Create the FileLogger that will be responsible for logging event to a file.
        /// </summary>
        /// <param name="filePath">The target log file.</param>
        public FileLogger(string filePath)
        {
            _streamWriter = new StreamWriter(filePath);
        }
        /// <summary>
        /// Log value to a file.
        /// </summary>
        /// <param name="value"></param>
        public void Log(string value)
        {
            _streamWriter.WriteLine(value);
        }
        /// <summary>
        /// Flush logging.
        /// </summary>
        public void Flush()
        {
            _streamWriter.Flush();
        }
        /// <summary>
        /// Close the logger.
        /// </summary>
        public void Close()
        {
            _streamWriter.Flush();
            _streamWriter.Close();
        }
    }
}
