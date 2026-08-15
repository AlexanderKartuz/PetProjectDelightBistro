using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Models.DTOs;
using DelightBistroMvc.Services.Apis;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
{
    public class DelightBistroMainIndexGenerator : IDelightBistroMainIndexGenerator
    {
        private IMenuTypeGenerator _menuTypeGenerator;

        private CatFactApi _catFactApi;
        private DogApi _dogApi;

        public DelightBistroMainIndexGenerator(
            IMenuTypeGenerator menuTypeGenerator,
            CatFactApi catFactApi,
            DogApi dogApi)
        {
            _menuTypeGenerator = menuTypeGenerator;
            _catFactApi = catFactApi;
            _dogApi = dogApi;
        }

        public async Task<MainIndexViewModel> GetMainIndexViewModelAsync(string menuType)
        {
            var catFactTask = _catFactApi.GetCatFact();
            var dogTask = _dogApi.GetDog();

            var menus = _menuTypeGenerator.GetAllMenuViewModel(menuType);

            CatFactDto catFact;
            DogDto dogDto;

            try
            {
                await Task.WhenAll(catFactTask, dogTask);
                catFact = await catFactTask;
                dogDto = await dogTask;
            }
            catch
            {
                catFact = new CatFactDto()
                {
                    Fact = "Факто временно не доступен",
                    Length = 0,
                };

                dogDto = new DogDto()
                {
                    Message = "",
                    Status = "error",
                };
            }

            var mainIndexViewModel = new MainIndexViewModel
            {
                MenuTypeViewModels = menus,
                CatFactViewModel = catFact,
                DogViewModel = dogDto,
            };

            return mainIndexViewModel;
        }
    }
}
