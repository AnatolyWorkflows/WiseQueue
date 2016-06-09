using System;

namespace WiseQueue.Core.Common.Entities.Tasks
{
    /// <summary>
    /// Class contains all information about the task that has been serialized.
    /// </summary>
    public class TaskActivationDetailsEntity
    {
        #region Fields...
        /// <summary>
        /// Information about class that will be used for executing method.
        /// </summary>
        private readonly string instanceType;
        /// <summary>
        /// Information about method that will be executed.
        /// </summary>
        private readonly string method;
        /// <summary>
        /// Parameters' types that are using in the method.
        /// </summary>
        private readonly string parametersTypes;
        /// <summary>
        /// Arguments' values that are using in the method.
        /// </summary>
        private readonly string arguments;
        #endregion

        #region Properties...       
        /// <summary>
        /// Information about class that will be used for executing method.
        /// </summary>
        public string InstanceType
        {
            get { return instanceType; }
        }

        /// <summary>
        /// Information about method that will be executed.
        /// </summary>
        public string Method
        {
            get { return method; }
        }

        /// <summary>
        /// Parameters' types that are using in the method.
        /// </summary>
        public string ParametersTypes
        {
            get { return parametersTypes; }
        }

        /// <summary>
        /// Arguments' values that are using in the method.
        /// </summary>
        public string Arguments
        {
            get { return arguments; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instanceType">Information about class that will be used for executing method.</param>
        /// <param name="method">Information about method that will be executed.</param>
        /// <param name="parametersTypes">Parameters' types that are using in the method.</param>
        /// <param name="arguments">Argument' values that are using in the method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="instanceType"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="method"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="parametersTypes"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="arguments"/> is <see langword="null" />.</exception>
        public TaskActivationDetailsEntity(string instanceType, string method, string parametersTypes, string arguments)
        {
            if (string.IsNullOrWhiteSpace(instanceType)) 
                throw new ArgumentNullException("instanceType");
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException("method");
            if (string.IsNullOrWhiteSpace(parametersTypes))
                throw new ArgumentNullException("parametersTypes");
            if (string.IsNullOrWhiteSpace(arguments))
                throw new ArgumentNullException("arguments");
            
            this.instanceType = instanceType;
            this.method = method;
            this.parametersTypes = parametersTypes;
            this.arguments = arguments;
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}({2})", instanceType, method, arguments);
        }

        #endregion

    }
}
