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
            if (!TryResolveCardId(context, out var cardId))
            {
                context.Result = new BadRequestResult();
                return;
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

            var isAdmin = authService.GetRole() == UserRole.Admin;
            var isCreator = card.CreatedByUserId == currentUserId;

            if (!isAdmin && !isCreator)
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }

        private static bool TryResolveCardId(ActionExecutingContext context, out int cardId)
        {
            if (int.TryParse(context.RouteData.Values["id"]?.ToString(), out cardId) && cardId > 0)
            {
                return true;
            }

            if (context.ActionArguments.TryGetValue("form", out var model) &&
                model is EditHeroCardFormViewModel editModel &&
                editModel.CardId > 0)
            {
                cardId = editModel.CardId;
                return true;
            }

            cardId = 0;
            return false;
        }
    }
}
