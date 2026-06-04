using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Localizations;
using WebNet23Online.Models.LittleLemon;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.LittleLemon;

namespace WebNet23Online.Controllers
{
    public class LittleLemonController : Controller
    {
        private ILittleLemonMenuService _littleLemonMenuService;
        private ILittleLemonTestimonialService _littleLemonTestimonialService;
        private ILittleLemonSubscribeService _littleLemonSubscribeService;
        private ILittleLemonReservationService _littleLemonReservationService;
        private IWebHostEnvironment _webHostEnvironment;

        public LittleLemonController(ILittleLemonMenuService littleLemonMenuService,
                                     ILittleLemonTestimonialService littleLemonTestimonialService,
                                     ILittleLemonSubscribeService littleLemonSubscribeService,
                                     ILittleLemonReservationService littleLemonReservationService,
                                     IWebHostEnvironment webHostEnvironment)
        {
            _littleLemonMenuService = littleLemonMenuService;
            _littleLemonTestimonialService = littleLemonTestimonialService;
            _littleLemonSubscribeService = littleLemonSubscribeService;
            _littleLemonReservationService = littleLemonReservationService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string category)
        {
            var menuItems = _littleLemonMenuService.GetMenuItems(category);

            var testimonials = _littleLemonTestimonialService.GetTestimonials();
            var hero = new LittleLemonHeroSectionViewModel
            {
                CallToActionHref = Url.Action("Reservation", "LittleLemon") ?? "/LittleLemon/Reservation",
                CallToActionText = "Reserve a Table",
                HeroImageUrl = "/images/little-lemon/images/restauranfood.jpg",
                HeroImageAlt = "Signature Mediterranean platter at Little Lemon"
            };

            var pageModel = new LittleLemonIndexPageViewModel
            {
                Hero = hero,
                MenuItems = menuItems,
                Testimonials = testimonials
            };
            return View(pageModel);
        }


