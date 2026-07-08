using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Models.AnimalWorld;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class AnimalWorldMapper : IAnimalWorldMapper
    {
        public List<ZooViewModel> FromZooDataToZooViewModel(List<ZooData> zoosData)
        {
            var zoos = zoosData.Select(zoo => new ZooViewModel
            {
                Id = zoo.Id,
                ZooName = zoo.ZooName,
                Address = zoo.Address,
                Description = zoo.Description,
            });
            return zoos.ToList();
        }

        public List<AnimalFamilyViewModel> FromAnimalFamilyDataToAnimalFamilyViewModel(List<AnimalFamilyData> animalFamiliesData)
        {
            var animalFamilies = animalFamiliesData.Select(animalFamily => new AnimalFamilyViewModel
            {
                AnimalFamilyName = animalFamily.AnimalFamilyName,
                Description = animalFamily.Description,
            });
            return animalFamilies.ToList();
        }

        public List<AnimalSpeciesViewModel> FromAnimalSpeciesDataToAnimalSpeciesViewModel(List<AnimalSpeciesData> animalSpeciesData)
        {
            var animalSpecies = animalSpeciesData.Select(animalSpecies => new AnimalSpeciesViewModel
            {
                AnimalSpeciesName = animalSpecies.AnimalSpeciesName,
                Url = animalSpecies.AnimalSpeciesUrl,
                NativeRange = animalSpecies.NativeRange,
                Description = animalSpecies.Description,
                Zoos = animalSpecies.ZooData.Select(s => s.ZooName).ToList()
            });
            return animalSpecies.ToList();
        }

        public List<AnimalSpeciesInfoViewModel> FromAnimalSpeciesDataToAnimalSpeciesInfoViewModel(List<AnimalSpeciesData> animalSpeciesData)
        {
            var animalSpecies = animalSpeciesData.Select(animalSpecies => new AnimalSpeciesInfoViewModel
            {
                AnimalSpeciesName = animalSpecies.AnimalSpeciesName,
                AnimalFamilyName = animalSpecies.AnimalFamily.AnimalFamilyName,
                NativeRange = animalSpecies.NativeRange
            });
            return animalSpecies.ToList();
        }

        public List<PromotionViewModel> FromPromotionDataToPromotionViewModel(List<PromotionData> promotionsData)
        {
            var promotions = promotionsData.Select(promotion => new PromotionViewModel
            {
                PromotionName = promotion.PromotionName,
                Description = promotion.Description,
                Place = promotion.Venue.ZooName,
                EndDate = promotion.EndDate,
                ZooId = promotion.ZooId,
            });
            return promotions.ToList();
        }
    }
}
