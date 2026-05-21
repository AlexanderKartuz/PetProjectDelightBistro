using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        public CommentsController()
        {
            
        }

        public bool AddComment()
        {

            return true;
        }
    }
}
