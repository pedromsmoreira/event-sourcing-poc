namespace EventSourcing.WriteModel.ExceptionHandler.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IGlobalExceptionHandler globalExceptionHandler;

        public ExceptionHandlingMiddleware(RequestDelegate next, IGlobalExceptionHandler globalExceptionHandler)
        {
            this.next = next;
            this.globalExceptionHandler = globalExceptionHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception exception)
            {
                await this.globalExceptionHandler.HandleAsync(context, exception);
            }
        }
    }
}