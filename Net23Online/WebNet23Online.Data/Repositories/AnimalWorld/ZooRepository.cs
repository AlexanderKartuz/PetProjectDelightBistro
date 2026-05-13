using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;

namespace WebNet23Online.Data.Repositories.AnimalWorld
{
    public class ZooRepository : BaseRepository<ZooData>, IZooRepository
    {
        public const int START_PAGE_COUNT_ANIMAL_SPECIES = 3;

        public ZooRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<ZooData> GetRandomElements()
        {
            return _dbSet.Include(zoo => zoo.AnimalSpecies).ThenInclude(species => species.AnimalFamily).OrderBy(r => Guid.NewGuid()).Take(START_PAGE_COUNT_ANIMAL_SPECIES).ToList();
        }

        public ZooData GetElementByName(string name)
        {
            return _dbSet.FirstOrDefault(animal => animal.ZooName.ToLower() == name.ToLower());
        }

        public void AddAnimalSpecies(int zooId, int animalSpeciesId)
        {
            var zoo = _dbSet.Include(animal => animal.AnimalSpecies).First(zoo => zoo.Id == zooId);
            var animalSpecies = _context.AnimalSpecies.First(animalSpecies => animalSpecies.Id == animalSpeciesId);
            zoo.AnimalSpecies.Add(animalSpecies);
            _context.SaveChanges();
        }

        public List<ZooData> GetAllWithAnimalSpecies()
        {
            return _dbSet.Include(zoo => zoo.AnimalSpecies).ThenInclude(species => species.AnimalFamily).ToList();
        }
    }
}
