using WebNet23Online.Models.DelightBistro;
using WebNet23Online.Services.Apis;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services.DelightBistro
{
    public class DelightBistroMainIndexGenerator : IDelightBistroMainIndexGenerator
    {
        private IMenuTypeGenerator _menuTypeGenerator;
        private CatFactApi _catFactApi;

        public DelightBistroMainIndexGenerator(IMenuTypeGenerator menuTypeGenerator, CatFactApi catFactApi)
        {
            _menuTypeGenerator = menuTypeGenerator;
            _catFactApi = catFactApi;
        }

        public MainIndexViewModel GetMainIndexViewModel(string menuType)
        {
            var catFactTask = _catFactApi.GetCatFact();
            Task.WaitAll(catFactTask);

            var catFact = catFactTask.Result;

            var mainIndexViewModel = new MainIndexViewModel
            {
                MenuTypeViewModels = _menuTypeGenerator.GetAllMenuViewModel(menuType),
                CatFactViewModel = catFact,
            };

            return mainIndexViewModel;
        }
    }
}
