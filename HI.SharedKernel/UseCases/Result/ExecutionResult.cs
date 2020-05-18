using HI.SharedKernel.Errors;

namespace HI.SharedKernel.Result
{
    public class ExecutionResult<TSuccess> : Result<TSuccess, Error>
    {
        public ExecutionResult(TSuccess success) : base(success) { }
        public ExecutionResult(Error error) : base(error) { }
    }
}