using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddComment([FromForm] int entityId, [FromForm] string newCommentText)
        {
            if (string.IsNullOrWhiteSpace(newCommentText))
            {
                return BadRequest();
            }

            var comment = _commentsService.AddZooComment(entityId, newCommentText.Trim());
            if (comment == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                author = comment.Author,
                text = comment.Text,
                createdAt = comment.CreatedAt.ToString("g"),
            });
        }
    }
}
