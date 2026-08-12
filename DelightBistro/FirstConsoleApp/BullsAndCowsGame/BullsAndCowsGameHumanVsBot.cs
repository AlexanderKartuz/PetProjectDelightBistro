using FirstConsoleApp.BullsAndCowsGame.Interfaces;

namespace FirstConsoleApp.BullsAndCowsGame
{
    public class BullsAndCowsGameHumanVsBot : BaseBullsAndCowsGame, IGame
    {
        public BullsAndCowsGameHumanVsBot(IGameSettings gameSettings, IRandomNumberGenerator randomNumberGenerator) : base(gameSettings, randomNumberGenerator)
        {
            
        }

        public override void Play()
        {
            ShowWelcomeMessage();
            var playing = true;

            while (playing)
            {
                Console.Write($"\nAttempt {Attempts + 1}. Enter your guess: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                var lowerInput = input.ToLower();
                if (lowerInput == "exit")
                {
                    Console.WriteLine($"Goodbye! I thought of the number {HiddenNumber.Value}");
                    break;
                }

                if (lowerInput == "restart")
                {
                    Restart();
                    continue;
                }

                if (ProcessGuess(input))
                {
                    playing = AskUserForRematch();
                }
            }

            Console.WriteLine("Thanks for the game!");
        }

        private void ShowWelcomeMessage()
        {   
            Console.WriteLine("\nBulls and Cows: human vs bot");
            Console.WriteLine($"I've guessed a {Settings.NumberLength}-digit number.");
            Console.WriteLine("Commands:");
            Console.WriteLine("'exit'- quit the game");
            Console.WriteLine("'restart'- start a new game");
        }

        private bool AskUserForRematch()
        {
            while (true)
            {
                Console.Write("Want to play again? (yes/no): ");
                var answer = Console.ReadLine().ToLower();

                if (answer == "yes")
                {
                    Restart();
                    return true;
                }
                else if (answer == "no")
                {
                    return false;
                }

                Console.WriteLine("Please enter 'yes' or 'no'");
            }
        }
    }
}