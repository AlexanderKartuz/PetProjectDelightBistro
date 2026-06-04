using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Hubs;
using WebNet23Online.Hubs.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JdmController : ControllerBase
    {
        private IJdmRepository _jdmRepository;
        private IHubContext<JdmHub, IJdmHub> _jdmHub;

        public JdmController(IJdmRepository jdmRepository, IHubContext<JdmHub, IJdmHub> jdmHub)
        {
            _jdmRepository = jdmRepository;
            _jdmHub = jdmHub;
        }

        public void NotifyAboutJdmCars(string model, int price, string url)
        {
            _jdmHub.Clients.All.NewJdmCarsCreated(model, price, url);
        }
    }
}