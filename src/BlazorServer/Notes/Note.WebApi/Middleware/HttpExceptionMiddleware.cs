using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Note.WebApi.Middleware
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public HttpExceptionMiddleware(RequestDelegate dlt)
            => _next = dlt;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
