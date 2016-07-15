namespace Common.Core.Interfaces
{
    /// <summary>
    /// Interface shows that object has a method that can be used for executing its logic.
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        /// Occurs when object should do its action.
        /// </summary>
        void Execute();
    }
}
