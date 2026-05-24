using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebNet23Online.Data.HelperModels.DelightBistro;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DelightBistroController : ControllerBase
    {
        private IFoodItemRepository _foodItemRepository;
        private IOrderRepository _orderRepository;
        private IAuthService _authService;

        public DelightBistroController(IFoodItemRepository foodItemRepository, IAuthService authService, IOrderRepository orderRepository)
        {
            _foodItemRepository = foodItemRepository;
            _authService = authService;
            _orderRepository = orderRepository;
        }

        public bool Delete([FromQuery] List<int> ids)
        {
            _foodItemRepository.Delete(ids);
            return true;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateOrder([FromBody] CreateOrderDto createOrder) // принимать параметры List id?
        {
            if (!_authService.IsAuthenticated())
            {
                return BadRequest(new { message = "Заказ пустой" });
            }

            if (createOrder.foodItemIds.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Заказ пустой" });
            }

            var selectedIds = createOrder.foodItemIds; // list ids
            var selectedFoodItems = _foodItemRepository.GetByIds(selectedIds);
            if (selectedFoodItems.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Блюда не найдены" });
            }
            var totalPrice = selectedFoodItems.Sum(fi => fi.Price);

            var orderData = new OrderData()
            {
                CreatedDateTime = DateTime.UtcNow,
                FoodItems = selectedFoodItems,
                TotalPrice = totalPrice,
                User = _authService.GetUser()
            };
            _orderRepository.Add(orderData);

            // Ответ клиенту
            var responseDto = new
            {
                Message = "Заказ успешно создан.",
                OrderId= orderData.Id,
                CreatedTime =orderData.CreatedDateTime,
                TotalPrice = totalPrice,
            };

            return Ok(responseDto);
        }
    }
}
