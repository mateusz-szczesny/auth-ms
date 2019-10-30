namespace Auth.Responses
{
    public class ErrorPayload
    {
        public ErrorPayload(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }
}
