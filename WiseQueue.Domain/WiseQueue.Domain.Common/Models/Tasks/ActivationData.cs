using System;
using System.Reflection;

namespace WiseQueue.Domain.Common.Models.Tasks
{
    public class ActivationData
    {
        public MethodInfo Method { get; private set; }
        public object[] Arguments { get; private set; }
        public Type[] ArgumentTypes { get; private set; }
        public Type InstanceType { get; private set; }


        public ActivationData(Type instanceType, MethodInfo method, object[] arguments, Type[] argumentTypes)
        {
            if (instanceType == null) 
                throw new ArgumentNullException("instanceType");
            if (method == null) 
                throw new ArgumentNullException("method");
            if (arguments == null) 
                throw new ArgumentNullException("arguments");
            if (argumentTypes == null) 
                throw new ArgumentNullException("argumentTypes");

            InstanceType = instanceType;
            Method = method;
            Arguments = arguments;            
            ArgumentTypes = argumentTypes;
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}({2})", InstanceType.Name, Method.Name, Arguments);
        }

        #endregion
    }
}
