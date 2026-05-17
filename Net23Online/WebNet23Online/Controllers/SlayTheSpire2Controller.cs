using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.SlayTheSpire2;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class SlayTheSpire2Controller : Controller
    {
        private readonly ISlayTheSpire2RewardImageService _rewardImageService;
        private readonly ISlayTheSpire2CardOptionsService _cardOptionsService;
        private readonly IAuthService _authService;
        private readonly ISlayTheSpire2HeroesRepository _heroesRepository;
        private readonly ISlayTheSpire2HeroesCardsRepository _heroesCardsRepository;

        public SlayTheSpire2Controller(
            ISlayTheSpire2RewardImageService rewardImageService,
            ISlayTheSpire2CardOptionsService cardOptionsService,
            IAuthService authService,
            ISlayTheSpire2HeroesRepository heroesRepository,
            ISlayTheSpire2HeroesCardsRepository heroesCardsRepository)
        {
            _rewardImageService = rewardImageService;
            _cardOptionsService = cardOptionsService;
            _authService = authService;
            _heroesRepository = heroesRepository;
            _heroesCardsRepository = heroesCardsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Heroes(int id)
        {
            return View(BuildHeroesViewModel(id));
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddCard([Bind(Prefix = "AddCardForm")] AddHeroCardFormViewModel form)
        {
            var hero = _heroesRepository.GetById(form.HeroId);
            if (hero == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("Heroes", BuildHeroesViewModel(form.HeroId, form));
            }

            var userId = _authService.GetUserId();

            _heroesCardsRepository.Add(new SlayTheSpire2HeroesCards
            {
                HeroId = form.HeroId,
                Name = form.Name.Trim(),
                Description = form.Description.Trim(),
                Rarity = form.Rarity.Trim(),
                ManaCost = form.ManaCost,
                TypeOfCard = form.TypeOfCard.Trim(),
                Upgraded = form.Upgraded,
                ImageUrl = string.IsNullOrWhiteSpace(form.ImageUrl) ? string.Empty : form.ImageUrl.Trim(),
                CreatedByUserId = userId,
                ModifiedByUserId = userId,
                ModifiedAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Heroes), new { id = form.HeroId });
        }

        [HttpGet]
        [Authorize]
        [IsSlayTheSpire2CreatorOrAdmin]
        public IActionResult EditCard(int id)
        {
            var card = _heroesCardsRepository.Get(id);
            if (card == null)
            {
                return NotFound();
            }

            return View(BuildEditCardViewModel(card));
        }

        [HttpPost]
        [Authorize]
        [IsSlayTheSpire2CreatorOrAdmin]
        public IActionResult EditCard(EditHeroCardFormViewModel form)
        {
            if (_heroesRepository.GetById(form.HeroId) == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(BuildEditCardViewModel(form));
            }

            var card = _heroesCardsRepository.Get(form.CardId);
            if (card == null)
            {
                return NotFound();
            }

            card.HeroId = form.HeroId;
            card.Name = form.Name.Trim();
            card.Description = form.Description.Trim();
            card.Rarity = form.Rarity.Trim();
            card.ManaCost = form.ManaCost;
            card.TypeOfCard = form.TypeOfCard.Trim();
            card.Upgraded = form.Upgraded;
            card.ImageUrl = string.IsNullOrWhiteSpace(form.ImageUrl) ? string.Empty : form.ImageUrl.Trim();

            var userId = _authService.GetUserId();
            card.ModifiedByUserId = userId;
            card.ModifiedAt = DateTime.UtcNow;

            _heroesCardsRepository.Update(card);

            return RedirectToAction(nameof(Heroes), new { id = card.HeroId });
        }

        private EditHeroCardFormViewModel BuildEditCardViewModel(SlayTheSpire2HeroesCards card) =>
            new()
            {
                CardId = card.Id,
                HeroId = card.HeroId,
                HeroName = _heroesRepository.GetById(card.HeroId)?.Name,
                Name = card.Name,
                Description = card.Description,
                Rarity = card.Rarity,
                ManaCost = card.ManaCost,
                TypeOfCard = card.TypeOfCard,
                Upgraded = card.Upgraded,
                ImageUrl = card.ImageUrl,
                HeroOptions = BuildHeroSelectList(card.HeroId),
                RarityOptions = _cardOptionsService.BuildRaritySelectList(card.Rarity),
                TypeOfCardOptions = _cardOptionsService.BuildTypeOfCardSelectList(card.TypeOfCard)
            };

        private EditHeroCardFormViewModel BuildEditCardViewModel(EditHeroCardFormViewModel form)
        {
            form.HeroOptions = BuildHeroSelectList(form.HeroId);
            form.RarityOptions = _cardOptionsService.BuildRaritySelectList(form.Rarity);
            form.TypeOfCardOptions = _cardOptionsService.BuildTypeOfCardSelectList(form.TypeOfCard);
            form.HeroName = _heroesRepository.GetById(form.HeroId)?.Name;
            return form;
        }

        private HeroesViewModel BuildHeroesViewModel(int heroId, AddHeroCardFormViewModel? addCardForm = null)
        {
            var hero = _heroesRepository.GetById(heroId);
            var currentUserId = _authService.GetUserId();
            var isAuthenticated = _authService.IsAuthenticated();
            var isAdmin = isAuthenticated && _authService.GetRole() == UserRole.Admin;

            var cards = hero != null
                ? _heroesCardsRepository.GetByHeroId(heroId)
                    .Select(c => new HeroCardViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Rarity = c.Rarity,
                        ManaCost = c.ManaCost,
                        TypeOfCard = c.TypeOfCard,
                        Upgraded = c.Upgraded,
                        ImageUrl = c.ImageUrl,
                        CanEdit = isAdmin || (isAuthenticated && c.CreatedByUserId == currentUserId)
                    })
                    .ToList()
                : new List<HeroCardViewModel>();

            addCardForm ??= new AddHeroCardFormViewModel { HeroId = heroId };

            if (addCardForm.HeroId == 0)
            {
                addCardForm.HeroId = heroId;
            }

            return new HeroesViewModel
            {
                HeroId = heroId,
                Found = hero != null,
                Name = hero?.Name,
                Color = hero?.Color,
                Cards = cards,
                AddCardForm = addCardForm,
                HeroOptions = BuildHeroSelectList(addCardForm.HeroId),
                RarityOptions = _cardOptionsService.BuildRaritySelectList(addCardForm.Rarity),
                TypeOfCardOptions = _cardOptionsService.BuildTypeOfCardSelectList(addCardForm.TypeOfCard)
            };
        }

        private List<SelectListItem> BuildHeroSelectList(int selectedHeroId) =>
            _heroesRepository.GetAll()
                .OrderBy(h => h.Id)
                .Select(h => new SelectListItem
                {
                    Value = h.Id.ToString(),
                    Text = h.Name,
                    Selected = h.Id == selectedHeroId
                })
                .ToList();

        [HttpGet]
        public IActionResult KickStarter()
        {
            return View(new KickStarterViewModel());
        }

        [HttpPost]
        public IActionResult KickStarter(KickStarterViewModel model)
        {
            model ??= new KickStarterViewModel();
            model.ImageUrl = _rewardImageService.ResolveRewardImageUrl(model.DonationAmount);
            return View(model);
        }
    }
}
