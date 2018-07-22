namespace EventSourcing.WriteModel.ExceptionHandler
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IHandler
    {
        Task HandleAsync(HttpContext context, Exception exception);
    }
}