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
        public IActionResult Relics()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Heroes(int id)
        {
            return View(BuildHeroesViewModel(id));
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddCard(int heroId)
        {
            if (_heroesRepository.GetById(heroId) == null)
            {
                return NotFound();
            }

            return View("EditCard", BuildCardFormViewModel(heroId: heroId));
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddCard(HeroCardFormViewModel form)
        {
            if (!form.IsNew)
            {
                return BadRequest();
            }

            if (_heroesRepository.GetById(form.HeroId) == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("EditCard", BuildCardFormViewModel(form: form));
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

            return View(BuildCardFormViewModel(card: card));
        }

        [HttpPost]
        [Authorize]
        [IsSlayTheSpire2CreatorOrAdmin]
        public IActionResult EditCard(HeroCardFormViewModel form)
        {
            if (form.IsNew)
            {
                return BadRequest();
            }

            if (_heroesRepository.GetById(form.HeroId) == null)
            {
                return NotFound();
            }

            var card = _heroesCardsRepository.Get(form.CardId);
            if (card == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("EditCard", BuildCardFormViewModel(form: form));
            }

            ApplyFormToCard(card, form);

            var userId = _authService.GetUserId();
            card.ModifiedByUserId = userId;
            card.ModifiedAt = DateTime.UtcNow;

            _heroesCardsRepository.Update(card);

            return RedirectToAction(nameof(Heroes), new { id = form.HeroId });
        }

        private void ApplyFormToCard(SlayTheSpire2HeroesCards card, HeroCardFormViewModel form)
        {
            card.HeroId = form.HeroId;
            card.Name = form.Name.Trim();
            card.Description = form.Description.Trim();
            card.Rarity = form.Rarity.Trim();
            card.ManaCost = form.ManaCost;
            card.TypeOfCard = form.TypeOfCard.Trim();
            card.Upgraded = form.Upgraded;
            card.ImageUrl = string.IsNullOrWhiteSpace(form.ImageUrl) ? string.Empty : form.ImageUrl.Trim();
        }

        private HeroCardFormViewModel BuildCardFormViewModel(
            int heroId = 0,
            SlayTheSpire2HeroesCards? card = null,
            HeroCardFormViewModel? form = null)
        {
            if (form != null)
            {
                form.HeroOptions = BuildHeroSelectList(form.HeroId);
                form.RarityOptions = _cardOptionsService.BuildRaritySelectList(form.Rarity);
                form.TypeOfCardOptions = _cardOptionsService.BuildTypeOfCardSelectList(form.TypeOfCard);
                form.HeroName = _heroesRepository.GetById(form.HeroId)?.Name;
                return form;
            }

            if (card != null)
            {
                return new HeroCardFormViewModel
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
            }

            return new HeroCardFormViewModel
            {
                HeroId = heroId,
                HeroName = _heroesRepository.GetById(heroId)?.Name,
                HeroOptions = BuildHeroSelectList(heroId),
                RarityOptions = _cardOptionsService.BuildRaritySelectList(null),
                TypeOfCardOptions = _cardOptionsService.BuildTypeOfCardSelectList(null)
            };
        }

        private HeroesViewModel BuildHeroesViewModel(int heroId)
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

            return new HeroesViewModel
            {
                HeroId = heroId,
                Found = hero != null,
                Name = hero?.Name,
                Color = hero?.Color,
                Cards = cards
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
