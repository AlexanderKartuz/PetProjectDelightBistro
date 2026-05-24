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
        public List<FoodItemData> FoodItems { get; set; }
        public decimal TotalPrice { get; set; }
        public UserData User { get; set; }
    }
}
