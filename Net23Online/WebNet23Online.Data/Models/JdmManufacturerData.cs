using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNet23Online.Data.Models
{
    public class JdmManufacturerData : BaseModel
    {
        public string ManufacturerType { get; set; } = "";
        public virtual List<JdmCarsData> JdmCarsDatas { get; set; } = new();
    }
}