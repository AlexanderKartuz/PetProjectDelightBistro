namespace FirstConsoleApp.BullsAndCowsGame
{
    public class GameSettingsFactory
    {
        public GameSettings ChooseGameSettings()
        {
            while (true)
            {
                Console.WriteLine("Choose difficulty:");
                Console.WriteLine("1 - Easy (3 digits)");
                Console.WriteLine("2 - Medium (4 digits)");
                Console.WriteLine("3 - Hard (5 digits)");

                var choice = Console.ReadLine();

                try
                {
                    var settings = CreateDifficultyForGame(choice);
                    Console.WriteLine($"You've chosen {settings.NumberLength}-digits difficulty");
                    return settings;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}. Try again.");
                }
            }
        }

        private GameSettings CreateDifficultyForGame(string difficulty)
        {
            return difficulty switch
            {
                "1" => new GameSettings(100, 999),
                "2" => new GameSettings(1000, 9999),
                "3" => new GameSettings(10000, 99999),
                _ => throw new ArgumentException("Wrong key. Choose 1, 2 or 3.")
            };
        }
    }
}
