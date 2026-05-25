using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentService;

        public CommentsController(ICommentsService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult ZooCommentsIndex(int zooId)
        {
            var viewModel = _commentService.GetZooComments(zooId);
            viewModel.HasComments = viewModel.Comments.Any();
            return View(viewModel);
        }
    }
}
