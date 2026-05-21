using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        public bool AddComment(int zooId, string text)
        {
            return _commentsService.AddZooComment(zooId, text);
        }
    }
}
