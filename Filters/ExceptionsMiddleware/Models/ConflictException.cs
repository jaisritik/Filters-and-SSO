using System;

namespace Filters.ExceptionsMiddleware.Models;

public class ConflictException : Exception
{
    public string Error { get; set; }

    public ConflictException(string error) : base(error)
    {
        Error = error;
    }
}