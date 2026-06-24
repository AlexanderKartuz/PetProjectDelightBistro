using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface IJdmPostsRepository : IBaseRepository<JdmPostsData>
    {
        List<JdmPostsData> GetPublishedPosts();
        List<JdmPostsData> GetByPostId(int postId);
        int DeleteOldPosts(DateTime oldTimePublished);
        List<JdmPostsData> GetByOldPosts(DateTime oldTimePublished);
    }
}