        [HttpGet]
        public IActionResult Subscribe()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Subscribe(LittleLemonSubscribeViewModel model)
        {
            var message = _littleLemonSubscribeService.GetSubscribeMessage(model.Email);
            TempData[LittleLemonSubscribeViewModel.MESSAGE_KEY] = message;
            if (Url.IsLocalUrl(model.ReturnUrl))
            {
                return LocalRedirect(model.ReturnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
        [CanAccessLittleLemonReservation]
        public IActionResult Reservation()
        {
            var hero = new LittleLemonHeroSectionViewModel
            {
                CallToActionHref = (Url.Action("Index", "LittleLemon") + "#menu") ?? "/LittleLemon/Index#menu",
                CallToActionText = "Order For Delivery",
                HeroImageUrl = "/images/little-lemon/images/restauranfood.jpg",
                HeroImageAlt = "Signature Mediterranean platter at Little Lemon"
            };
            var reservation = new LittleLemonReservationViewModel
            {
                GuestName = string.Empty
            };
            var pageModel = new LittleLemonReservationPageViewModel
            {
                Hero = hero,
                Reservation = reservation
            };

            return View(pageModel);
        }
        [HttpPost]
        [CanAccessLittleLemonReservation]
        public IActionResult Reservation(LittleLemonReservationPageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var hero = new LittleLemonHeroSectionViewModel
                {
                    CallToActionHref = (Url.Action("Index", "LittleLemon") + "#menu") ?? "/LittleLemon/Index#menu",
                    CallToActionText = "Order For Delivery",
                    HeroImageUrl = "/images/little-lemon/images/restauranfood.jpg",
                    HeroImageAlt = "Signature Mediterranean platter at Little Lemon"
                };
                var pageModel = new LittleLemonReservationPageViewModel
                {
                    Hero = hero,
                    Reservation = viewModel.Reservation ?? new LittleLemonReservationViewModel()
                };

                return View(pageModel);
            }

            var reservation = viewModel.Reservation!;
            if (_littleLemonReservationService.HasReservationAtDateTime(
                    reservation.ReservationDateOnly!,
                    reservation.AvailableTimesOnly!,
                    reservation.SeatingPreference!))
            {
                ModelState.AddModelError(string.Empty, LittleLemon.Reservation_DuplicateWarning);
                var heroOnDuplicate = new LittleLemonHeroSectionViewModel
                {
                    CallToActionHref = (Url.Action("Index", "LittleLemon") + "#menu") ?? "/LittleLemon/Index#menu",
                    CallToActionText = "Order For Delivery",
                    HeroImageUrl = "/images/little-lemon/images/restauranfood.jpg",
                    HeroImageAlt = "Signature Mediterranean platter at Little Lemon"
                };
                var pageModelOnDuplicate = new LittleLemonReservationPageViewModel
                {
                    Hero = heroOnDuplicate,
                    Reservation = reservation
                };

                return View(pageModelOnDuplicate);
            }

            var reservationId = _littleLemonReservationService.CreateReservation(reservation);
            if (viewModel.DessertReferencePhoto != null && viewModel.DessertReferencePhoto.Length > 0)
            {
                var pathToFolder = Path.Combine("images", "little-lemon", "reservation-desserts");
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, pathToFolder);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                var fileName = $"cake-{reservationId}.jpg";
                var path = Path.Combine(fullPath, fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    viewModel.DessertReferencePhoto.CopyTo(fileStream);
                }
                var cakePhotoUrl = $"/{pathToFolder.Replace("\\", "/")}/{fileName}";
                _littleLemonReservationService.SetReservationCakePhotoUrl(reservationId, cakePhotoUrl);
            }


            return RedirectToAction(nameof(Confirmation), new { reservationId });
        }

        [HttpPost]
        public IActionResult CreateGuest(string guestName)
        {
            var guestId = _littleLemonReservationService.CreateGuest(guestName);

            return RedirectToAction(nameof(Reservation));

        }

        [HttpPost]
        [CanAccessLittleLemonReservation]
        public IActionResult LinkReservationToGuest(int reservationId, int guestId)
        {
            var isLinked = _littleLemonReservationService.LinkReservationToGuest(reservationId, guestId);
            if (!isLinked)
            {
                return RedirectToAction(nameof(Reservation));
            }

            return RedirectToAction(nameof(Confirmation), new { reservationId });
        }
        [CanAccessLittleLemonReservation]
        public IActionResult Confirmation(int reservationId)
        {
            var reservation = _littleLemonReservationService.GetReservationViewModelById(reservationId);
            if (reservation == null)
            {
                return RedirectToAction(nameof(Reservation));
            }

            var hero = new LittleLemonHeroSectionViewModel
            {
                CallToActionHref = (Url.Action("Index", "LittleLemon") + "#menu") ?? "/LittleLemon/Index#menu",
                CallToActionText = "Order For Delivery",
                HeroImageUrl = "/images/little-lemon/images/restauranfood.jpg",
                HeroImageAlt = "Signature Mediterranean platter at Little Lemon"
            };
            var pageModel = new LittleLemonConfirmationViewModel
            {
                Hero = hero,
                Reservation = reservation,
                CanSeeHistory = true
            };
            return View(pageModel);
        }
        [CanAccessLittleLemonReservation]
        public IActionResult History()
        {
            var hero = new LittleLemonHeroSectionViewModel
            {
                CallToActionHref = (Url.Action("Index", "LittleLemon") + "#menu") ?? "/LittleLemon/Index#menu",
                CallToActionText = "Order For Delivery",
                HeroImageUrl = "/images/little-lemon/images/restauranfood.jpg",
                HeroImageAlt = "Signature Mediterranean platter at Little Lemon"
            };
            var reservations = _littleLemonReservationService.GetReservationHistoryForCurrentUser();
            var pageModel = new LittleLemonHistoryPageViewModel
            {
                Hero = hero,
                Reservations = reservations,
            };
            return View(pageModel);
        }


        public IActionResult HistoryPrint()
        {
            var path = Path.GetTempFileName();
            var reservations = _littleLemonReservationService.GetReservationHistoryForCurrentUser();
            using (var file = System.IO.File.CreateText(path))
            {
                file.WriteLine("Id,Date,Time,Guests,Seating,Name,Occasion,Notes,CakePhotoUrl");
                foreach (var item in reservations)
                {
                    var reservation = item.Reservation!;
                    file.WriteLine(
                        $"{item.ReservationId},{reservation.ReservationDateOnly},{reservation.AvailableTimesOnly},{reservation.NumberOfGuests},{reservation.SeatingPreference},{reservation.GuestName},{reservation.Occasion},{reservation.UserComments},{reservation.CakePhotoUrl}");
                }
            }

            var fileStream = new FileStream(path, FileMode.Open);
            return File(fileStream, "text/csv");
        }

    }
}
