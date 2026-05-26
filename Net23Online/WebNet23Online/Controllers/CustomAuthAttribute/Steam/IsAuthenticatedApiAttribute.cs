using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.CustomAuthAttribute
{
    public class IsAuthenticatedApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();
            if (!authService.IsAuthenticated())
            {
                context.Result = new ObjectResult(new
                {
                    error = "Authentication required."
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
