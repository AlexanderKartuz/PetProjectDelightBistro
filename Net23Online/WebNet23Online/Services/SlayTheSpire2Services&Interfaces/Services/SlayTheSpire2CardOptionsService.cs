using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class SlayTheSpire2CardOptionsService : ISlayTheSpire2CardOptionsService
    {
        private readonly IReadOnlyList<string> _raritiesSource = new[] { "Basic", "Common", "Uncommon", "Rare", "Ancient" };
        private readonly IReadOnlyList<string> _typesSource = new[] { "Attack", "Skill", "Power" };

        public IReadOnlyList<string> Rarities => _raritiesSource;

        public IReadOnlyList<string> Types => _typesSource;

        public List<SelectListItem> BuildRaritySelectList(string? selectedRarity) =>
            BuildSelectList(_raritiesSource, selectedRarity);

        public List<SelectListItem> BuildTypeOfCardSelectList(string? selectedType) =>
            BuildSelectList(_typesSource, selectedType);

        private List<SelectListItem> BuildSelectList(IReadOnlyList<string> source, string? selectedValue)
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
