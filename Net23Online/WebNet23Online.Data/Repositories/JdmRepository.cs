using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class JdmRepository : BaseRepository<JdmCarsData>, IJdmRepository
    {
        public JdmRepository(WebContext webContext) : base(webContext)
        {

        }
        public List<JdmCarsData> IncludeManufactureData()
        {
            return _dbSet
                .Include(g => g.JdmManufacturerData)
                .ToList();
        }

        /*void Link(int manufactureId, int modelId)
        {
            var manufacture = _context.JdmManufacturer.First(x => x.Id == manufactureId);
            var carsJdm = _context.JdmCars.First(x => x.Id == modelId);
            manufacture.JdmCarsDatas.Add(carsJdm);
            _context.SaveChanges();
        }*/

        public List<VehicleInspectionHistoryDataModel> GetCarsNotVehicleInspectionHistory()
        {
            var sql = @"SELECT MIN (JdmCars.ManufacturerType) Manufacturer, COUNT(JdmCars.Id) CountCars
            FROM JdmCars
            WHERE JdmCars.VehicleInspectionHistoryUrl IS NULL 
            AND JdmCars.Price > 10000
            GROUP BY JdmCars.ManufacturerType";
            var results = _context
                  .Database
                  .SqlQueryRaw<VehicleInspectionHistoryDataModel>(sql)
                  .ToList();
            return results;
        }
    }
}