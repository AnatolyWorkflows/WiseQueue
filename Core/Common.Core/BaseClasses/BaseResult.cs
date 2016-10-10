namespace Common.Core.BaseClasses
{
    /// <summary>
    /// Base class for all response from methods.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseResult<TResult>
    {
        #region Properties...
        /// <summary>
        /// Result.
        /// </summary>
        public TResult Result { get; private set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMsg { get; private set; }

        /// <summary>
        /// Flag shows whether or not there was an error during method execution.
        /// </summary>
        public bool HasError { get; private set; }
        #endregion

        #region Constructors...
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="result">The result.</param>
        protected BaseResult(TResult result)
        {
            HasError = false;
            Result = result;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorMsg">Error message.</param>
        protected BaseResult(string errorMsg)
        {
            HasError = true;
            ErrorMsg = errorMsg;
            Result = default(TResult);
        }
        #endregion

        public override string ToString()
        {
            if (HasError)
                return string.Format("ERROR: {0}", ErrorMsg);
            return string.Format("Result = {0}", Result);
        }
    }
}
