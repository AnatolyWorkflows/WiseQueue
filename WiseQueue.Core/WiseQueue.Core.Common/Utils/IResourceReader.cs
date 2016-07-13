using System.Reflection;

namespace WiseQueue.Core.Common.Utils
{
    public interface IResourceReader
    {
        string ReadStringResource(Assembly assembly, string resourceName);        
    }
}
