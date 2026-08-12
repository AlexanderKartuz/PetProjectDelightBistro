namespace FirstConsoleApp.GuessTheNumberStuff
{
    public abstract class BaseGuessTheNumberGame
    {
        protected GameRule _rule = new();
        private bool _isUserAreWinner = false;

        /// <summary>
        /// This is a main method to play The Guess number
        /// </summary>
        public void Play()
        {
            ConigurateGameRule();

            PlayOneRound();

            SayToUserTheGameResult();
        }

        private void SayToUserTheGameResult()
        {
            if (_isUserAreWinner)
            {
                Console.WriteLine("You are winner");
            }
            else
            {
                Console.WriteLine("Looser");
            }

            Console.WriteLine("The end");
        }

        private void PlayOneRound()
        {
            do
            {
                _rule.Attempt++;
                var guess = GetNumberFromConsole($"Enter your guess in [{_rule.MinValue}/{_rule.MaxValue}]");

                if (guess < _rule.TheNumber)
                {
                    Console.WriteLine("My number is bigger");
                }
                if (guess > _rule.TheNumber)
                {
                    Console.WriteLine("My number is less");
                }
                if (guess == _rule.TheNumber)
                {
                    Console.WriteLine("You are win");
                }

                _isUserAreWinner = guess == _rule.TheNumber;
            } while (!_isUserAreWinner); // isGuessAreRight == false
        }

        protected abstract void ConigurateGameRule();

        /// <summary>
        /// We just get count of number a dived it for 4
        /// </summary>
        /// <returns></returns>
        protected int CalculateMaxAttempt()
        {
            var countOfNumbers = _rule.MaxValue - _rule.MinValue;
            return countOfNumbers / 4;
        }

        protected int GetNumberFromConsole(string messageForUser)
        {
            var isThisANumberForMax = false;
            var number = 0;
            do
            {
                Console.WriteLine(messageForUser);
                var guessStr = Console.ReadLine();

                isThisANumberForMax = int.TryParse(guessStr, out number);
                if (!isThisANumberForMax)
                {
                    Console.WriteLine("It's not a number. Please enter just a number");
                }

            } while (!isThisANumberForMax);

            return number;
        }
    }
}
