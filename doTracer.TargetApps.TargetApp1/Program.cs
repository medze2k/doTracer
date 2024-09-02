using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to CTF.MA");
            Sample();

            Console.ReadLine();
        }

        static void Sample()
        {
            string data = Convert.ToBase64String(Encoding.ASCII.GetBytes("Welcome to Morocco!")); ;
            Console.WriteLine(data);
        }
    }
}
