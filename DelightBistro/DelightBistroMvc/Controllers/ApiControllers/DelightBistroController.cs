using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using DelightBistroMvc.Data.HelperModels.DelightBistro;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DelightBistroController : ControllerBase
    {
        private IFoodItemRepository _foodItemRepository;
        private IOrderRepository _orderRepository;
        private IAuthService _authService;
        private IHubContext<DeligtBistroHub, IDeligtBistroHub> _deligtBistroHub;

        public DelightBistroController(IFoodItemRepository foodItemRepository, IAuthService authService, IOrderRepository orderRepository, IHubContext<DeligtBistroHub, IDeligtBistroHub> deligtBistroHub)
        {
            _foodItemRepository = foodItemRepository;
            _authService = authService;
            _orderRepository = orderRepository;
            _deligtBistroHub = deligtBistroHub;
        }

        public bool Delete([FromQuery] List<int> ids)
        {
            _foodItemRepository.DeleteAsync(ids);
            return true;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateOrder([FromBody] CreateOrderDto createOrder)
        {
            //if (!_authService.IsAuthenticated())
            //{
            //    return BadRequest(new { message = "Авторизуйтесь для заказа" });
            //}

            if (createOrder.foodItemIds.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Заказ пустой" });
            }

            var selectedIds = createOrder.foodItemIds;
            var selectedFoodItems = _foodItemRepository.GetByIdsAsync(selectedIds);

            if (selectedFoodItems.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Блюда не найдены" });

            }

            if (selectedFoodItems.Count != selectedIds.Count)
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
            _orderRepository.AddAsync(orderData);

            // Ответ клиенту
            var responseDto = new
            {
                Message = "Заказ успешно создан.",
                OrderId = orderData.Id,
                CreatedTime = orderData.CreatedDateTime,
                TotalPrice = totalPrice,
            };

            return Ok(responseDto);
        }

        public void NotifyAboutFood(string name, decimal price)
        {
            _deligtBistroHub.Clients.All.NewFoodWasCreated(name, price);
        }
    }
}
