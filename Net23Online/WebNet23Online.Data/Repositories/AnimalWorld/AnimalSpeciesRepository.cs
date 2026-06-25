using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
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

        public List<AnimalSpeciesData> GetAllWithFamilies(string? searchCategory, string? searchQuery)
        {
            //return _dbSet.Include(s => s.AnimalFamily).ToList();

            var dataSource = _dbSet.Include(s => s.AnimalFamily).AsQueryable();
            if (string.IsNullOrEmpty(searchCategory) || string.IsNullOrEmpty(searchQuery))
            {
                return dataSource.ToList();
            }

            var queryValue = searchQuery.ToLower();
            var parameter = Expression.Parameter(typeof(AnimalSpeciesData), "animal");
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var constQuery = Expression.Constant(queryValue, typeof(string));
            Expression finalExpression;
            if (searchCategory == "Species")
            {
                var field = Expression.Property(parameter, nameof(AnimalSpeciesData.AnimalSpeciesName));
                finalExpression = BuildContainsExpression(field, toLowerMethod, containsMethod, constQuery);
            }
            else if (searchCategory == "Family")
            {
                var familyProp = Expression.Property(parameter, nameof(AnimalSpeciesData.AnimalFamily));
                var familyNameProp = Expression.Property(familyProp, "AnimalFamilyName");
                finalExpression = BuildContainsExpression(familyNameProp, toLowerMethod, containsMethod, constQuery);
            }
            else if (searchCategory == "Range")
            {
                var field = Expression.Property(parameter, nameof(AnimalSpeciesData.NativeRange));
                finalExpression = BuildContainsExpression(field, toLowerMethod, containsMethod, constQuery);
            }
            else
            {
                var speciesField = Expression.Property(parameter, nameof(AnimalSpeciesData.AnimalSpeciesName));
                var familyProp = Expression.Property(parameter, nameof(AnimalSpeciesData.AnimalFamily));
                var familyNameProp = Expression.Property(familyProp, "AnimalFamilyName");
                var rangeField = Expression.Property(parameter, nameof(AnimalSpeciesData.NativeRange));

                var speciesExp = BuildContainsExpression(speciesField, toLowerMethod, containsMethod, constQuery);
                var familyExp = BuildContainsExpression(familyNameProp, toLowerMethod, containsMethod, constQuery);
                var rangeExp = BuildContainsExpression(rangeField, toLowerMethod, containsMethod, constQuery);
                finalExpression = Expression.OrElse(Expression.OrElse(speciesExp, familyExp), rangeExp);
            }

            if (finalExpression != null)
            {
                var lambda = Expression.Lambda<Func<AnimalSpeciesData, bool>>(finalExpression, parameter);
                dataSource = dataSource.Where(lambda);
            }

            return dataSource.ToList();
        }

        private Expression BuildContainsExpression(Expression propertyField, MethodInfo toLowerMethod, MethodInfo containsMethod, ConstantExpression constQuery)
        {
            var toLowerCall = Expression.Call(propertyField, toLowerMethod);
            return Expression.Call(toLowerCall, containsMethod, constQuery);
        }
    }
}
