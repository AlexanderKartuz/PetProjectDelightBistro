using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using WebNet23Online.Data.Enums;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.CustomAuthAttribute
{
    public class CanAccessLittleLemonReservationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
            if (!authorizationService.IsAuthenticated())
            {
                context.Result = ((Controller)context.Controller).RedirectToAction("login", "Auth");
                return;
            }
            var role = authorizationService.GetRole();
            var canAccess = role == UserRole.Admin || role == UserRole.User;

            if (!canAccess)
            {
                context.Result = ((Controller)context.Controller).RedirectToAction("Deny", "Auth");
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
