using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Models.AnimalWorld;

namespace WebNet23Online.Services.Interfaces
{
    public interface IAnimalWorldMapper
    {
        List<ZooViewModel> FromZooDataToZooViewModel(List<ZooData> zoosData);

        List<AnimalFamilyViewModel> FromAnimalFamilyDataToAnimalFamilyViewModel(List<AnimalFamilyData> animalFamiliesData);

        List<AnimalSpeciesViewModel> FromAnimalSpeciesDataToAnimalSpeciesViewModel(List<AnimalSpeciesData> animalSpeciesData);

        List<AnimalSpeciesInfoViewModel> FromAnimalSpeciesDataToAnimalSpeciesInfoViewModel(List<AnimalSpeciesData> animalSpeciesData);

        List<PromotionViewModel> FromPromotionDataToPromotionViewModel(List<PromotionData> promotionsData);
    }
}
