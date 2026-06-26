using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNet23Online.Data.Models
{
    public class JdmCarsBlogCommentsData : BaseModel
    {
        public int PostsId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public virtual UserData User { get; set; } = null!;
        public virtual JdmPostsData Posts { get; set; }
    }
}