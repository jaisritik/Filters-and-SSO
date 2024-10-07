using Filters.ExceptionsMiddleware.Models;
using System.Text.Json;

namespace Filters.ExceptionsMiddleware;


public class GlobalExceptionHandler
{
    private readonly RequestDelegate next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        var result = new ResultViewModel();

        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            result.BadRequest(ex.Error, ex.Errors);
            await SetContext(context, result, StatusCodes.Status400BadRequest);
        }
        catch (NotFoundException ex)
        {
            result.NotFound(ex.Error);
            await SetContext(context, result, StatusCodes.Status404NotFound);
        }
        catch (ConflictException ex)
        {   
            result.Conflict(ex.Error);
            await SetContext(context, result, StatusCodes.Status409Conflict);
        }
        catch(UnauthorizeException ex)  
        {
            result.Unauthorized(ex.Error, ex.Errors);
            await SetContext(context, result, StatusCodes.Status401Unauthorized);
        }
        catch (Exception ex)
        {
            result.InternalServerError(ex.Message);
            await SetContext(context, result, StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task SetContext(HttpContext context, ResultViewModel result, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}

