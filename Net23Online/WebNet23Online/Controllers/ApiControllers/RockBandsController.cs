using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RockBandsController : ControllerBase
    {
        private readonly IRockBandsRepository _rockBandsRepository;

        public RockBandsController(IRockBandsRepository rockBandsRepository)
        {
            _rockBandsRepository = rockBandsRepository;
        }

        public bool IsBandNameFree(string name)
        {
            Thread.Sleep(1000);
            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }

            return !_rockBandsRepository.IsBandNameTaken(name.Trim());
        }
    }
}
