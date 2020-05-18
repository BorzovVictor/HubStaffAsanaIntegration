using Newtonsoft.Json;

namespace HI.SharedKernel.Errors
{
    public abstract class Error
    {
        public abstract int Id { get; }
        public abstract string Message { get; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}