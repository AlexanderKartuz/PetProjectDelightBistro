using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Models.DelightBistro
{
    public class MainIndexViewModel
    {
        public List<MenuTypeViewModel> MenuTypeViewModels { get; set; }

        // API
        public CatFactDto CatFactViewModel { get; set; }
        public DogDto DogViewModel { get; set; }

    }
}
