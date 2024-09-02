using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace doTracer
{
    /// <summary>
    /// This class represents a method id (or mid) that is used in the doTracer project to uniquely identify a method.
    /// </summary>
    public class MethodId
    {
        private MethodInfo _methodInfo;
        /// <summary>
        /// Create a MethodId object.
        /// </summary>
        /// <param name="methodInfo">The methodInfo object of the method.</param>
        public MethodId(MethodInfo methodInfo)
        {
            this._methodInfo = methodInfo;
        }
        /// <summary>
        /// Build the method id (or mid) string.
        /// </summary>
        /// <returns>Method id (or mid)</returns>
        public override string ToString()
        {
            string className = _methodInfo.DeclaringType.FullName;
            string methodName = _methodInfo.Name;
            StringBuilder sbParameters = new StringBuilder();
            ParameterInfo[] pi = _methodInfo.GetParameters();
            for (int i = 0; i < pi.Length; i++)
            {
                if (pi[i].ParameterType.IsGenericType)
                {
                    sbParameters.Append(pi[i].ParameterType.ToString());
                }
                else
                {
                    sbParameters.Append(pi[i].ParameterType.FullName);
                }
                if (i < pi.Length - 1)
                {
                    sbParameters.Append(",");
                }
            }
            string returnTypeName;
            if (_methodInfo.ReturnType.IsGenericType)
            {
                returnTypeName = _methodInfo.ReturnType.ToString();
            }
            else
            {
                returnTypeName = _methodInfo.ReturnType.FullName;
            }
            string isStatic = _methodInfo.IsStatic ? "static" : "nonstatic";
            string protection = null;
            if (_methodInfo.IsPrivate)
            {
                protection = "private";
            }
            else if (_methodInfo.IsPublic)
            {
                protection = "public";
            }
            else
            {
                protection = "unknown";
            }
            return string.Format("{0}.{1}({2}){3}:{4}:{5}", className, methodName, sbParameters.ToString(), returnTypeName, isStatic, protection);
        }
    }
}
