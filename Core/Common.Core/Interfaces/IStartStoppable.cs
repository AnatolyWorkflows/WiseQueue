namespace Common.Core.Interfaces
{
    /// <summary>
    /// Interface shows that object should be started before using and should be stopped after it.
    /// </summary>
    public interface IStartStoppable
    {
        /// <summary>
        /// Start manager.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop.
        /// </summary>
        void Stop(); 
    }
}
