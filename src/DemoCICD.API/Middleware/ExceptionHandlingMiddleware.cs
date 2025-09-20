using System.Text.Json;
using System;
using DemoCICD.Domain.Exceptions;
using Serilog;
using System.Linq;

namespace DemoCICD.API.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{

    public ExceptionHandlingMiddleware() { }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            title = GetTitle(exception),
            type = GetErrorCode(exception),
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception),
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            Application.Exceptions.ValidationException => StatusCodes.Status400BadRequest,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            FormatException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            Application.Exceptions.ValidationException => "Validation Error",
            FluentValidation.ValidationException => "Validation Error",
            DomainException applicationException => applicationException.Title,
            _ => "Server Error"
        };

    private static string GetErrorCode(Exception exception) =>
        exception switch
        {
            Application.Exceptions.ValidationException => "ValidationError",
            FluentValidation.ValidationException => "ValidationError",
            DomainException domainException => domainException.GetType().Name,
            _ => "InternalServerError"
        };

    private static object? GetErrors(Exception exception)
    {
        return exception switch
        {
            Application.Exceptions.ValidationException validationException =>
                validationException.Errors.Select(e => new { code = e.PropertyName, message = e.ErrorMessage }).ToArray(),
            FluentValidation.ValidationException fluentValidationException =>
                fluentValidationException.Errors.Select(e => new { code = e.PropertyName, message = e.ErrorMessage }).ToArray(),
            _ => null
        };
    }
}
