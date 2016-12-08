namespace Common.Core.Interfaces.DataAccessLayer
{
    /// <summary>
    /// Pattern Unit of Work.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commit transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        void Rollback();
    }
}
