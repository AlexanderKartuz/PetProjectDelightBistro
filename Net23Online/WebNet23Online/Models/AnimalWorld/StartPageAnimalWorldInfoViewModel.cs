using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Models.AnimalWorld
{
    public class StartPageAnimalWorldInfoViewModel
    {
        public List<AnimalFamilyViewModel> AnimalFamilies { get; set; }

        public List<AnimalSpeciesViewModel> AnimalSpecies { get; set; }

        public List<AnimalWorldRandomAnimalDto> RandomAnimals { get; set; }
    }
}
