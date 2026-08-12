namespace FirstConsoleApp.GuessTheNumberStuff
{
    public class GuessTheNumberGameHumanVsBot : BaseGuessTheNumberGame
    {
        protected override void ConigurateGameRule()
        {
            var ruleGenerator = new RuleGenerator();
            ruleGenerator.SetRandomRule(_rule);
        }
    }
}
