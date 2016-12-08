using System;

namespace Common.Core.Errors
{
    public class ResponseResult<TItem>
    {
        public TItem Result { get; private set; }

        public string ErrorMessage { get; private set; }

        public Exception Ex { get; private set; }

        public bool HasError { get; private set; }

        public ResponseResult(TItem otem)
        {
            HasError = false;
            Result = otem;
        }

        public ResponseResult(string errorMessage)
        {
            HasError = true;
            ErrorMessage = errorMessage;
        }

        public ResponseResult(string errorMessage, Exception ex): this(errorMessage)
        {
            Ex = ex;
        }
    }
}
