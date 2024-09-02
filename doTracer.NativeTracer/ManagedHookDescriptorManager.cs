using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doTracer
{
    /// <summary>
    /// The managed hook descriptor manager class, used to manage hook descriptors.
    /// </summary>
    public class ManagedHookDescriptorManager
    {
        /// <summary>
        /// This object is used as a hooking lock in case the application is multi-threaded.
        /// </summary>
        public static readonly object ManagedHookLock = new object();
        //NOTE: Dictionary<string, ManagedHookDescriptor> crashes for unknown raison (maybe related to Jitting)
        private static Hashtable _managedHookDescriptors = new Hashtable();
        /// <summary>
        /// Obtains the ManagedHookDescriptor corresponding to the methodId (or mid).
        /// </summary>
        /// <param name="methodId">The method id (or mid) of the method.</param>
        /// <returns></returns>
        public static ManagedHookDescriptor Get(string methodId)
        {
            return (ManagedHookDescriptor) _managedHookDescriptors[methodId];
        }
        /// <summary>
        /// Link the ManagedHookDescriptor with a method id (or mid).
        /// </summary>
        /// <param name="methodId">The method id (or mid) of the method.</param>
        /// <param name="descriptor">The managed hook descriptor of the method.</param>
        public static void Set(string methodId, ManagedHookDescriptor descriptor)
        {
            //TODO: check if descriptor already exist for method!!
            _managedHookDescriptors.Add(methodId, descriptor);
        }
    }
}
