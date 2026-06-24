using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNet23Online.Data.Models
{
    public class JdmPostsData : BaseModel
    {
        public string Title { get; set; }
        public string? Text { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? UrlPicture { get; set; }
        public bool IsPublished { get; set; }
        public List<JdmCarsBlogCommentsData> Comments { get; set; }
    }
}
