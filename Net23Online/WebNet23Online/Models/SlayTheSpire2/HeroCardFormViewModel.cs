using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes;

namespace WebNet23Online.Models.SlayTheSpire2
{
    public class HeroCardFormViewModel
    {
        public const int MaxHeroId = 5;

        public int CardId { get; set; }

        [Range(1, MaxHeroId, ErrorMessage = "Выберите героя")]
        [Display(Name = "Герой")]
        public int HeroId { get; set; }

        public string? HeroName { get; set; }

        public List<SelectListItem> HeroOptions { get; set; } = new();

        public List<SelectListItem> RarityOptions { get; set; } = new();

        public List<SelectListItem> TypeOfCardOptions { get; set; } = new();

        [Required(ErrorMessage = "Укажите название карты")]
        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите описание")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите редкость")]
        [Display(Name = "Редкость")]
        public string Rarity { get; set; } = string.Empty;

        [MinMaxCheckAttribute(0, 4, ErrorMessage = "Мана должна быть от 0 до 4")]
        [Display(Name = "Мана")]
        public int ManaCost { get; set; }

        [Required(ErrorMessage = "Укажите тип карты")]
        [Display(Name = "Тип карты")]
        public string TypeOfCard { get; set; } = string.Empty;

        [Display(Name = "Улучшенная")]
        public bool Upgraded { get; set; }

        [Display(Name = "URL изображения")]
        public string? ImageUrl { get; set; }

        public bool IsNew => CardId == 0;
    }
}
