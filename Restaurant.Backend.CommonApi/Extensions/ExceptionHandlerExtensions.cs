using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Restaurant.Backend.CommonApi.Middleware;

namespace Restaurant.Backend.CommonApi.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionHandlerExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
