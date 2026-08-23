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
using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DelightBistroController : ControllerBase
    {
        private readonly IFoodItemRepository _foodItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<DeligtBistroHub, IDeligtBistroHub> _deligtBistroHub;

        public DelightBistroController(IFoodItemRepository foodItemRepository, IAuthService authService, IOrderRepository orderRepository, IHubContext<DeligtBistroHub, IDeligtBistroHub> deligtBistroHub, IUnitOfWork unitOfWork)
        {
            _foodItemRepository = foodItemRepository;
            _authService = authService;
            _orderRepository = orderRepository;
            _deligtBistroHub = deligtBistroHub;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteAsync([FromQuery] List<int> ids,
            CancellationToken cancellationToken = default)
        {
            await _foodItemRepository.DeleteAsync(ids, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto createOrder,
            CancellationToken cancellationToken = default)
        {
            if (createOrder.foodItemIds.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Заказ пустой" });
            }

            var selectedIds = createOrder.foodItemIds;
            var selectedFoodItems = await _foodItemRepository.GetByIdsAsync(selectedIds, cancellationToken);

            if (selectedFoodItems.IsNullOrEmpty())
            {
                return BadRequest(new { message = "Блюда не найдены" });

            }

            if (selectedFoodItems.Count != selectedIds.Count)
            {
                return BadRequest(new { message = "Блюда не найдены" });
            }

            var totalPrice = selectedFoodItems.Sum(fi => fi.Price);
            var user = await _authService.GetUserAsync(cancellationToken);

            var orderData = new OrderData()
            {
                CreatedDateTime = DateTime.UtcNow,
                FoodItems = selectedFoodItems,
                TotalPrice = totalPrice,
                User = user,
            };
            await _orderRepository.AddAsync(orderData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
