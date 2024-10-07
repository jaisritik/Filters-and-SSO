using System.Runtime;
using Microsoft.OpenApi;

namespace Filters.ExceptionsMiddleware.Models;
public class NotFoundException : Exception
{
    public string Error { get; set; }
    public NotFoundException(string error) : base(error)
    {
        Error = error;
    }
}