namespace DelightBistroMinimalApi.DbStuff
{
    public class TeaRepository
    {
        private MiniDbContext _dbContext;

        public TeaRepository(MiniDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Tea> GetTeas()
        {
            var teasDb = _dbContext.Teas.ToList();
            return teasDb;
        }

        public Tea? GetTea(int id)
        {
            var teaDb = _dbContext.Teas.FirstOrDefault(t => t.Id == id);
            return teaDb;
        }

        public void CreateTea(Tea tea)
        {
            _dbContext.Teas.Add(tea);
            _dbContext.SaveChanges();
        }

        public Tea? ChangeTea(int id, Tea tea)
        {
            var changedTea = _dbContext.Teas.FirstOrDefault(t => t.Id == id);

            if (changedTea == null)
            {
                return null;
            }

            changedTea.Name = tea.Name;
            changedTea.Price = tea.Price;
            changedTea.Description = tea.Description;
            changedTea.ImgUrl = tea.ImgUrl;

            _dbContext.SaveChanges();
            return changedTea;
        }

        public bool DeleteTea(int id)
        {
            var tea = _dbContext.Teas.FirstOrDefault(i => i.Id == id);

            if (tea == null)
            {
                return false;
            }
            _dbContext.Teas.Remove(tea);
            _dbContext.SaveChanges();
            return true;
        }
    }
}