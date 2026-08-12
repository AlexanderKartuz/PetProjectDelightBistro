using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Controllers.CustomAuthAttribute
{
    public class IsAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();

            if (authService.GetRole() != UserRole.Admin)
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
