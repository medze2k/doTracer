using doTracer.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace doTracer.ManagedHooks.System.Reflection
{
    public class AssemblyHook
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly GetAssembly(global::System.Type type)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.GetAssembly(System.Type)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "GetAssembly", new string[]{"type"}, new object[]{type});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.GetAssembly(type);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "GetAssembly", retVal);
                return retVal;
            }
        }

        //Instance method unsupported: Equals
        //Instance method unsupported: GetHashCode
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadFrom(global::System.String assemblyFile)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadFrom(System.String)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadFrom", new string[]{"assemblyFile"}, new object[]{assemblyFile});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadFrom(assemblyFile);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadFrom", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly ReflectionOnlyLoadFrom(global::System.String assemblyFile)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.ReflectionOnlyLoadFrom(System.String)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "ReflectionOnlyLoadFrom", new string[]{"assemblyFile"}, new object[]{assemblyFile});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.ReflectionOnlyLoadFrom(assemblyFile);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "ReflectionOnlyLoadFrom", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadFrom(global::System.String assemblyFile,global::System.Security.Policy.Evidence securityEvidence)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadFrom(System.String,System.Security.Policy.Evidence)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadFrom", new string[]{"assemblyFile", "securityEvidence"}, new object[]{assemblyFile,securityEvidence});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadFrom(assemblyFile,securityEvidence);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadFrom", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadFrom(global::System.String assemblyFile,global::System.Security.Policy.Evidence securityEvidence,global::System.Byte[] hashValue,global::System.Configuration.Assemblies.AssemblyHashAlgorithm hashAlgorithm)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadFrom(System.String,System.Security.Policy.Evidence,System.Byte[],System.Configuration.Assemblies.AssemblyHashAlgorithm)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadFrom", new string[]{"assemblyFile", "securityEvidence", "hashValue", "hashAlgorithm"}, new object[]{assemblyFile,securityEvidence,hashValue,hashAlgorithm});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadFrom(assemblyFile,securityEvidence,hashValue,hashAlgorithm);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadFrom", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadFrom(global::System.String assemblyFile,global::System.Byte[] hashValue,global::System.Configuration.Assemblies.AssemblyHashAlgorithm hashAlgorithm)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadFrom(System.String,System.Byte[],System.Configuration.Assemblies.AssemblyHashAlgorithm)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadFrom", new string[]{"assemblyFile", "hashValue", "hashAlgorithm"}, new object[]{assemblyFile,hashValue,hashAlgorithm});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadFrom(assemblyFile,hashValue,hashAlgorithm);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadFrom", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly UnsafeLoadFrom(global::System.String assemblyFile)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.UnsafeLoadFrom(System.String)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "UnsafeLoadFrom", new string[]{"assemblyFile"}, new object[]{assemblyFile});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.UnsafeLoadFrom(assemblyFile);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "UnsafeLoadFrom", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.String assemblyString)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.String)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"assemblyString"}, new object[]{assemblyString});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(assemblyString);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly ReflectionOnlyLoad(global::System.String assemblyString)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.ReflectionOnlyLoad(System.String)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "ReflectionOnlyLoad", new string[]{"assemblyString"}, new object[]{assemblyString});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.ReflectionOnlyLoad(assemblyString);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "ReflectionOnlyLoad", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.String assemblyString,global::System.Security.Policy.Evidence assemblySecurity)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.String,System.Security.Policy.Evidence)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"assemblyString", "assemblySecurity"}, new object[]{assemblyString,assemblySecurity});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(assemblyString,assemblySecurity);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.Reflection.AssemblyName assemblyRef)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.Reflection.AssemblyName)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"assemblyRef"}, new object[]{assemblyRef});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(assemblyRef);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.Reflection.AssemblyName assemblyRef,global::System.Security.Policy.Evidence assemblySecurity)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.Reflection.AssemblyName,System.Security.Policy.Evidence)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"assemblyRef", "assemblySecurity"}, new object[]{assemblyRef,assemblySecurity});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(assemblyRef,assemblySecurity);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadWithPartialName(global::System.String partialName)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadWithPartialName(System.String)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadWithPartialName", new string[]{"partialName"}, new object[]{partialName});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadWithPartialName(partialName);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadWithPartialName", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadWithPartialName(global::System.String partialName,global::System.Security.Policy.Evidence securityEvidence)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadWithPartialName(System.String,System.Security.Policy.Evidence)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadWithPartialName", new string[]{"partialName", "securityEvidence"}, new object[]{partialName,securityEvidence});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadWithPartialName(partialName,securityEvidence);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadWithPartialName", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.Byte[] rawAssembly)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.Byte[])System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"rawAssembly"}, new object[]{rawAssembly});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(rawAssembly);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly ReflectionOnlyLoad(global::System.Byte[] rawAssembly)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.ReflectionOnlyLoad(System.Byte[])System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "ReflectionOnlyLoad", new string[]{"rawAssembly"}, new object[]{rawAssembly});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.ReflectionOnlyLoad(rawAssembly);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "ReflectionOnlyLoad", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.Byte[] rawAssembly,global::System.Byte[] rawSymbolStore)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.Byte[],System.Byte[])System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"rawAssembly", "rawSymbolStore"}, new object[]{rawAssembly,rawSymbolStore});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(rawAssembly,rawSymbolStore);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.Byte[] rawAssembly,global::System.Byte[] rawSymbolStore,global::System.Security.SecurityContextSource securityContextSource)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.Byte[],System.Byte[],System.Security.SecurityContextSource)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"rawAssembly", "rawSymbolStore", "securityContextSource"}, new object[]{rawAssembly,rawSymbolStore,securityContextSource});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(rawAssembly,rawSymbolStore,securityContextSource);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly Load(global::System.Byte[] rawAssembly,global::System.Byte[] rawSymbolStore,global::System.Security.Policy.Evidence securityEvidence)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.Load(System.Byte[],System.Byte[],System.Security.Policy.Evidence)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "Load", new string[]{"rawAssembly", "rawSymbolStore", "securityEvidence"}, new object[]{rawAssembly,rawSymbolStore,securityEvidence});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.Load(rawAssembly,rawSymbolStore,securityEvidence);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "Load", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadFile(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadFile(System.String)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadFile", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadFile(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadFile", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly LoadFile(global::System.String path,global::System.Security.Policy.Evidence securityEvidence)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.LoadFile(System.String,System.Security.Policy.Evidence)System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "LoadFile", new string[]{"path", "securityEvidence"}, new object[]{path,securityEvidence});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.LoadFile(path,securityEvidence);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "LoadFile", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly GetExecutingAssembly()
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.GetExecutingAssembly()System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "GetExecutingAssembly", new string[]{}, new object[]{});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.GetExecutingAssembly();
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "GetExecutingAssembly", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly GetCallingAssembly()
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.GetCallingAssembly()System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "GetCallingAssembly", new string[]{}, new object[]{});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.GetCallingAssembly();
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "GetCallingAssembly", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Reflection.Assembly GetEntryAssembly()
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.GetEntryAssembly()System.Reflection.Assembly:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "GetEntryAssembly", new string[]{}, new object[]{});
                mhd.Revert();
                global::System.Reflection.Assembly retVal = global::System.Reflection.Assembly.GetEntryAssembly();
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "GetEntryAssembly", retVal);
                return retVal;
            }
        }

        //Instance method unsupported: GetName
        //Instance method unsupported: GetName
        //Instance method unsupported: GetType
        //Instance method unsupported: GetType
        //Instance method unsupported: GetType
        //Instance method unsupported: GetExportedTypes
        //Instance method unsupported: GetTypes
        //Instance method unsupported: GetManifestResourceStream
        //Instance method unsupported: GetManifestResourceStream
        //Instance method unsupported: GetSatelliteAssembly
        //Instance method unsupported: GetSatelliteAssembly
        //Instance method unsupported: GetObjectData
        //Instance method unsupported: GetCustomAttributes
        //Instance method unsupported: GetCustomAttributes
        //Instance method unsupported: IsDefined
        //Instance method unsupported: GetCustomAttributesData
        //Instance method unsupported: LoadModule
        //Instance method unsupported: LoadModule
        //Instance method unsupported: CreateInstance
        //Instance method unsupported: CreateInstance
        //Instance method unsupported: CreateInstance
        //Instance method unsupported: GetLoadedModules
        //Instance method unsupported: GetLoadedModules
        //Instance method unsupported: GetModules
        //Instance method unsupported: GetModules
        //Instance method unsupported: GetModule
        //Instance method unsupported: GetFile
        //Instance method unsupported: GetFiles
        //Instance method unsupported: GetFiles
        //Instance method unsupported: GetManifestResourceNames
        //Instance method unsupported: GetReferencedAssemblies
        //Instance method unsupported: GetManifestResourceInfo
        //Instance method unsupported: ToString
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.String CreateQualifiedName(global::System.String assemblyName,global::System.String typeName)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.Reflection.Assembly.CreateQualifiedName(System.String,System.String)System.String:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.Reflection.Assembly", "CreateQualifiedName", new string[]{"assemblyName", "typeName"}, new object[]{assemblyName,typeName});
                mhd.Revert();
                global::System.String retVal = global::System.Reflection.Assembly.CreateQualifiedName(assemblyName,typeName);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.Reflection.Assembly", "CreateQualifiedName", retVal);
                return retVal;
            }
        }

        //Instance method unsupported: GetType
    }
}

