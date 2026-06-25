using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;

namespace WebNet23Online.Data.Repositories.AnimalWorld
{
    public class AnimalSpeciesRepository : BaseRepository<AnimalSpeciesData>, IAnimalSpeciesRepository
    {
        public const int START_PAGE_COUNT_ANIMAL_SPECIES = 3;

        public AnimalSpeciesRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<AnimalSpeciesData> GetRandomElements()
        {
            return _dbSet.Include(s => s.ZooData).OrderBy(r => Guid.NewGuid()).Take(START_PAGE_COUNT_ANIMAL_SPECIES).ToList();
        }

        public AnimalSpeciesData GetElementByName(string name)
        {
            return _dbSet.FirstOrDefault(animal => animal.AnimalSpeciesName.ToLower() == name.ToLower());
        }

        public List<string> GetAllAnimalSpeciesNames()
        {
            var sql = @$"SELECT AnimalSpeciesName
                         FROM AnimalSpecies";
            return _context.Database.SqlQueryRaw<string>(sql).ToList();
        }

        public List<AnimalSpeciesData> GetAllWithFamilies()
        {
            return _dbSet.Include(s => s.AnimalFamily).ToList();
        }
    }
}
