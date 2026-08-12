namespace FirstConsoleApp.Minesweeper
{
    public class HardMode : BaseMinesweeper
    {
        protected override void ConigurateGameRule()
        {
            _rule.FieldHeight = 30;
            _rule.FieldWidth = 16;
            _rule.NumberOfBombs = 99;
        }
    }
}
