using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NAudio;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using WebNet23Online.Data.Repositories;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.JapaneseDomesticMarket;
using WebNet23Online.Services;
using WebNet23Online.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Controllers
{
    public class JapaneseDomesticMarketController : Controller
    {
        private IJapaneseDomesticMarketGenerator _jdmItemGenerator;
        private IJDMCatalogGenerator _jdmCatalogGenerator;
        private IJdmRepository _jdmRepository;
        private IJdmManufacturerRepository _jdmManufacturerRepository;

        public JapaneseDomesticMarketController(IJapaneseDomesticMarketGenerator jdmItemGenerator, IJDMCatalogGenerator jdmCatalogGenerator, IJdmRepository jdmRepository, IJdmManufacturerRepository jdmManufacturerRepository)
        {
            _jdmItemGenerator = jdmItemGenerator;
            _jdmCatalogGenerator = jdmCatalogGenerator;
            _jdmRepository = jdmRepository;
            _jdmManufacturerRepository = jdmManufacturerRepository;
        }
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Catalog(string manufacturerType)
        {
            // var jdmItems = _jdmItemGenerator.GenerateJDMCarsItems();
            var JdmCarsData = _jdmRepository.GetAll();
            var jdmItems = _jdmItemGenerator.GenerateJDMCarsItems(JdmCarsData);
            var viewModel = _jdmCatalogGenerator.GetManufacturerTypeFromJDMItems(jdmItems, manufacturerType);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateCars()
        {
            var manufactures = _jdmManufacturerRepository.GetAll();
            var manufacturesListItems = new List<SelectListItem>();

            manufacturesListItems.Add(new SelectListItem
            {
                Text = "Выбери производителя",
                Value = ""
            });
            manufacturesListItems.AddRange(manufactures.Select(x => new SelectListItem
            {
                Text = x.ManufacturerType,
                Value = x.Id.ToString()
            }));
            var viewModel = new JapaneseDomesticMarketViewModels
            {
                AllManufacturer = manufacturesListItems
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateCars(JapaneseDomesticMarketViewModels viewModel)
        {
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
            var jdmCarsData = new JdmCarsData
            {
                Id = viewModel.Id,
                Marka = viewModel.Marka,
                Model = viewModel.Model,
                Price = viewModel.Price,
                Url = viewModel.Url,
                ManufacturerType = manufacturer.ManufacturerType
            };
            _jdmRepository.Add(jdmCarsData);
            return RedirectToAction(nameof(Catalog));
        }

        [HttpGet]
        public IActionResult JDMBuilder()
        {
            var jdmCars = _jdmRepository.GetAll();
            var carsJdmViewModel = _jdmItemGenerator.GenerateJDMCarsItems(jdmCars);
            var viewModel = new JDMCatalogViewModels
            {
                CarsJDMItems = carsJdmViewModel
            };
            return View();
        }

        [HttpPost]
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
            return View();
        }

        /*public IActionResult LinkJdmQuiz(int manufactureId, int modelId)
        {
            _jdmRepository.Link(manufactureId, modelId);
            return RedirectToAction(nameof(Home));
        }*/
    }
}