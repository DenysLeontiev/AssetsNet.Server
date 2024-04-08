public class GptRequestsLimitExceededException : Exception
{
    public GptRequestsLimitExceededException() : base("Requests limit exceeded. Please upgrade your plan.")
    {
    }

    public GptRequestsLimitExceededException(string message) : base(message)
    {
    }

    public GptRequestsLimitExceededException(string message, Exception innerException) : base(message, innerException)
    {
    }
}