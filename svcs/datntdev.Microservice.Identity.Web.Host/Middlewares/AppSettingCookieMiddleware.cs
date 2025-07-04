using datntdev.Microservice.Identity.Web.Host.Models;
using datntdev.Microservice.Identity.Web.Host.Services;
using System.Text;
using System.Text.Json;

namespace datntdev.Microservice.Identity.Web.Host.Middlewares
{
    public class AppSettingCookieMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var appSettingService = context.RequestServices.GetRequiredService<AppSettingService>();

            var appSetting = GetCookie(context);
            appSettingService.AppSetting = appSetting ?? AppSettingModel.Default;

            if (appSetting == null) SetCookie(context, appSettingService.AppSetting);

            return next(context);
        }

        private static void SetCookie(HttpContext context, AppSettingModel appSetting)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30) // Set expiration to 30 days
            };
            var jsonSettings = JsonSerializer.Serialize(appSetting);
            var base64Settings = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonSettings));
            context.Response.Cookies.Append(AppSettingService.CookieName, base64Settings, cookieOptions);
        }

        private static AppSettingModel? GetCookie(HttpContext context)
        {
            var cookieValue = context.Request.Cookies[AppSettingService.CookieName];
            if (string.IsNullOrEmpty(cookieValue)) return null;

            var jsonSettings = Encoding.UTF8.GetString(Convert.FromBase64String(cookieValue));
            return JsonSerializer.Deserialize<AppSettingModel>(jsonSettings);
        }
    }
}
