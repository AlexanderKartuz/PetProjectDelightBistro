using WebNet23Online.Models.CustomValidatioAttributes.DelightBistro;

namespace WebNet23Online.Models.DelightBistro
{
    public class CreateMenuViewModel
    {
        [IsUniqueMenu]
        public string Name { get; set; }
        public string? Creator { get; set; }
    }
}
