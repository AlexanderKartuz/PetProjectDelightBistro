using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DelightBistroMvc.Services;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Controllers.CustomAuthAttribute
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
