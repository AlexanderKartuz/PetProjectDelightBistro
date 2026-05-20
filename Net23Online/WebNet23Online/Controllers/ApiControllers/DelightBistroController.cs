using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DelightBistroController : ControllerBase
    {
        private IFoodItemRepository _foodItemRepository;
        public DelightBistroController(IFoodItemRepository foodItemRepository)
        {
            _foodItemRepository = foodItemRepository;
        }

        public bool Delete([FromQuery] List<int> ids)
        {
            _foodItemRepository.Delete(ids);
            return true;
        }
    }
}
