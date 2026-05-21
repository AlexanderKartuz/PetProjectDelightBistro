using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class CommentsController : Controller
    {
        private ICommentsService _commentService;

        public CommentsController(ICommentsService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult ZooCommentsIndex(int zooId)
        {
            return View(_commentService.GetZooComments(zooId));
        }
    }
}
