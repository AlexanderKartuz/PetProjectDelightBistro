using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class JdmManufacturerRepository : BaseRepository<JdmManufacturerData>, IJdmManufacturerRepository
    {
        public JdmManufacturerRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
