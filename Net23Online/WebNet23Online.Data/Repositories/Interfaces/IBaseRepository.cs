using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface IBaseRepository<DataModel>
        where DataModel : BaseModel
    {
        public void Add(DataModel model);
        public List<DataModel> GetAll();
        public void Remove(DataModel model);
        public DataModel? Get(int id);
        public void Update(DataModel model);
        public void Update(List<DataModel> models);
        public void Delete(int id);
        void DeleteRange(List<DataModel> models);
        public bool Any();
        void Delete(List<int> ids);
        List<DataModel> GetByIds(List<int> ids);
        List<DataModel> GetAllWithExpression(string? sortBy, 
            string? direction, 
            string? sortType, 
            string? sortValue);
    }
}