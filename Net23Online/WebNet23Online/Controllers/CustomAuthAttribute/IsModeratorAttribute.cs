using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebNet23Online.Services;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.CustomAuthAttribute
{
    public class IsModeratorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();

            if (!authService.AtLeastModerator())
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
