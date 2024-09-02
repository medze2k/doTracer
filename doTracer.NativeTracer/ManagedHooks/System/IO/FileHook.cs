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

namespace doTracer.ManagedHooks.System.IO
{
    public class FileHook
    {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.StreamReader OpenText(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.OpenText(System.String)System.IO.StreamReader:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "OpenText", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.StreamReader retVal = global::System.IO.File.OpenText(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "OpenText", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.StreamWriter CreateText(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.CreateText(System.String)System.IO.StreamWriter:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "CreateText", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.StreamWriter retVal = global::System.IO.File.CreateText(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "CreateText", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.StreamWriter AppendText(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.AppendText(System.String)System.IO.StreamWriter:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "AppendText", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.StreamWriter retVal = global::System.IO.File.AppendText(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "AppendText", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Copy(global::System.String sourceFileName,global::System.String destFileName)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Copy(System.String,System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Copy", new string[]{"sourceFileName", "destFileName"}, new object[]{sourceFileName,destFileName});
                mhd.Revert();
                global::System.IO.File.Copy(sourceFileName,destFileName);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Copy", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Copy(global::System.String sourceFileName,global::System.String destFileName,global::System.Boolean overwrite)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Copy(System.String,System.String,System.Boolean)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Copy", new string[]{"sourceFileName", "destFileName", "overwrite"}, new object[]{sourceFileName,destFileName,overwrite});
                mhd.Revert();
                global::System.IO.File.Copy(sourceFileName,destFileName,overwrite);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Copy", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream Create(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Create(System.String)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Create", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.Create(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Create", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream Create(global::System.String path,global::System.Int32 bufferSize)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Create(System.String,System.Int32)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Create", new string[]{"path", "bufferSize"}, new object[]{path,bufferSize});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.Create(path,bufferSize);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Create", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream Create(global::System.String path,global::System.Int32 bufferSize,global::System.IO.FileOptions options)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Create(System.String,System.Int32,System.IO.FileOptions)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Create", new string[]{"path", "bufferSize", "options"}, new object[]{path,bufferSize,options});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.Create(path,bufferSize,options);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Create", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream Create(global::System.String path,global::System.Int32 bufferSize,global::System.IO.FileOptions options,global::System.Security.AccessControl.FileSecurity fileSecurity)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Create(System.String,System.Int32,System.IO.FileOptions,System.Security.AccessControl.FileSecurity)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Create", new string[]{"path", "bufferSize", "options", "fileSecurity"}, new object[]{path,bufferSize,options,fileSecurity});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.Create(path,bufferSize,options,fileSecurity);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Create", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Delete(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Delete(System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Delete", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.File.Delete(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Delete", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Boolean Exists(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Exists(System.String)System.Boolean:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Exists", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.Boolean retVal = global::System.IO.File.Exists(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Exists", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream Open(global::System.String path,global::System.IO.FileMode mode)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Open(System.String,System.IO.FileMode)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Open", new string[]{"path", "mode"}, new object[]{path,mode});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.Open(path,mode);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Open", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream Open(global::System.String path,global::System.IO.FileMode mode,global::System.IO.FileAccess access)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Open(System.String,System.IO.FileMode,System.IO.FileAccess)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Open", new string[]{"path", "mode", "access"}, new object[]{path,mode,access});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.Open(path,mode,access);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Open", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream Open(global::System.String path,global::System.IO.FileMode mode,global::System.IO.FileAccess access,global::System.IO.FileShare share)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Open(System.String,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Open", new string[]{"path", "mode", "access", "share"}, new object[]{path,mode,access,share});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.Open(path,mode,access,share);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Open", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetCreationTime(global::System.String path,global::System.DateTime creationTime)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetCreationTime(System.String,System.DateTime)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetCreationTime", new string[]{"path", "creationTime"}, new object[]{path,creationTime});
                mhd.Revert();
                global::System.IO.File.SetCreationTime(path,creationTime);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetCreationTime", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetCreationTimeUtc(global::System.String path,global::System.DateTime creationTimeUtc)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetCreationTimeUtc(System.String,System.DateTime)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetCreationTimeUtc", new string[]{"path", "creationTimeUtc"}, new object[]{path,creationTimeUtc});
                mhd.Revert();
                global::System.IO.File.SetCreationTimeUtc(path,creationTimeUtc);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetCreationTimeUtc", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.DateTime GetCreationTime(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetCreationTime(System.String)System.DateTime:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetCreationTime", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.DateTime retVal = global::System.IO.File.GetCreationTime(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetCreationTime", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.DateTime GetCreationTimeUtc(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetCreationTimeUtc(System.String)System.DateTime:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetCreationTimeUtc", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.DateTime retVal = global::System.IO.File.GetCreationTimeUtc(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetCreationTimeUtc", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetLastAccessTime(global::System.String path,global::System.DateTime lastAccessTime)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetLastAccessTime(System.String,System.DateTime)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetLastAccessTime", new string[]{"path", "lastAccessTime"}, new object[]{path,lastAccessTime});
                mhd.Revert();
                global::System.IO.File.SetLastAccessTime(path,lastAccessTime);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetLastAccessTime", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetLastAccessTimeUtc(global::System.String path,global::System.DateTime lastAccessTimeUtc)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetLastAccessTimeUtc(System.String,System.DateTime)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetLastAccessTimeUtc", new string[]{"path", "lastAccessTimeUtc"}, new object[]{path,lastAccessTimeUtc});
                mhd.Revert();
                global::System.IO.File.SetLastAccessTimeUtc(path,lastAccessTimeUtc);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetLastAccessTimeUtc", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.DateTime GetLastAccessTime(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetLastAccessTime(System.String)System.DateTime:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetLastAccessTime", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.DateTime retVal = global::System.IO.File.GetLastAccessTime(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetLastAccessTime", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.DateTime GetLastAccessTimeUtc(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetLastAccessTimeUtc(System.String)System.DateTime:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetLastAccessTimeUtc", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.DateTime retVal = global::System.IO.File.GetLastAccessTimeUtc(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetLastAccessTimeUtc", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetLastWriteTime(global::System.String path,global::System.DateTime lastWriteTime)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetLastWriteTime(System.String,System.DateTime)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetLastWriteTime", new string[]{"path", "lastWriteTime"}, new object[]{path,lastWriteTime});
                mhd.Revert();
                global::System.IO.File.SetLastWriteTime(path,lastWriteTime);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetLastWriteTime", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetLastWriteTimeUtc(global::System.String path,global::System.DateTime lastWriteTimeUtc)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetLastWriteTimeUtc(System.String,System.DateTime)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetLastWriteTimeUtc", new string[]{"path", "lastWriteTimeUtc"}, new object[]{path,lastWriteTimeUtc});
                mhd.Revert();
                global::System.IO.File.SetLastWriteTimeUtc(path,lastWriteTimeUtc);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetLastWriteTimeUtc", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.DateTime GetLastWriteTime(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetLastWriteTime(System.String)System.DateTime:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetLastWriteTime", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.DateTime retVal = global::System.IO.File.GetLastWriteTime(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetLastWriteTime", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.DateTime GetLastWriteTimeUtc(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetLastWriteTimeUtc(System.String)System.DateTime:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetLastWriteTimeUtc", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.DateTime retVal = global::System.IO.File.GetLastWriteTimeUtc(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetLastWriteTimeUtc", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileAttributes GetAttributes(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetAttributes(System.String)System.IO.FileAttributes:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetAttributes", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.FileAttributes retVal = global::System.IO.File.GetAttributes(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetAttributes", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetAttributes(global::System.String path,global::System.IO.FileAttributes fileAttributes)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetAttributes(System.String,System.IO.FileAttributes)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetAttributes", new string[]{"path", "fileAttributes"}, new object[]{path,fileAttributes});
                mhd.Revert();
                global::System.IO.File.SetAttributes(path,fileAttributes);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetAttributes", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Security.AccessControl.FileSecurity GetAccessControl(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetAccessControl(System.String)System.Security.AccessControl.FileSecurity:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetAccessControl", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.Security.AccessControl.FileSecurity retVal = global::System.IO.File.GetAccessControl(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetAccessControl", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Security.AccessControl.FileSecurity GetAccessControl(global::System.String path,global::System.Security.AccessControl.AccessControlSections includeSections)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.GetAccessControl(System.String,System.Security.AccessControl.AccessControlSections)System.Security.AccessControl.FileSecurity:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "GetAccessControl", new string[]{"path", "includeSections"}, new object[]{path,includeSections});
                mhd.Revert();
                global::System.Security.AccessControl.FileSecurity retVal = global::System.IO.File.GetAccessControl(path,includeSections);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "GetAccessControl", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetAccessControl(global::System.String path,global::System.Security.AccessControl.FileSecurity fileSecurity)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.SetAccessControl(System.String,System.Security.AccessControl.FileSecurity)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "SetAccessControl", new string[]{"path", "fileSecurity"}, new object[]{path,fileSecurity});
                mhd.Revert();
                global::System.IO.File.SetAccessControl(path,fileSecurity);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "SetAccessControl", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream OpenRead(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.OpenRead(System.String)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "OpenRead", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.OpenRead(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "OpenRead", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.IO.FileStream OpenWrite(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.OpenWrite(System.String)System.IO.FileStream:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "OpenWrite", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.FileStream retVal = global::System.IO.File.OpenWrite(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "OpenWrite", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.String ReadAllText(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.ReadAllText(System.String)System.String:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "ReadAllText", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.String retVal = global::System.IO.File.ReadAllText(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "ReadAllText", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.String ReadAllText(global::System.String path,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.ReadAllText(System.String,System.Text.Encoding)System.String:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "ReadAllText", new string[]{"path", "encoding"}, new object[]{path,encoding});
                mhd.Revert();
                global::System.String retVal = global::System.IO.File.ReadAllText(path,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "ReadAllText", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteAllText(global::System.String path,global::System.String contents)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.WriteAllText(System.String,System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "WriteAllText", new string[]{"path", "contents"}, new object[]{path,contents});
                mhd.Revert();
                global::System.IO.File.WriteAllText(path,contents);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "WriteAllText", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteAllText(global::System.String path,global::System.String contents,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.WriteAllText(System.String,System.String,System.Text.Encoding)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "WriteAllText", new string[]{"path", "contents", "encoding"}, new object[]{path,contents,encoding});
                mhd.Revert();
                global::System.IO.File.WriteAllText(path,contents,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "WriteAllText", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Byte[] ReadAllBytes(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.ReadAllBytes(System.String)System.Byte[]:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "ReadAllBytes", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.Byte[] retVal = global::System.IO.File.ReadAllBytes(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "ReadAllBytes", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteAllBytes(global::System.String path,global::System.Byte[] bytes)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.WriteAllBytes(System.String,System.Byte[])System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "WriteAllBytes", new string[]{"path", "bytes"}, new object[]{path,bytes});
                mhd.Revert();
                global::System.IO.File.WriteAllBytes(path,bytes);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "WriteAllBytes", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.String[] ReadAllLines(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.ReadAllLines(System.String)System.String[]:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "ReadAllLines", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.String[] retVal = global::System.IO.File.ReadAllLines(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "ReadAllLines", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.String[] ReadAllLines(global::System.String path,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.ReadAllLines(System.String,System.Text.Encoding)System.String[]:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "ReadAllLines", new string[]{"path", "encoding"}, new object[]{path,encoding});
                mhd.Revert();
                global::System.String[] retVal = global::System.IO.File.ReadAllLines(path,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "ReadAllLines", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Collections.Generic.IEnumerable<global::System.String> ReadLines(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.ReadLines(System.String)System.Collections.Generic.IEnumerable`1[System.String]:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "ReadLines", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.Collections.Generic.IEnumerable<global::System.String> retVal = global::System.IO.File.ReadLines(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "ReadLines", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static global::System.Collections.Generic.IEnumerable<global::System.String> ReadLines(global::System.String path,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.ReadLines(System.String,System.Text.Encoding)System.Collections.Generic.IEnumerable`1[System.String]:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "ReadLines", new string[]{"path", "encoding"}, new object[]{path,encoding});
                mhd.Revert();
                global::System.Collections.Generic.IEnumerable<global::System.String> retVal = global::System.IO.File.ReadLines(path,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "ReadLines", retVal);
                return retVal;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteAllLines(global::System.String path,global::System.String[] contents)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.WriteAllLines(System.String,System.String[])System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "WriteAllLines", new string[]{"path", "contents"}, new object[]{path,contents});
                mhd.Revert();
                global::System.IO.File.WriteAllLines(path,contents);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "WriteAllLines", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteAllLines(global::System.String path,global::System.String[] contents,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.WriteAllLines(System.String,System.String[],System.Text.Encoding)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "WriteAllLines", new string[]{"path", "contents", "encoding"}, new object[]{path,contents,encoding});
                mhd.Revert();
                global::System.IO.File.WriteAllLines(path,contents,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "WriteAllLines", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteAllLines(global::System.String path,global::System.Collections.Generic.IEnumerable<global::System.String> contents)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.WriteAllLines(System.String,System.Collections.Generic.IEnumerable`1[System.String])System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "WriteAllLines", new string[]{"path", "contents"}, new object[]{path,contents});
                mhd.Revert();
                global::System.IO.File.WriteAllLines(path,contents);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "WriteAllLines", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteAllLines(global::System.String path,global::System.Collections.Generic.IEnumerable<global::System.String> contents,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.WriteAllLines(System.String,System.Collections.Generic.IEnumerable`1[System.String],System.Text.Encoding)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "WriteAllLines", new string[]{"path", "contents", "encoding"}, new object[]{path,contents,encoding});
                mhd.Revert();
                global::System.IO.File.WriteAllLines(path,contents,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "WriteAllLines", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AppendAllText(global::System.String path,global::System.String contents)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.AppendAllText(System.String,System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "AppendAllText", new string[]{"path", "contents"}, new object[]{path,contents});
                mhd.Revert();
                global::System.IO.File.AppendAllText(path,contents);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "AppendAllText", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AppendAllText(global::System.String path,global::System.String contents,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.AppendAllText(System.String,System.String,System.Text.Encoding)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "AppendAllText", new string[]{"path", "contents", "encoding"}, new object[]{path,contents,encoding});
                mhd.Revert();
                global::System.IO.File.AppendAllText(path,contents,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "AppendAllText", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AppendAllLines(global::System.String path,global::System.Collections.Generic.IEnumerable<global::System.String> contents)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.AppendAllLines(System.String,System.Collections.Generic.IEnumerable`1[System.String])System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "AppendAllLines", new string[]{"path", "contents"}, new object[]{path,contents});
                mhd.Revert();
                global::System.IO.File.AppendAllLines(path,contents);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "AppendAllLines", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AppendAllLines(global::System.String path,global::System.Collections.Generic.IEnumerable<global::System.String> contents,global::System.Text.Encoding encoding)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.AppendAllLines(System.String,System.Collections.Generic.IEnumerable`1[System.String],System.Text.Encoding)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "AppendAllLines", new string[]{"path", "contents", "encoding"}, new object[]{path,contents,encoding});
                mhd.Revert();
                global::System.IO.File.AppendAllLines(path,contents,encoding);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "AppendAllLines", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Move(global::System.String sourceFileName,global::System.String destFileName)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Move(System.String,System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Move", new string[]{"sourceFileName", "destFileName"}, new object[]{sourceFileName,destFileName});
                mhd.Revert();
                global::System.IO.File.Move(sourceFileName,destFileName);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Move", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Replace(global::System.String sourceFileName,global::System.String destinationFileName,global::System.String destinationBackupFileName)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Replace(System.String,System.String,System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Replace", new string[]{"sourceFileName", "destinationFileName", "destinationBackupFileName"}, new object[]{sourceFileName,destinationFileName,destinationBackupFileName});
                mhd.Revert();
                global::System.IO.File.Replace(sourceFileName,destinationFileName,destinationBackupFileName);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Replace", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Replace(global::System.String sourceFileName,global::System.String destinationFileName,global::System.String destinationBackupFileName,global::System.Boolean ignoreMetadataErrors)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Replace(System.String,System.String,System.String,System.Boolean)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Replace", new string[]{"sourceFileName", "destinationFileName", "destinationBackupFileName", "ignoreMetadataErrors"}, new object[]{sourceFileName,destinationFileName,destinationBackupFileName,ignoreMetadataErrors});
                mhd.Revert();
                global::System.IO.File.Replace(sourceFileName,destinationFileName,destinationBackupFileName,ignoreMetadataErrors);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Replace", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Decrypt(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Decrypt(System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Decrypt", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.File.Decrypt(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Decrypt", "N/A");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Encrypt(global::System.String path)
        {
            ManagedHookDescriptor mhd = ManagedHookDescriptorManager.Get("System.IO.File.Encrypt(System.String)System.Void:static:public");
            lock (ManagedHookDescriptorManager.ManagedHookLock)
            {
                StackFrame sf = new StackFrame(1);
                Console.WriteLine("Called from {0}", sf.GetMethod().Module.Assembly.Location);
                GlobalLogger.LogPreCall("System.IO.File", "Encrypt", new string[]{"path"}, new object[]{path});
                mhd.Revert();
                global::System.IO.File.Encrypt(path);
                mhd.Apply();
                GlobalLogger.LogPostCall("System.IO.File", "Encrypt", "N/A");
            }
        }

        //Instance method unsupported: Equals
        //Instance method unsupported: GetHashCode
        //Instance method unsupported: GetType
        //Instance method unsupported: ToString
    }
}

