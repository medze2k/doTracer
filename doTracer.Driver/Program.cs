using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using doTracer.Loggers;
using doTracer.Tracers;

namespace Driver
{
    internal class Program
    {
        /// <summary>
        /// This is a driver program that demonstrates a simple usage of NativeCodeTracer class.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
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
        }
    }
}
