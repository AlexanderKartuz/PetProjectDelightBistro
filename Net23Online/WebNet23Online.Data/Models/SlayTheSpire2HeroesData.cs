using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNet23Online.Data.Models
{
    public class SlayTheSpire2HeroesData:BaseModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual List<SlayTheSpire2HeroesCards> Cards { get; set; } = new();
    }
}
