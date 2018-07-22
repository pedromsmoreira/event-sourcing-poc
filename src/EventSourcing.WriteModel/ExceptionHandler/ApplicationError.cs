namespace EventSourcing.WriteModel.ExceptionHandler
{
    using System;
    using System.Net;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    public class ApplicationError
    {
        public ApplicationError()
        {
        }

        public ApplicationError(HttpContext context, Exception exception)
        {
            this.Context = context;
            this.Exception = exception;
        }

        public Exception Exception { get; set; }

        public string Message { get; set; }

        public HttpContext Context { get; set; }

        public string CreateApplicationErrorResponse(HttpStatusCode code)
        {
            return JsonConvert.SerializeObject(new
            {
                code,
                error = this.Exception.Message,
                stacktrace = this.Exception
            });
        }
    }
}