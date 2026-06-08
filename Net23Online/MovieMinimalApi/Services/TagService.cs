using Microsoft.EntityFrameworkCore;
using MovieMinimalApi.DbStuff;
using MovieMinimalApi.Dtos;

namespace MovieMinimalApi.Services
{
    public class TagService
    {
        private readonly MiniDbContext _dbContext;

        public TagService(MiniDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Normalize(string tagName) => tagName.Trim();

        public async Task<Tag> FindOrCreateTagAsync(string tagName)
        {
            var normalized = Normalize(tagName);
            if (string.IsNullOrEmpty(normalized))
            {
                throw new ArgumentException("Tag name cannot be empty.");
            }

            var existing = await _dbContext.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == normalized.ToLower());

            if (existing != null)
            {
                return existing;
            }

            var tag = new Tag { Name = normalized };
            _dbContext.Tags.Add(tag);
            return tag;
        }

        public MovieDto ToDto(Movie movie) =>
            new(
                movie.Id,
                movie.Name,
                movie.Url,
                movie.Rating,
                movie.Tags.Select(t => t.Name).OrderBy(n => n).ToList()
            );
    }
}
