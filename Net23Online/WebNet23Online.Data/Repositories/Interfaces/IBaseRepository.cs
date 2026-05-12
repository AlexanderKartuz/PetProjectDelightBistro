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
        public void Delete(int id);
        public bool Any();
    }
}