namespace Common.Core.Interfaces.DataAccessLayer
{
    /// <summary>
    /// Interface shows that object is a data context factory.
    /// </summary>
    public interface IDataContextFactory
    {
        /// <summary>
        /// Create a new data context.
        /// </summary>
        /// <returns>The IDataContext instance.</returns>
        IDataContext CreateDataContext();
    }
}
