using System.Reflection;

namespace Common.Core.ResourceHelper
{
    public interface IResourceReaderHelper
    {
        string ReadStringResource(Assembly assembly, string resourceName);        
    }
}
