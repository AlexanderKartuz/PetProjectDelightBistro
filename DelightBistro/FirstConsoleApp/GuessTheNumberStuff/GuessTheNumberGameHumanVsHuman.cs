namespace FirstConsoleApp.GuessTheNumberStuff
{
    public class GuessTheNumberGameHumanVsHuman : BaseGuessTheNumberGame
    {
        protected override void ConigurateGameRule()
        {
            _rule = new GameRule();

            var random = new Random();

            _rule.MinValue = GetNumberFromConsole("Enter a min value");

            _rule.MaxValue = GetNumberFromConsole("Enter a max value");

            _rule.TheNumber = random.Next(1, 10); // [1, 10]

            _rule.Attempt = 0;

            _rule.MaxAttempt = CalculateMaxAttempt();
        }
    }
}
