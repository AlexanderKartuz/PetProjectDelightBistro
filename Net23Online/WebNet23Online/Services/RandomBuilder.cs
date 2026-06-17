using WebNet23Online.RelfectionTools;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    [AutoRegister]
    public class RandomBuilder : IRandomBuilder
    {
        private Random _random;
        public Random GetRandom()
        {
            if (_random is null)
            {
                _random = new Random();
            }

            return _random;
        }
    }
}
