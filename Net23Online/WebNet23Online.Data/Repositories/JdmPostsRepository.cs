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
    public class JdmPostsRepository : BaseRepository<JdmPostsData>, IJdmPostsRepository
    {
        private NotificationRepository _notificationRepository;

        public JdmPostsRepository(WebContext webContext, NotificationRepository notificationRepository) : base(webContext)
        {
            _notificationRepository = notificationRepository;
        }
        public List<JdmPostsData> GetPublishedPosts()
        {
            return _dbSet
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();
        }

        public List<JdmPostsData> GetByPostId(int postId)
        {
            return _dbSet
                .Include(x => x.Title)
                .Include(x => x.Text)
                .Include(x => x.Id == postId)
                .OrderByDescending(x => x.PublishedDate)
                .ToList();
        }

        public List<JdmPostsData> GetByOldPosts(DateTime oldTimePublished)
        {
            oldTimePublished = DateTime.Now.AddDays(-45);
            return _dbSet
                .Where(x => x.PublishedDate < oldTimePublished)
                .ToList();
        }

        public int DeleteOldPosts(DateTime oldTimePublished)
        {
            var oldPosts = GetByOldPosts(oldTimePublished);
            if (!oldPosts.Any())
            {
                return 0;
            }
                _dbSet.RemoveRange(oldPosts);
                _context.SaveChanges();
            return oldPosts.Count;
        }
    }
}
