using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Models.Tasks;
using WiseQueue.Core.Server.Factories;

namespace WiseQueue.Core.Server.Models
{
    public class Worker: BaseLoggerObject, IWorker
    {
        private ActivationData activationData;

        /// <summary>
        /// Worker name.
        /// </summary>
        public string Name { get; private set; }

        public Worker(TaskModel taskModel, IExpressionConverter expressionConverter, ICommonLoggerFactory loggerFactory): base(loggerFactory)
        {
            if (taskModel == null) 
                throw new ArgumentNullException("taskModel");
            if (expressionConverter == null)
                throw new ArgumentNullException("expressionConverter");

            //TODO: activationData = expressionConverter.Convert(taskModel.ActivationData);
        }

        #region Implementation of IExecutable

        /// <summary>
        /// Occurs when object should do its action.
        /// </summary>
        public void Execute()
        {
            MethodInfo method = activationData.Method;
            object[] arguments = activationData.Arguments;
            Type instanceType = activationData.InstanceType;

            //TODO: Use IoC container.
            var obj = Activator.CreateInstance(instanceType);
            method.Invoke(obj, arguments);
        }

        #endregion
    }
}
