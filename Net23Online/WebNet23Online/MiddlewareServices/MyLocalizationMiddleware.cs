using System.Globalization;
using System.Text.RegularExpressions;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.MiddlewareServices;

public class MyLocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public MyLocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authService = context.RequestServices.GetRequiredService<IAuthService>();

        var culture = new CultureInfo("en-US");
        if (authService.IsAuthenticated())
        {
            culture = authService.GetLanguage() switch
            {
                Data.Enums.Language.Russian => new CultureInfo("ru-RU"),
                Data.Enums.Language.English => new CultureInfo("en-US"),
                Data.Enums.Language.Deutsch => new CultureInfo("de-DE"),
                _ => throw new NotImplementedException()
            };
        }
        else
        {
            // Guest

            // just and example of header value
            // ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,la;q=0.6
            context.Request.Headers.AcceptLanguage.First();
        }

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        // before controller

        await _next.Invoke(context);

        // after controller
    }
}
