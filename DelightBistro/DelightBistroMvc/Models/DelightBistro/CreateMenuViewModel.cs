using DelightBistroMvc.Models.CustomValidatioAttributes.DelightBistro;

namespace DelightBistroMvc.Models.DelightBistro
{
    public class CreateMenuViewModel
    {
        [IsUniqueMenu]
        public string Name { get; set; }
        public string? Creator { get; set; }
    }
}
