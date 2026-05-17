using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class SlayTheSpire2CardOptionsService : ISlayTheSpire2CardOptionsService
    {
        private static readonly string[] RaritiesSource = { "Basic", "Common", "Uncommon", "Rare", "Ancient" };
        private static readonly string[] TypesSource = { "Attack", "Skill", "Power" };

        public IReadOnlyList<string> Rarities => RaritiesSource;

        public IReadOnlyList<string> Types => TypesSource;

        public List<SelectListItem> BuildRaritySelectList(string? selectedRarity) =>
            BuildSelectList(RaritiesSource, selectedRarity);

        public List<SelectListItem> BuildTypeOfCardSelectList(string? selectedType) =>
            BuildSelectList(TypesSource, selectedType);

        private static List<SelectListItem> BuildSelectList(string[] source, string? selectedValue)
        {
            var values = source.ToList();
            if (!string.IsNullOrWhiteSpace(selectedValue)
                && !values.Any(x => string.Equals(x, selectedValue, StringComparison.OrdinalIgnoreCase)))
            {
                values.Add(selectedValue);
            }

            return values
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x,
                    Selected = string.Equals(x, selectedValue, StringComparison.OrdinalIgnoreCase)
                })
                .ToList();
        }
    }
}
