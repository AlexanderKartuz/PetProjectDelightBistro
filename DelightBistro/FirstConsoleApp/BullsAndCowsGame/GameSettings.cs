using FirstConsoleApp.BullsAndCowsGame.Interfaces;

namespace FirstConsoleApp.BullsAndCowsGame
{
    public class GameSettings : IGameSettings
    {
        public int MinValue { get; }
        public int MaxValue { get; }
        public int NumberLength { get; }

        public GameSettings(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentException("MinValue must be less than MaxValue");
            }

            MinValue = minValue;
            MaxValue = maxValue;
            NumberLength = minValue.ToString().Length;
        }
    }
}