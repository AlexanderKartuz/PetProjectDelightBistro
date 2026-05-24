using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebNet23Online.Data.Enums;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.CustomAuthAttribute
{
    public class IsNotBlockedInTrackerAttribute  : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();

            if (authService.IsBlockedInTracker() || !authService.IsAuthenticated())
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
