namespace FirstConsoleApp.Minesweeper
{
    public class EasyMode : BaseMinesweeper
    {
        protected override void ConigurateGameRule()
        {
            _rule.FieldHeight = 9;
            _rule.FieldWidth = 9;
            _rule.NumberOfBombs = 10;
        }
    }
}
