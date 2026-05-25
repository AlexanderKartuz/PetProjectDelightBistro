using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/rock-legends")]
    public class RockLegendsApiController : ControllerBase
    {
        private readonly IRockLegendsRepository _rockLegendsRepository;
        private readonly IRockLegendsGenresRepository _genreRepository;

        public RockLegendsApiController(
            IRockLegendsRepository rockLegendsRepository,
            IRockLegendsGenresRepository genreRepository)
        {
            _rockLegendsRepository = rockLegendsRepository;
            _genreRepository = genreRepository;
        }

        [HttpPost("like/{id}")]
        [Authorize] 
        public IActionResult LikeBand(int id)
        {
            var hasVoted = HttpContext.Session.GetString("HasVotedInRockPoll");
            if (hasVoted == "true")
            {
                return BadRequest(new { success = false, message = "Вы уже отдавали свой голос! Рок-н-ролл за честные выборы 🤘" });
            }

            var targetBand = _rockLegendsRepository.GetById(id);
            if (targetBand == null) return NotFound(new { message = "Группа не найдена" });

            targetBand.Likes++;
            _rockLegendsRepository.Update(targetBand);

            HttpContext.Session.SetString("HasVotedInRockPoll", "true");

            return Ok(new { success = true, newLikes = targetBand.Likes });
        }

        [HttpGet("validate-genre")]
        public IActionResult ValidateGenreName(string name)
        {
            if (string.IsNullOrEmpty(name)) return Ok(new { isValid = true });

            var allGenres = _genreRepository.GetAll();
            bool exists = allGenres.Any(g => g.Name.Trim().ToLower() == name.Trim().ToLower());

            if (exists)
            {
                return Ok(new { isValid = false, message = "Такой жанр уже гордо существует в нашей базе!" });
            }

            return Ok(new { isValid = true });
        }
    }
}