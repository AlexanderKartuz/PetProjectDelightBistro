using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public abstract class BaseRepository<DataModel>
        : IBaseRepository<DataModel> where DataModel : BaseModel
    {
        protected WebContext _context;
        protected DbSet<DataModel> _dbSet;

        public BaseRepository(WebContext context)
        {
            _context = context;
            _dbSet = _context.Set<DataModel>();
        }

        public virtual void Add(DataModel model)
        {
            _dbSet.Add(model);
            _context.SaveChanges();
        }

        public virtual void Remove(DataModel model)
        {
            _dbSet.Remove(model);
            _context.SaveChanges();
        }

        public virtual DataModel? Get(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual bool Any()
        {
            return _dbSet.Any();
        }

        public virtual List<DataModel> GetAll()
        {
            return _dbSet.ToList();
        }

        public List<DataModel> GetAllWithExpression(string? sortBy,
            string? direction,
            string? sortType,
            string? sortValue)
        {
            var dataSource = _dbSet
                .AsQueryable();

            if (sortBy is null)
            {
                return dataSource.ToList();
            }

            // goal
            // dataSource = dataSource.OrderBy(girl => girl.Id);

            // girl
            var parameter = Expression.Parameter(typeof(DataModel), "girl");

            // girl.Id
            var field = Expression.Property(parameter, sortBy);

            // int
            var sortPropertyType = typeof(DataModel)
                .GetProperty(sortBy)!
                .PropertyType;

            // girl => girl.Id
            var lambdaForOrder = Expression.Lambda(field, parameter);

            var methodName = direction is null
                || direction == "asc"
                ? "OrderBy"
                : "OrderByDescending";

            // dataSource.OrderBy<DataModel, int>(girl => girl.Id);
            var orderByMethod = typeof(Queryable)
                .GetMethods()
                .First(x => x.Name == methodName
                    && x.GetParameters().Count() == 2)
                .MakeGenericMethod(typeof(DataModel), sortPropertyType);
            var sortedSource =
                (IQueryable<DataModel>)orderByMethod.Invoke(null, [dataSource, lambdaForOrder])!;

            if (sortType is null || string.IsNullOrEmpty(sortType))
            {
                return sortedSource.ToList();
            }

            // dataSource.Where<DataModel>(girl => girl.Id > 50);
            var convertedSortValue = Convert.ChangeType(sortValue, sortPropertyType);
            var constSortValue = Expression.Constant(convertedSortValue);
            Expression filterExpression;
            //  girl.Id > 50
            if (sortType == "more")
            {
                filterExpression = Expression.GreaterThan(field, constSortValue);
            }
            else if (sortType == "less")
            {
                filterExpression = Expression.LessThan(field, constSortValue);
            }
            else if (sortType == "eq")
            {
                filterExpression = Expression.Equal(field, constSortValue);
            }
            else
            {
                throw new Exception($"Unkonw filter type: {sortType}");
            }

            // girl => girl.id > 50
            var lambdaForWhere = Expression.Lambda(filterExpression, parameter);

            var whereMethod = typeof(Queryable)
               .GetMethods()
               .First(x => x.Name == "Where")
               .MakeGenericMethod(typeof(DataModel));

            var filteredAndSortedSource =
                (IQueryable<DataModel>)whereMethod.Invoke(null, [sortedSource, lambdaForWhere])!;

            return filteredAndSortedSource.ToList();
        }

        public virtual void Update(DataModel model)
        {
            _dbSet.Update(model);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var user = _dbSet.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _dbSet.Remove(user);
                _context.SaveChanges();
            }
        }

        public virtual void Delete(List<int> ids)
        {
            var models = _dbSet.Where(x => ids.Contains(x.Id));
            if (models.Any())
            {
                _dbSet.RemoveRange(models);
                _context.SaveChanges();
            }
        }

        public virtual List<DataModel> GetByIds(List<int> ids)
        {
            var foodItems = _dbSet.Where(x => ids.Contains(x.Id)).ToList();
            return foodItems;
        }

        public void Update(List<DataModel> models)
        {
            _dbSet.UpdateRange(models);
            _context.SaveChanges();
        }
    }
}
