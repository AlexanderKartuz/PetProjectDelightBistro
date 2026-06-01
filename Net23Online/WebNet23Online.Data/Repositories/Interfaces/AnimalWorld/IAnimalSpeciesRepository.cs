using WebNet23Online.Data.Models.AnimalWorld;

namespace WebNet23Online.Data.Repositories.Interfaces.AnimalWorld
{
    public interface IAnimalSpeciesRepository : IAnimalWorldRepository<AnimalSpeciesData>
    {
        List<string> GetAllAnimalSpeciesNames();
    }
}
