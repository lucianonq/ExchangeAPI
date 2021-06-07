using ExchangeAPI.CustomExceptionMiddleware;
using Microsoft.AspNetCore.Builder;

namespace ExchangeAPI.Helpers
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
