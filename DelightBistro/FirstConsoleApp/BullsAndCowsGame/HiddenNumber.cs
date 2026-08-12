using FirstConsoleApp.BullsAndCowsGame.Interfaces;

namespace FirstConsoleApp.BullsAndCowsGame
{
    public class HiddenNumber
    {
        public string Value { get; }
        public IGameSettings Settings { get; }

        public HiddenNumber(IGameSettings settings, IRandomNumberGenerator randomNumberGenerator)
        {
            Settings = settings;
            Value = randomNumberGenerator.GenerateNumber(settings);
        }

        public GuessResult CheckGuess(string guess)
        {
            ValidateGuess(guess);

            var guessResult = new GuessResult();

            guessResult.Bulls = CountBulls(Value, guess);
            guessResult.Cows = CountCows(Value, guess);

            return guessResult;
        }

        private int CountBulls(string hiddenNumber, string guessNumber)
        {
            var bulls = 0;

            for (var i = 0; i < hiddenNumber.Length; i++)
            {
                if (hiddenNumber[i] == guessNumber[i])
                {
                    bulls++;
                }
            }

            return bulls;
        }

        private int CountCows(string hiddenNumber, string guessNumber)
        {
            var cows = 0;

            for (var i = 0; i < hiddenNumber.Length; i++)
            {
                if (hiddenNumber[i] != guessNumber[i] && hiddenNumber.Contains(guessNumber[i]))
                {
                    cows++;
                }
            }

            return cows;
        }

        private void ValidateGuess(string guess)
        {
            if (guess.Length != Settings.NumberLength)
            {
                throw new ArgumentException($"Number must contain {Settings.NumberLength} digits.");
            }

            if (!guess.All(char.IsDigit))
            {
                throw new ArgumentException("Input must contain only digits.");
            }

            if (guess[0] == '0')
            {
                throw new ArgumentException("First digit cannot be zero.");
            }

            if (guess.Distinct().Count() != guess.Length)
            {
                throw new ArgumentException("Digits must not repeat.");
            }

        }
    }
}