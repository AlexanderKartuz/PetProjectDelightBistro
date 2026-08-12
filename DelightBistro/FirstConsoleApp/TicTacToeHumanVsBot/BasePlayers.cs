namespace FirstConsoleApp.TicTacToeHumanVsBot;


public abstract class BasePlayers
{
    public string Mark { get; set; }

    public BasePlayers(string mark)
    {
        Mark = mark;
    }
    public abstract void MakeMove(TicTacToeBoard board);

}