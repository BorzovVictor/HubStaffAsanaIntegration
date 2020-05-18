namespace HI.SharedKernel.Errors
{
    public class UpdateSumFieldsError: Error
    {
        public override int Id { get; }
        public override string Message { get; }

        public UpdateSumFieldsError(string message, int id)
        {
            Message = message;
            Id = id;
        }
    }
}