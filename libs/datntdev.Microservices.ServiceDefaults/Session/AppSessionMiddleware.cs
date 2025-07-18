using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.ServiceDefaults.Session
{
    public class AppSessionMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.RequestServices.GetService<AppSessionContext>()?.SetUserInfo(context);

            return next(context);
        }
    }
}
