using HI.SharedKernel.Errors;
using HI.SharedKernel.Result;

namespace HI.SharedKernel
{
    public abstract class UseCase<TSuccess>
    {
        public ExecutionResult Success(TSuccess success) => new ExecutionResult(success);
        public ExecutionResult Failure(Error error) => new ExecutionResult(error);

        public class ExecutionResult : Result<TSuccess, Error>
        {
            public ExecutionResult(TSuccess success) : base(success) { }
            public ExecutionResult(Error error) : base(error) { }
        }
    }
}