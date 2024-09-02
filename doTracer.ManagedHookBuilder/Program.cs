using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedHookBuilder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                if (!arg.StartsWith("--"))
                {
                    Console.WriteLine("Invalid command line detected. Please run --help for help.");
                    return;
                }
            }
            bool? isSystem = null;
            string assembly = null;
            string type = null;
            string method = null;
            string output = null;
            string signature = null;
            foreach (var arg in args)
            {
                string[] cmd = arg.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                switch (cmd[0])
                {
                    case "--help":
                        {
                            if (args.Length != 1)
                            {
                                Console.WriteLine("Argument count invalid for --help. Exiting.");
                                return;
                            }
                            else
                            {
                                HandleHelp();
                            }
                            break;
                        }
                    case "--is-system":
                        {
                            if (cmd.Length != 2)
                            {
                                Console.WriteLine("Invalid value for --is-system. Exiting.");
                                return;
                            }
                            if (cmd[1] == "yes")
                            {
                                isSystem = true;
                            }
                            else if (cmd[1] == "no")
                            {
                                isSystem = false;
                            }
                            else
                            {
                                Console.WriteLine("Unknown value detected for --is-system. Exiting.");
                                return;
                            }
                            break;
                        }
                    case "--assembly":
                        {
                            if (cmd.Length != 2)
                            {
                                Console.WriteLine("Invalid value for --assembly. Exiting.");
                                return;
                            }
                            assembly = cmd[1];
                            break;
                        }
                    case "--type":
                        {
                            if (cmd.Length != 2)
                            {
                                Console.WriteLine("Invalid value for --type. Exiting.");
                                return;
                            }
                            type = cmd[1];
                            break;
                        }
                    case "--method":
                        {
                            if (cmd.Length != 2)
                            {
                                Console.WriteLine("Invalid value for --method. Exiting.");
                                return;
                            }
                            method = cmd[1];
                            break;
                        }
                    case "--signature":
                        {
                            if (cmd.Length == 2)
                            {
                                signature = cmd[1];
                            }
                            else
                            {
                                signature = "";
                            }
                            break;
                        }
                    case "--output":
                        {
                            if (cmd.Length != 2)
                            {
                                Console.WriteLine("Invalid value for --output. Exiting.");
                                return;
                            }
                            output = cmd[1];
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Unknown command detected. Exiting.");
                            return;
                        }
                }
            }
            if (!isSystem.HasValue)
            {
                Console.WriteLine("Missing required --is-system parameter. Exiting.");
                return;
            }
            if (!isSystem.Value && assembly == null)
            {
                Console.WriteLine("Missing required --assembly parameter when --is-system=no. Exiting.");
                return;
            }
            if (type == null)
            {
                Console.WriteLine("Missing required --type parameter. Exiting.");
                return;
            }
            if (output == null)
            {
                Console.WriteLine("Missing required --output parameter. Exiting.");
                return;
            }

            ManagedHookBuilder mhb = new ManagedHookBuilder(isSystem.Value, assembly, type, method, signature);
            mhb.BuildSources(output);

            Console.WriteLine("Sources were saved successfully in {0}.", output);
        }
        static void HandleHelp()
        {
            Console.WriteLine("ManagedHookBuilder");
            Console.WriteLine("ManagedHookBuilder is a helper tool for the doTracer project.");
            Console.WriteLine("");
            Console.WriteLine("This tool help us automate generation of source code for managed .NET hooks.");
            Console.WriteLine("Supported commands:");
            Console.WriteLine("");
            Console.WriteLine("  --is-system= : Required, [yes or no]. Is the assembly that we want to hook is a System assembly.");
            Console.WriteLine("  --assembly= : Required if --is-system=no. The location of the .NET assembly for which we want to create managed .NET hooks.");
            Console.WriteLine("  --type= : Required. The full name of the type for which we want to create managed .NET hooks.");
            Console.WriteLine("  --method= : Optional. The name of the method for which we want to create managed .NET hooks. When this argumented is omitted, hooks will be generated for all methods of the specified type.");
            Console.WriteLine("  --signature= : Optional. The signature of the method for which we want to create managed .NET hooks. This argument maybe required when the method name alone is ambiguous (eg. System.String).");
            Console.WriteLine("  --output= : Required. Specify source code generation output directory for doTracer (eg. C:\\doTracer\\ManagedHooks\\).");
        }
    }
}
