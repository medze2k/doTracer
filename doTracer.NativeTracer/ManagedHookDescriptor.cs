using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace doTracer
{
    /// <summary>
    /// This class represents a Managed Hook Descriptor that holds pointers and data bytes related to hooking.
    /// </summary>
    public class ManagedHookDescriptor
    {
        [DllImport("kernel32.dll")]
        static extern bool VirtualProtect(IntPtr address, int size, int newProtect, out int oldProtect);
        [DllImport("kernel32.dll")]
        static extern void DebugBreak();
        /// <summary>
        /// The real address of the method.
        /// </summary>
        public IntPtr RealNativePtr { get; private set; }
        /// <summary>
        /// The address of the hook code.
        /// </summary>
        public IntPtr HookNativePtr { get; private set; }
        /// <summary>
        /// The saved original bytes, before hook application.
        /// </summary>
        public byte[] RealBytes { get; private set; }
        /// <summary>
        /// The saved hook bytes.
        /// </summary>
        public byte[] HookBytes { get; private set; }
        /// <summary>
        /// Create a managed hook descriptor. Managed hook descriptors should be stored in ManagedHookDescriptorManager.
        /// </summary>
        /// <param name="realNativePtr">The real address of the method that will be hooked.</param>
        /// <param name="hookNativePtr">The address of the hook code.</param>
        public ManagedHookDescriptor(IntPtr realNativePtr, IntPtr hookNativePtr)
        {
            RealNativePtr = realNativePtr;
            HookNativePtr = hookNativePtr;
            RealBytes = new byte[5];
            HookBytes = new byte[5];
            BuildRealBytes();
            BuildHookBytes();
        }
        /// <summary>
        /// Build original bytes and cache them.
        /// </summary>
        private void BuildRealBytes()
        {
            //Backup real bytes
            Marshal.Copy(RealNativePtr, RealBytes, 0, 5);
        }
        /// <summary>
        /// Build hook bytes and cache them.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void BuildHookBytes()
        {
            //Build hook bytes
            if (IntPtr.Size == 4)
            {
                HookBytes[0] = 0xE9; // jmp
                int delta = checked((int)((long)HookNativePtr - (long)RealNativePtr - 5)); // offset
                byte[] offset = BitConverter.GetBytes(delta);
                Array.Copy(offset, 0, HookBytes, 1, 4);
            }
            else
            {
                throw new NotImplementedException("Please implement x64 hook.");
            }
            
        }
        /// <summary>
        /// Apply the hook bytes.
        /// </summary>
        public void Apply()
        {
            //TODO: Reset protection to RX only after modification
            VirtualProtect(RealNativePtr, 20, 0x40, out _);
            //Apply hook
            Marshal.Copy(HookBytes, 0, RealNativePtr, 5);
        }
        /// <summary>
        /// Revert to original bytes.
        /// </summary>
        public void Revert()
        {
            //Revert to original
            Marshal.Copy(RealBytes, 0, RealNativePtr, 5);
        }
    }
}
