using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        [HttpPost]
        [Authorize]
        public bool CreateOrder([FromBody] CreateOrderDto createOrder) // сделать вх параметры List id?
        {
            if (!_authService.IsAuthenticated())
            {
                return false;
            }

            if (createOrder.foodItemIds.IsNullOrEmpty())
            {
                return false;
            }

            var selectedIds = createOrder.foodItemIds; // list ids
            var selectedFoodItems = _foodItemRepository.GetByIds(selectedIds);
            if (selectedFoodItems.IsNullOrEmpty())
            {
                return false;
            }
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
