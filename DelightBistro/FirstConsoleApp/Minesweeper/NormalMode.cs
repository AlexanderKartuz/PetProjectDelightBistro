namespace FirstConsoleApp.Minesweeper
{
    public class NormalMode : BaseMinesweeper
    {
        protected override void ConigurateGameRule()
        {
            _rule.FieldHeight = 16;
            _rule.FieldWidth = 16;
            _rule.NumberOfBombs = 40;
        }

    }
}
