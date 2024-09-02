using System;
using System.Runtime.InteropServices;
using myStringer;

[StructLayout(LayoutKind.Sequential)]
public struct SYSTEM_INFO
{
    public uint dwOemId;
    public uint dwPageSize;
    public uint lpMinimumApplicationAddress;
    public uint lpMaximumApplicationAddress;
    public uint dwActiveProcessorMask;
    public uint dwNumberOfProcessors;
    public uint dwProcessorType;
    public uint dwAllocationGranularity;
    public uint dwProcessorLevel;
    public uint dwProcessorRevision;
}

namespace TestApp
{
    // Example of a class using a private constructor.
    public class Counter
    {
        private Counter() { }

        public static int currentCount;

        public static int IncrementCount()
        {
            return ++currentCount;
        }
    }


    class Program
    {
        [DllImport("kernel32")]
        static extern void GetSystemInfo(ref SYSTEM_INFO pSI);

        static Program()
        {
            Console.WriteLine("\nStatic constructor called.");
        }

        static void Main(string[] args)
        {
            ProfiledMethod();

            SYSTEM_INFO pSI = new SYSTEM_INFO();
            GetSystemInfo(ref pSI);

            // Keep the console window open in debug mode.
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        static void ProfiledMethod()
        {
            Console.WriteLine("\nHello, Simple Profiler!");

            Counter.currentCount = 100;
            Counter.IncrementCount();
            Console.WriteLine("\nNew count: {0}", Counter.currentCount);

            Stringer myStringInstance = new Stringer();
            Console.WriteLine("\nClient code executes");
            myStringInstance.StringerMethod();
        }
    }
}
