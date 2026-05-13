using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebNet23Online.Data.Enums;
using WebNet23Online.Models.Steam;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.Steam;

namespace WebNet23Online.Controllers.CustomAuthAttribute.Steam
{
    /// <summary>
    /// Allows editing for: Admin (always) or (Creator + required role)
    /// Moderator role by default
    /// </summary>
    public class EditForCreatorWithRequiredRoleAttribute : ActionFilterAttribute
    {
        public UserRole RequiredRole { get; set; } = UserRole.Moderator;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int gameId;

            if (!int.TryParse(context.RouteData.Values["id"]?.ToString(), out gameId))
            {
                if (context.ActionArguments.TryGetValue("viewModel", out var model) &&
                    model is EditGameViewModel editModel)
                {
                    gameId = editModel.Id;
                }
                else
                {
                    context.Result = new BadRequestResult();
                    return;
                }
            }

            if (gameId == 0)
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

            var currentUserId = authService.GetUserId();
            if (currentUserId == 0)
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            var userRole = authService.GetRole();
            var isAdmin = userRole == UserRole.Admin;
            var hasRequiredRole = userRole == RequiredRole;
            var isOwner = catalogService.IsUserCreatorOfTheGame(currentUserId, gameId);

            var canEdit = isAdmin || (isOwner && hasRequiredRole);

            if (!canEdit)
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
