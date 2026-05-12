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
                ZooName = zoo.ZooName,
                Address = zoo.Address,
                AnimalFamilies = FromAnimalFamilyDataToAnimalFamilyViewModel(zoo.AnimalSpecies.Select(s => s.AnimalFamily).DistinctBy(s => s.AnimalFamilyName).ToList()),
                AnimalSpecies = FromAnimalSpeciesDataToAnimalSpeciesViewModel(zoo.AnimalSpecies),
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
            });
            return animalSpecies.ToList();
        }
    }
}
