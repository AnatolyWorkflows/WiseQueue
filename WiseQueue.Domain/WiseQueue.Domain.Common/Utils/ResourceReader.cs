using System;
using System.IO;
using System.Reflection;
using Common.Core.Logging;
using WiseQueue.Core.Common.Models;
using WiseQueue.Core.Common.Utils;

namespace WiseQueue.Domain.Common.Utils
{
    public class ResourceReader: BaseLoggerObject, IResourceReader
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="loggerFactory"/> is <see langword="null" />.</exception>
        public ResourceReader(ICommonLoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region Implementation of IResourceReader

        public string ReadStringResource(Assembly assembly, string resourceName)
        {
            string fullPath = string.Format("{0}.{1}", assembly.GetName().Name, resourceName);
            using (Stream stream = assembly.GetManifestResourceStream(fullPath))
            {
                if (stream == null)
                    throw new InvalidOperationException(String.Format("There is no {0} in the assembly {1}.",
                        resourceName, assembly));

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}
