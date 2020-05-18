using System;

namespace HI.SharedKernel.Result
{
    public interface IResult<out TSuccess, out TError>
    {
        TSuccess Success { get; }
        TError Failure { get; }
        bool Succeded { get; }
        bool Failed { get; }
    }
    public class Result<TSuccess, TError> : IResult<TSuccess, TError>
    {
        protected TSuccess successValue;
        protected TError failValue;
        public bool Succeded { get; protected set; }
        public bool Failed => !Succeded;
        public TSuccess Success => Succeded
            ? successValue
            : throw new MemberAccessException("Reason:" + Failure);
        public TError Failure => Failed
            ? failValue
            : throw new MemberAccessException("Can't access this property because result is success");

        public Result(TSuccess success)
        {
            Succeded = true;
            successValue = success;
        }

        public Result(TError err)
        {
            Succeded = false;
            failValue = err;
        }
    }
}