using Common.Core.BaseClasses;

namespace WiseQueue.Core.Common
{
    /// <summary>
    /// This is a response class for all method in this project.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class MethodResult<TResult> : BaseResult<TResult>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="result">The result.</param>
        public MethodResult(TResult result) : base(result)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorMsg">Error message.</param>
        public MethodResult(string errorMsg) : base(errorMsg)
        {
        }
    }
}
