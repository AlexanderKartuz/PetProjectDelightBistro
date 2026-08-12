namespace FirstConsoleApp.GuessTheNumberStuff
{
    internal class RuleGenerator
    {
        public void SetRandomRule(GameRule gameRule)
        {
            var random = new Random();
            gameRule.MinValue = random.Next(1, 10);
            gameRule.MaxValue = random.Next(10, 100);
            gameRule.TheNumber = random.Next(gameRule.MinValue, gameRule.MaxValue);
            gameRule.Attempt = 13;
        }
    }
}
