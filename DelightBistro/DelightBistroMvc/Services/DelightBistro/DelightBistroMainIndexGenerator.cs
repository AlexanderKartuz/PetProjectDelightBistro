using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.Apis;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
{
    public class DelightBistroMainIndexGenerator : IDelightBistroMainIndexGenerator
    {
        private IMenuTypeGenerator _menuTypeGenerator;

        private CatFactApi _catFactApi;
        private DogApi _dogApi;

        public DelightBistroMainIndexGenerator(IMenuTypeGenerator menuTypeGenerator, CatFactApi catFactApi, DogApi dogApi)
        {
            _menuTypeGenerator = menuTypeGenerator;
            _catFactApi = catFactApi;
            _dogApi = dogApi;
        }

        public MainIndexViewModel GetMainIndexViewModel(string menuType)
        {
            var catFactTask = _catFactApi.GetCatFact();
            var dogTask = _dogApi.GetDog();

            Task.WaitAll(catFactTask, dogTask);

            var catFact = catFactTask.Result;
            var dogDto = dogTask.Result;

            var mainIndexViewModel = new MainIndexViewModel
            {
                MenuTypeViewModels = _menuTypeGenerator.GetAllMenuViewModel(menuType),
                CatFactViewModel = catFact,
                DogViewModel = dogDto,
            };

            return mainIndexViewModel;
        }
    }
}
