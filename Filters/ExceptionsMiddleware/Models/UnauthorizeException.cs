namespace Filters.ExceptionsMiddleware.Models
{
    public class UnauthorizeException : Exception
    {
        public string Error { get; set; }
        public Dictionary<string, string[]> Errors { get; }

        public UnauthorizeException(string error, Dictionary<string, string[]> errors) : base(error)
        {
            Error = error;
            Errors = errors;
        }
    }
}
