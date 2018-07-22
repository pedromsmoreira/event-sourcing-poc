namespace EventSourcing.WriteModel.ExceptionHandler.Middleware
{
    using System;
    using Microsoft.AspNetCore.Builder;

    public static class ExceptionHandlingBuilderExceptions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}