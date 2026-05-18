using WebNet23Online.Data.Models.AnimalWorld;

namespace WebNet23Online.Data.Repositories.Interfaces.AnimalWorld
{
    public interface IZooRepository : IAnimalWorldRepository<ZooData>
    {
        void AddAnimalSpecies(int zooId, int animalSpeciesId);
        List<string> GetZooAnimalFamilies(int id);
    }
}
