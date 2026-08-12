using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsLoginFree(string login)
        {
            Thread.Sleep(1000);
            return _userRepository.IsNameUniq(login);
        }
    }
}
