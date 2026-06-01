using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.SlayTheSpire2;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.CustomAuthAttribute
{
    public class IsSlayTheSpire2CreatorOrAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var denyResult = GetDenyResultIfUserCannotEditCard(context);
            if (denyResult != null)
            {
                context.Result = denyResult;
                return;
            }

            base.OnActionExecuting(context);
        }

        private IActionResult? GetDenyResultIfUserCannotEditCard(ActionExecutingContext context)
        {
            if (!TryGetCardIdFromRequest(context, out var cardId))
            {
                return new BadRequestResult();
            }

            var cardsRepository = context
                .HttpContext
                .RequestServices
                .GetRequiredService<ISlayTheSpire2HeroesCardsRepository>();

            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();

            var card = cardsRepository.Get(cardId);
            if (card == null)
            {
                return new NotFoundResult();
            }

            var currentUserId = authService.GetUserId();
            if (currentUserId == 0)
            {
                return ((Controller)context.Controller).RedirectToAction("Deny", "Auth");
            }

            var isAdmin = authService.GetRole() == UserRole.Admin;
            var isCreator = card.CreatedByUserId == currentUserId;

            if (!isAdmin && !isCreator)
            {
                return ((Controller)context.Controller).RedirectToAction("Deny", "Auth");
            }

            return null;
        }

        private bool TryGetCardIdFromRequest(ActionExecutingContext context, out int cardId)
        {
            if (int.TryParse(context.RouteData.Values["id"]?.ToString(), out var routeCardId) && routeCardId > 0)
            {
                cardId = routeCardId;
                return true;
            }

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is HeroCardFormViewModel form && form.CardId > 0)
                {
                    cardId = form.CardId;
                    return true;
                }
            }

            cardId = 0;
            return false;
        }
    }
}
