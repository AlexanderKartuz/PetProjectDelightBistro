using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models.AnimalWorld;

namespace WebNet23Online.Data.Models
{
    public class CommentData : BaseModel
    {
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public EntityType CommentType { get; set; }
        public int? ZooId { get; set; }
        public virtual UserData Author { get; set; }
        public virtual ZooData Zoo { get; set; }
    }
}
