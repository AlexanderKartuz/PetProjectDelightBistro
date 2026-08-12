namespace FirstConsoleApp.BullsAndCowsGameBySleepaidyAndYato
{
    public class BullsAndCowsGame
    {
        public int Round { get; private set; }
        protected Player _player { get; set; }
        private string _secretNumber { get; set; }
        private int _amountBulls;
        private int _amountCows;
        public BullsAndCowsGame(Player player)
        {
            this.Round = 1;
            this._player = player;
            if (_player is BotPlayer botPlayer)
            {
                botPlayer.SetGame(this);
            }
        }
        public void Play()
        {
            var isEnteredUniqueDigitsNumbers = false;
            var enteredNumberFromGameMaster = "";
            Console.WriteLine("Game Master, select a secret number!");
            do
            {
                enteredNumberFromGameMaster = GetTextFromConsole("Please enter a four-digit number with unique digits.");
                isEnteredUniqueDigitsNumbers = IsEnteredNumberCorrect(enteredNumberFromGameMaster);
            } while (!isEnteredUniqueDigitsNumbers);
            _secretNumber = enteredNumberFromGameMaster;
            do
            {
                PlayOneRound();
            } while (_amountBulls != 4);
        }

        private void PlayOneRound()
        {
            var guessedNumberFromPlayer = _player.MakeGuess();
            SearchForBullsAndCows(_secretNumber, guessedNumberFromPlayer, out _amountBulls, out _amountCows);
            _player.ProcessGuessResult(_amountBulls, _amountCows);
            Console.WriteLine($"The player guesses {guessedNumberFromPlayer}!");
            Round++;
        }

        protected string GetTextFromConsole(string text)
        {
            var textFromConsole = "";
            Console.WriteLine(text);
            textFromConsole = Console.ReadLine();
            return textFromConsole;
        }
        protected bool IsEnteredNumberCorrect(string enteredDigits)
        {
            var isPlayerEnteredCorrectDigit = false;
            if (!enteredDigits.All(char.IsDigit) || enteredDigits.Length != 4)
            {
                Console.WriteLine("Incorrect input. Please try again.");
            }
            else if (enteredDigits.Distinct().Count() != 4)
            {
                Console.WriteLine("The input contains non-unique numbers. Please try again.");
            }
            else
            {
                isPlayerEnteredCorrectDigit = true;
            }
            return isPlayerEnteredCorrectDigit;
        }
        public void SearchForBullsAndCows(string targetNumber, string candidateNumber, out int bulls, out int cows)
        {
            bulls = 0;
            cows = 0;
            for (int indexDigit = 0; indexDigit < targetNumber.Length; indexDigit++)
            {
                if (targetNumber[indexDigit] == candidateNumber[indexDigit])
                {
                    bulls++;
                }
                else if (targetNumber.Contains(candidateNumber[indexDigit]))
                {
                    cows++;
                }
            }
        }
    }
}
