using DelightBistroMvc.Models.DTOs;

namespace DelightBistroMvc.Models.DelightBistro
{
    public class MainIndexViewModel
    {
        public List<MenuTypeViewModel> MenuTypeViewModels { get; set; }

        // API
        public CatFactDto CatFactViewModel { get; set; }
        public DogDto DogViewModel { get; set; }

    }
}
