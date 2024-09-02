using doTracer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ManagedHookBuilder
{
    public class ManagedHookBuilder
    {
        private bool _isSystem;
        private string _assembly;
        private string _type;
        private string _method;
        private string _signature;
        public ManagedHookBuilder(bool isSystem, string assembly, string type, string method, string signature)
        {
            _isSystem = isSystem;
            _assembly = assembly;
            _type = type;
            _method = method;
            _signature = signature;
        }

        public void BuildSources(string path)
        {
            if (!_isSystem)
            {
                throw new NotImplementedException("Non System Hooking not implemenetd.");
            }

            StringBuilder source = new StringBuilder();
            Type type = Type.GetType(_type);
            Console.WriteLine("[INFO]: Type = {0}", _type);
            if (_method != null)
            {
                //Build source for only one method
                MethodInfo mi = null;
                try
                {
                    if (_signature != null)
                    {
                        string[] paramTypes = _signature.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        Type[] signatureTypes = paramTypes.Select(s => Type.GetType(s)).ToArray();
                        mi = type.GetMethod(_method, signatureTypes);
                    }
                    else
                    {
                        mi = type.GetMethod(_method);
                    }
                }
                catch (AmbiguousMatchException)
                {
                    MethodInfo[] miArray = typeof(Console).GetMethods().Where(m => m.Name == _method).ToArray();
                    Console.WriteLine("Ambiguous Match found. Please find more details below:");
                    for (int i = 0; i < miArray.Length; i++)
                    {
                        ParameterInfo[] parameters = miArray[i].GetParameters();
                        string signature = string.Join(",", parameters.Select(p => p.ParameterType.FullName).ToArray());
                        Console.WriteLine("[{0}] {1}({2})", i, miArray[i].Name, signature);
                    }
                    throw;
                }

                if (mi == null)
                {
                    throw new ArgumentException("Method not found.");
                }

                string head = BuildSourceForTypeHead(type.Namespace, type.Name);
                string method = BuildSourceForMethod(mi);
                string tail = BuildSourceForTypeTail();
                source.AppendLine(head);
                source.AppendLine(method);
                source.AppendLine(tail);
                string sourceFile = Path.Combine(path, type.Namespace.Replace(".", "\\"), type.Name) + "Hook.cs";
                File.WriteAllText(sourceFile, source.ToString());
            }
            else
            {
                //Build source for all methods
                MethodInfo[] methodInfos = type.GetMethods();
                string head = BuildSourceForTypeHead(type.Namespace, type.Name);
                source.AppendLine(head);
                for (int i = 0; i < methodInfos.Length; i++)
                {
                    if (!methodInfos[i].IsSpecialName && !methodInfos[i].IsGenericMethod)
                    {
                        string method = BuildSourceForMethod(methodInfos[i]);
                        source.AppendLine(method);
                    }
                }
                string tail = BuildSourceForTypeTail();
                source.AppendLine(tail);
                string sourceFile = Path.Combine(path, type.Namespace.Replace(".", "\\"), type.Name) + "Hook.cs";
                Directory.CreateDirectory(Path.Combine(path, type.Namespace.Replace(".", "\\")));
                File.WriteAllText(sourceFile, source.ToString());
            }
        }
        private string BuildSourceForTypeHead(string nameSpace, string typeName)
        {
            StringBuilder sbSource = new StringBuilder();

            sbSource.AppendLine("using doTracer.Loggers;");
            sbSource.AppendLine("using System;");
            sbSource.AppendLine("using System.Collections.Generic;");
            sbSource.AppendLine("using System.IO;");
            sbSource.AppendLine("using System.Linq;");
            sbSource.AppendLine("using System.Runtime.CompilerServices;");
            sbSource.AppendLine("using System.Runtime.InteropServices;");
            sbSource.AppendLine("using System.Text;");
            sbSource.AppendLine("using System.Threading.Tasks;");
            sbSource.AppendLine("using System.Diagnostics;");
            sbSource.AppendLine("");
            sbSource.AppendFormat("namespace doTracer.ManagedHooks.{0}\n", nameSpace);
            sbSource.AppendLine("{");
            sbSource.AppendFormat("    public class {0}Hook\n", typeName);
            sbSource.AppendLine("    {");

            return sbSource.ToString();
        }
        private string BuildSourceForTypeTail()
        {
            StringBuilder sbSource = new StringBuilder();

            sbSource.AppendLine("    }");
            sbSource.AppendLine("}");

            return sbSource.ToString();
        }
        private string BuildSourceForMethod(MethodInfo methodInfo)
        {
            if (methodInfo.IsGenericMethod)
            {
                return "        //Generic method unsupported: " + methodInfo.Name;
            }
            if (!methodInfo.IsStatic)
            {
                return "        //Instance method unsupported: " + methodInfo.Name;
            }
            StringBuilder sbSource = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            StringBuilder sbParamNames = new StringBuilder();
            ParameterInfo[] parameters = methodInfo.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                sbParams.AppendFormat("global::{0} {1}",
                    ConvertTypeToSource(parameters[i].ParameterType),
                    parameters[i].Name);
                sbParamNames.AppendFormat("{0}",
                    parameters[i].Name);
                if (i < parameters.Length - 1)
                {
                    sbParams.Append(",");
                    sbParamNames.Append(",");
                }
            }

            sbSource.AppendLine("        [MethodImpl(MethodImplOptions.NoInlining)]");
            sbSource.AppendFormat("        public {0} {1} {2}({3})\n", 
                methodInfo.IsStatic ? "static": "",
                ConvertTypeToSource(methodInfo.ReturnType) == "System.Void" ? "void": "global::" + ConvertTypeToSource(methodInfo.ReturnType),
                methodInfo.Name,
                sbParams.ToString());
            sbSource.AppendLine("        {");
            MethodId methodId = new MethodId(methodInfo);
            sbSource.AppendFormat("            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get(\"{0}\");\n",
                methodId.ToString());
            sbSource.AppendLine("            lock (ManagedHookDescriptorManager.ManagedHookLock)");
            sbSource.AppendLine("            {");
            sbSource.AppendLine("                StackFrame sf = new StackFrame(1);");
            sbSource.AppendLine("                Console.WriteLine(\"Called from {0}\", sf.GetMethod().Module.Assembly.Location);");
            //Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
            string argNames = string.Join(", ", parameters.Select(p => "\"" +  p.Name + "\""));
            sbSource.AppendFormat("                GlobalLogger.LogPreCall(\"{0}\", \"{1}\", new string[]{{{2}}}, new object[]{{{3}}});\n", 
                methodInfo.DeclaringType.FullName,
                methodInfo.Name,
                argNames,
                sbParamNames.ToString());
            sbSource.AppendLine("                mhd.Revert();");
            if (ConvertTypeToSource(methodInfo.ReturnType) == "System.Void")
            {
                sbSource.AppendFormat("                global::{0}.{1}({2});\n", 
                    methodInfo.DeclaringType.FullName,
                    methodInfo.Name,
                    sbParamNames.ToString());
            }
            else
            {
                sbSource.AppendFormat("                global::{3} retVal = global::{0}.{1}({2});\n",
                    methodInfo.DeclaringType.FullName,
                    methodInfo.Name,
                    sbParamNames.ToString(),
                    ConvertTypeToSource(methodInfo.ReturnType));
            }
            sbSource.AppendLine("                mhd.Apply();");
            if (ConvertTypeToSource(methodInfo.ReturnType) == "System.Void")
            {
                sbSource.AppendFormat("                GlobalLogger.LogPostCall(\"{0}\", \"{1}\", \"N/A\");\n",
                    methodInfo.DeclaringType.FullName,
                    methodInfo.Name);
            }
            else
            {
                sbSource.AppendFormat("                GlobalLogger.LogPostCall(\"{0}\", \"{1}\", retVal);\n",
                        methodInfo.DeclaringType.FullName,
                        methodInfo.Name);
            }
            if (ConvertTypeToSource(methodInfo.ReturnType) != "System.Void")
            {
                sbSource.AppendLine("                return retVal;");
            }
            sbSource.AppendLine("            }");
            sbSource.AppendLine("        }");

            return sbSource.ToString();
        }
        private string ConvertTypeToSource(Type type)
        {
            if (type.IsGenericType)
            {
                string sourceForm = type.ToString().Split(new string[] { "`" }, StringSplitOptions.RemoveEmptyEntries).First();
                string genericParams = string.Join(",", type.GenericTypeArguments.Select(t => "global::" + ConvertTypeToSource(t)));
                sourceForm = sourceForm + "<" + genericParams + ">";
                return sourceForm;
            }
            return type.FullName;
        }
    }
}
