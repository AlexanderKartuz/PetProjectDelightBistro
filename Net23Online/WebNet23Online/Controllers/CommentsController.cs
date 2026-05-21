using Microsoft.AspNetCore.Mvc;

namespace WebNet23Online.Controllers
{
    public class CommentsController : Controller
    {
        public IActionResult ZooCommentsIndex()
        {
            return View();
        }
    }
}
