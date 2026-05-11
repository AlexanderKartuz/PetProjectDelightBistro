using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNet23Online.Data.DataModels;

namespace WebNet23Online.Data.Models
{
    public class IngredientData : BaseModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public virtual List<FoodItemData> FoodItems { get; set; } = new();
        public virtual List<FoodItemIngredientData> FoodItemIngredientDatas { get; set; } = new();
        public int? CreatorId { get; set; }
        public virtual UserData? Creator { get; set; }
    }
}
