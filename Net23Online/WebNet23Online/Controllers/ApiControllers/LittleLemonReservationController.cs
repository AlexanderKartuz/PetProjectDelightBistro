using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Services.Interfaces.LittleLemon;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LittleLemonReservationController : ControllerBase
    {
        private ILittleLemonReservationService _littleLemonReservationService;

        public LittleLemonReservationController(ILittleLemonReservationService littleLemonReservationService)
        {
            _littleLemonReservationService = littleLemonReservationService;
        }

        public bool HasDuplicate([FromQuery] string date, [FromQuery] string time, [FromQuery] string seatingPreference)
        {
            return _littleLemonReservationService.HasReservationAtDateTime(date, time, seatingPreference);
        }
    }
}
