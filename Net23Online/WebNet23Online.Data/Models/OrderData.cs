using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNet23Online.Data.Models
{
    public class OrderData : BaseModel
    {
        public DateTime CreatedDateTime { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual List<FoodItemData> FoodItems { get; set; } = new();
        public int UserId { get; set; }
        public virtual UserData User { get; set; }
    }
}
