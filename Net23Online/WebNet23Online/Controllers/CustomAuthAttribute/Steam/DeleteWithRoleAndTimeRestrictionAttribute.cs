using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebNet23Online.Data.Enums;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.Steam;

namespace WebNet23Online.Controllers.CustomAuthAttribute.Steam
{
    /// <summary>
    /// Allows game deletion only for:
    /// <list type="bullet">
    ///   <item><description>Admin users (always)</description></item>
    ///   <item><description>Game owners who have the required role within the allowed time limit (default: 3 days, Moderator role)</description></item>
    /// </list>
    /// </summary>
    public class DeleteWithRoleAndTimeRestrictionAttribute : ActionFilterAttribute
    {
        public int AllowedDaysForCreator { get; set; } = 3;
        public UserRole RequiredRoleForCreator { get; set; } = UserRole.Moderator;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!int.TryParse(context.RouteData.Values["id"]?.ToString(), out int gameId))
            {
                context.Result = new BadRequestResult();
                return;
            }

            var catalogService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<ICatalogService>();

            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();

            var game = catalogService.GetGameDetails(gameId);
            if (game == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            var currentUserId = authService.GetUserId();
            if (currentUserId == 0)
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            var userRole = authService.GetRole();
            var isAdmin = userRole == UserRole.Admin;
            var hasRequiredRole = userRole == RequiredRoleForCreator;
            var isOwner = game.CreatedByUserId == currentUserId;

            var daysSinceCreation = (DateTime.UtcNow - game.CreatedAt).TotalDays;
            var isWithinTimeLimit = daysSinceCreation <= AllowedDaysForCreator;

            var canDelete = isAdmin || (isWithinTimeLimit && isOwner && hasRequiredRole);

            if (!canDelete)
            {
                context.Result = context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
