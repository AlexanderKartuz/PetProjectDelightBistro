using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.HelperModels.DelightBistro;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DelightBistroController : ControllerBase
    {
        private IFoodItemRepository _foodItemRepository;
        private IAuthService _authService;

        public DelightBistroController(IFoodItemRepository foodItemRepository, IAuthService authService)
        {
            _foodItemRepository = foodItemRepository;
            _authService = authService;
        }

        public bool Delete([FromQuery] List<int> ids)
        {
            _foodItemRepository.Delete(ids);
            return true;
        }

        public bool CreateOrder([FromBody] CreateOrderDto createOrder)
        {
            var selectedIds = createOrder.FoodItemids; // list ids
            var selectedFoodItems = _foodItemRepository.GetByIds(selectedIds);
            var totalPrice = selectedFoodItems.Sum(fi => fi.Price);

            var orederData = new OrderData()
            {
                CreatedDateTime = DateTime.Now,
                FoodItems = selectedFoodItems,
                TotalPrice = totalPrice,
                User = _authService.GetUser()
            };

            return true;
        }
    }
}
