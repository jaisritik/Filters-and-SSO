namespace Filters.ExceptionsMiddleware.Models;


public class BadRequestException : Exception
{
    public string Error { get; set; }
    public Dictionary<string, string[]> Errors { get; } 
    public BadRequestException(string error, Dictionary<string, string[]> errors) : base(error)
    {
        Error = error;
        Errors = errors;    
    }
}