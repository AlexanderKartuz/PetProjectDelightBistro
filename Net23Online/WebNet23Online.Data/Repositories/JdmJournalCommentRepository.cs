using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class JdmJournalCommentRepository : BaseRepository<JdmCarsBlogCommentsData>, IJdmJournalCommentRepository
    {
        public JdmJournalCommentRepository(WebContext webContext) : base(webContext)
        {

        }
        public List<JdmCarsBlogCommentsData> GetByPostId(int postId)
        {
            return _dbSet
                .Include(x => x.User)
                .Where(x => x.PostsId == postId)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }
    }
}