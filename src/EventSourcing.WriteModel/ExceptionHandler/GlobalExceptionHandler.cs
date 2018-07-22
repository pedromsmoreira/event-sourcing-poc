namespace EventSourcing.WriteModel.ExceptionHandler
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Infrastructure.Exceptions;

    using Microsoft.AspNetCore.Http;

    public class GlobalExceptionHandler : IGlobalExceptionHandler
    {
        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            ApplicationError error;
            HttpStatusCode statusCode;

            switch (exception)
            {
                case ArgumentException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    error = new ApplicationError(context, exception);
                    break;

                case NotFoundException ex:
                    statusCode = HttpStatusCode.NotFound;
                    error = new ApplicationError(context, exception);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    error = new ApplicationError(context, exception);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(error.CreateApplicationErrorResponse(statusCode));
        }
    }
}