using WebNet23Online.Models.DelightBistro;
using WebNet23Online.Services.Apis;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services.DelightBistro
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
