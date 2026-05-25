using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebNet23Online.Services.Interfaces
{
    public interface ISlayTheSpire2CardOptionsService
    {
        IReadOnlyList<string> Rarities { get; }

        IReadOnlyList<string> Types { get; }

        List<SelectListItem> BuildRaritySelectList(string? selectedRarity);

        List<SelectListItem> BuildTypeOfCardSelectList(string? selectedType);
    }
}
