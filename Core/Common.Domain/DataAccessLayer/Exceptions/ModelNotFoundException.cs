namespace Common.Domain.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Exception should be raised when the model hasn't been found in the database.
    /// </summary>
    public class ModelNotFoundException: DataAccessLayerException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message</param>
        public ModelNotFoundException(string message) : base(message)
        {
        }
    }
}
