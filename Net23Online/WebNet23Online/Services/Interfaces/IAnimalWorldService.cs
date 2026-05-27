using WebNet23Online.Models.AnimalWorld;

namespace WebNet23Online.Services.Interfaces
{
    public interface IAnimalWorldService
    {
        StartPageAnimalWorldInfoViewModel GetStartInfo();

        AnimalSpeciesViewModel GetAnimalSpeciesPageInfo();

        BindZooWithAnimalSpeciesViewModel GetBingZooAndAnimalSpeciesInfo();

        bool AddZoo(ZooViewModel viewModel);

        bool AddAnimalFamily(AnimalFamilyViewModel viewModel);

        bool AddAnimalSpecies(AnimalSpeciesViewModel viewModel);
        bool BindZooWithAnimalSpecies(int zooId, int animalSpeciesId);
        List<ZooViewModel> GetAllZoos();

        string GetZooName(int zooId);
        string GetAnimalSpeciesName(int animalSpeciesId);
    }
}