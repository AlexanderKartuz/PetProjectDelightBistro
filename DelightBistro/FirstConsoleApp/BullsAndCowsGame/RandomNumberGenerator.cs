using FirstConsoleApp.BullsAndCowsGame.Interfaces;

namespace FirstConsoleApp.BullsAndCowsGame
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random;

        public RandomNumberGenerator()
        {
            _random = new Random();
        }

        public string GenerateNumber(IGameSettings settings)
        {
            string number;
            do
            {
                number = _random.Next(settings.MinValue, settings.MaxValue + 1).ToString();
            } while (HasDuplicateDigits(number));

            return number;
        }

        private bool HasDuplicateDigits(string number)
        {
            var hasDuplicates = number.Length != number.Distinct().Count();
            return hasDuplicates;
        }
    }
}
