using FirstConsoleApp.BullsAndCowsGame.Interfaces;

namespace FirstConsoleApp.BullsAndCowsGame
{
    public abstract class BaseBullsAndCowsGame : IGame
    {
        protected IGameSettings Settings { get; }
        protected HiddenNumber HiddenNumber { get; private set; }
        protected int Attempts { get; private set; }

        private readonly IRandomNumberGenerator _randomNumberGenerator;

        protected BaseBullsAndCowsGame(IGameSettings gameSettings, IRandomNumberGenerator randomNumberGenerator)
        {
            Settings = gameSettings;
            _randomNumberGenerator = randomNumberGenerator;
            HiddenNumber = new HiddenNumber(gameSettings, randomNumberGenerator);
        }

        public abstract void Play();

        protected bool ProcessGuess(string guess)
        {
            try
            {
                var result = HiddenNumber.CheckGuess(guess);
                Attempts++;
                Console.WriteLine(result);

                if (result.Bulls == Settings.NumberLength)
                {
                    Console.WriteLine($"Congratulations! You've guessed the number {HiddenNumber.Value} in {Attempts} attempts!");
                    return true;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return false;
        }

        protected void Restart()
        {
            HiddenNumber = new HiddenNumber(Settings, _randomNumberGenerator);
            Attempts = 0;
            Console.WriteLine($"New game! I thought of a {Settings.NumberLength}-digit number.");
        }
    }
}