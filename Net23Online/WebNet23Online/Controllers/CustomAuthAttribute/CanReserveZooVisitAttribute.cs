using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.CustomAuthAttribute
{
    public class CanReserveZooVisitAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();
            var user = authService.GetUser();
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.Mobilephone))
            {
                context.Result = ((Controller)context.Controller).RedirectToAction("ZooReservationsDenied", "Tickets");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
