using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace doTracer.Tracers
{
    /// <summary>
    /// This interface describes methods that should be implemeneted by Tracers.
    /// </summary>
    public interface ITracer
    {
        /// <summary>
        /// Implement the start code of tracing.
        /// </summary>
        void Start();
        /// <summary>
        /// Implement the stop code of tracing.
        /// </summary>
        void Stop();
    }
}