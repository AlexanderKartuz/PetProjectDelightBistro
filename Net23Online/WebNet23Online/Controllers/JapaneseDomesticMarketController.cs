using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NAudio;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.JapaneseDomesticMarket;
using WebNet23Online.Services;
using WebNet23Online.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WebNet23Online.Controllers
{
    public class JapaneseDomesticMarketController : Controller
    {
        private IJapaneseDomesticMarketGenerator _jdmItemGenerator;
        private IJDMCatalogGenerator _jdmCatalogGenerator;
        private IJdmRepository _jdmRepository;
        private IJdmManufacturerRepository _jdmManufacturerRepository;
        private readonly IAuthService _authService;
        public IWebHostEnvironment _webHostEnvironment;
        private readonly IJdmJournalCommentRepository _journalCommentRepository;

        public JapaneseDomesticMarketController(IJapaneseDomesticMarketGenerator jdmItemGenerator, IJDMCatalogGenerator jdmCatalogGenerator, IJdmRepository jdmRepository, IJdmManufacturerRepository jdmManufacturerRepository, IAuthService authService, IWebHostEnvironment webHostEnvironment, IJdmJournalCommentRepository journalCommentRepository)
        {
            _jdmItemGenerator = jdmItemGenerator;
            _jdmCatalogGenerator = jdmCatalogGenerator;
            _jdmRepository = jdmRepository;
            _jdmManufacturerRepository = jdmManufacturerRepository;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
            _journalCommentRepository = journalCommentRepository;
        }
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Catalog(string manufacturerType)
        {
            var carsWithoutInspection = _jdmRepository.GetCarsNotVehicleInspectionHistory();
            var jdmCarsData = _jdmRepository.GetAll();
            var jdmItems = _jdmItemGenerator.GenerateJDMCarsItems(jdmCarsData);
            var catalogAuto = _jdmCatalogGenerator.GetManufacturerTypeFromJDMItems(jdmItems, manufacturerType);
            var viewModel = new CatalogCarsPermissionViewModel
            {
                CatalogAuto = catalogAuto,
                CarsWithoutInspection = carsWithoutInspection.Select(x => new VehicleInspectionHistoryItemViewModel
                {
                    Manufacturer = x.Manufacturer,
                    CountCars = x.CountCars
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateCars()
        {
            var viewModel = new JapaneseDomesticMarketViewModels
            {
                AllManufacturer = _jdmItemGenerator.GetListItemsJdmCars()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateCars(JapaneseDomesticMarketViewModels viewModel, IFormFile VehicleInspectionHistoryUrl)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AllManufacturer = _jdmItemGenerator.GetListItemsJdmCars();
                return View(viewModel);
            }

            if (viewModel.ManufactureId is null || viewModel.ManufactureId <= 0)
            {
                var manufactures = _jdmManufacturerRepository.GetAll();
                viewModel.AllManufacturer.AddRange(manufactures.Select(x => new SelectListItem
                {
                    Text = x.ManufacturerType,
                    Value = x.Id.ToString(),
                }));
                return View(viewModel);
            }
            var manufacturer = _jdmManufacturerRepository.Get(viewModel.ManufactureId.Value);
            if (manufacturer is null)
            {
                return RedirectToAction(nameof(Catalog));
            }

            var user = _authService.GetUser();
            if (user is not null)
            {
                var jdmCarsData = new JdmCarsData
                {
                    CreatorId = user.Id,
                    Marka = viewModel.Marka,
                    Model = viewModel.Model,
                    Price = viewModel.Price,
                    Url = viewModel.Url,
                    JdmManufacturerDataId = manufacturer.Id,
                    ManufacturerType = manufacturer.ManufacturerType
                };
                _jdmRepository.Add(jdmCarsData);

                if (viewModel.VehicleInspectionHistoryUrl is not null && viewModel.VehicleInspectionHistoryUrl.Length > 0)
                {
                    var carsId = jdmCarsData.Id;
                    var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
                    var folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "japanese-domestic-market", "history-inspection");
                    var fileName = $"history-inspection-{carsId}.txt";
                    var path = Path.Combine(folder, fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        VehicleInspectionHistoryUrl.CopyTo(fileStream);
                    }
                    jdmCarsData.VehicleInspectionHistoryUrl = "/" + Path.Combine(pathToWwwRootFolder, fileName).Replace("/", "\\");
                    _jdmRepository.Update(jdmCarsData);
                }

                return RedirectToAction(nameof(Catalog));
            }
            return View(viewModel);

        }

        [HttpGet]
        [Authorize]
        public IActionResult JDMBuilder()
        {
            var jdmCars = _jdmRepository.GetAll();
            var carsJdmViewModel = _jdmItemGenerator.GenerateJDMCarsItems(jdmCars);
            var viewModel = new JDMCatalogViewModels
            {
                CarsJDMItems = carsJdmViewModel
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult JDMBuilder(JapaneseDomesticMarketViewModels jdmItem)
        {
            var jdmCarsData = new JdmCarsData
            {
                ManufacturerType = jdmItem.ManufacturerType,
                Marka = jdmItem.Marka,
                Model = jdmItem.Model,
                Price = jdmItem.Price,
                Url = jdmItem.Url,
            };

            _jdmRepository.Add(jdmCarsData);

            return RedirectToAction(nameof(Catalog));
        }

        public IActionResult Journal()
        {
            var pageJournal = new JournalPageViewModel
            {
                Posts = new List<JournalPostViewModel>
         {
             new() { PostId = (int)JdmJournalPostId.Welcome },
             new() { PostId = (int)JdmJournalPostId.Raubichi },
             new() { PostId = (int)JdmJournalPostId.Lida },
         }
            };
            foreach (var post in pageJournal.Posts)
            {
                post.Comments = _journalCommentRepository
                    .GetByPostId(post.PostId)
                    .Select(c => new JournalCommentsViewModel
                    {
                        AuthorName = c.User.Name,
                        Text = c.Text,
                        CreatedDate = c.CreatedDate
                    })
                    .ToList();
                post.Form.PostId = post.PostId;
            }
            return View(pageJournal);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddComment(AddJournalCommentViewModel comment)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Journal));
            var user = _authService.GetUser()!;
            _journalCommentRepository.Add(new JdmCarsBlogCommentsData
            {
                PostsId = comment.PostId,
                Text = comment.Text.Trim(),
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow
            });
            return RedirectToAction(nameof(Journal), null, null, $"post-{comment.PostId}");
        }

        /*public IActionResult LinkJdmQuiz(int manufactureId, int modelId)
        {
            _jdmRepository.Link(manufactureId, modelId);
            return RedirectToAction(nameof(Home));
        }*/
        [IsJdmOwner]
        public IActionResult DeleteComments()
        {
            return RedirectToAction(nameof(Journal));
        }
    }
}