using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace doTracer
{
    /// <summary>
    /// This class represents the hook manager that allows us to hook methods.
    /// </summary>
    public class ManagedHookManager
    {
        [DllImport("kernel32.dll")]
        static extern void DebugBreak();
        /// <summary>
        /// Creates a hook manager object that allows to perform hooking.
        /// </summary>
        public ManagedHookManager()
        {

        }
        /// <summary>
        /// Hook all possible .NET methods.
        /// </summary>
        public void HookAll()
        {
            HookType(typeof(File));
            HookType(typeof(Assembly));
        }
        /// <summary>
        /// Hook all .NET methods in the specified type.
        /// </summary>
        /// <param name="type">A type where all possible .NET methods will be hooked.</param>
        public void HookType(Type type)
        {
            MethodInfo[] methodInfos = type.GetMethods();
            for (int i = 0; i < methodInfos.Length; i++)
            {
                HookMethod(methodInfos[i]);
            }
        }
        /// <summary>
        /// Hook a method.
        /// </summary>
        /// <param name="methodInfo">The MethodInfo object of the method that will be hooked.</param>
        public void HookMethod(MethodInfo methodInfo)
        {
            if (!IsHookingSupported(methodInfo))
            {
                return;
            }
            if (methodInfo.IsStatic)
            {
                //Hook static method
                HookStaticMethod(methodInfo);
            }
            else
            {
                //Hook instance method
                HookInstanceMethod(methodInfo);
            }
        }
        /// <summary>
        /// Hook a static method.
        /// </summary>
        /// <param name="methodInfo">The MethodInfo object of the static method that will be hooked.</param>
        private void HookStaticMethod(MethodInfo methodInfo)
        {
            if (!methodInfo.IsStatic)
            {
                throw new ArgumentException("The specified methodInfo is not a static method.");
            }            
            string realTypeName = methodInfo.DeclaringType.FullName;
            string hookTypeName = string.Format("doTracer.ManagedHooks.{0}Hook", realTypeName);
            Type hookType = Type.GetType(hookTypeName);
            Type[] signatureTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
            MethodInfo hookMethodInfo = hookType.GetMethod(methodInfo.Name, signatureTypes);
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);
            RuntimeHelpers.PrepareMethod(hookMethodInfo.MethodHandle);
            IntPtr realNativePtr = methodInfo.MethodHandle.GetFunctionPointer();
            IntPtr hookNativePtr = hookMethodInfo.MethodHandle.GetFunctionPointer();
            ManagedHookDescriptor mhd = new ManagedHookDescriptor(realNativePtr, hookNativePtr);
            MethodId mid = new MethodId(methodInfo);
            ManagedHookDescriptorManager.Set(mid.ToString(), mhd);
            mhd.Apply();
        }
        /// <summary>
        /// Hook an instance method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <exception cref="ArgumentException"></exception>
        private void HookInstanceMethod(MethodInfo methodInfo)
        {
            if (methodInfo.IsStatic)
            {
                throw new ArgumentException("The specified methodInfo is not an instance method.");
            }
            //throw new NotImplementedException("Instance method hooking is not implemented.");
        }
        private bool IsHookingSupported(MethodInfo methodInfo)
        {
            if (methodInfo.IsGenericMethod)
            {
                //Generic methods are currently not supported
                return false;
            }
            if (!methodInfo.IsStatic)
            {
                //Instance methods are currently not supported
                return false;
            }
            if (methodInfo.IsSpecialName)
            {
                //Special names (eg. op_Equality) are currently not supported
                return false;
            }
            return true;
        }
    }
}
