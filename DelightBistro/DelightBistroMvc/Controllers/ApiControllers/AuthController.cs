using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Services.UserService;

namespace DelightBistroMvc.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserDataService _userDataService;

        public AuthController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        [HttpGet]
        public bool IsLoginFree(string login)
        {
            //Thread.Sleep(1000);
            return _userDataService.IsNameUniq(login);
        }
    }
}
