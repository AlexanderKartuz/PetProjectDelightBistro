namespace FirstConsoleApp.TicTacToeHumanVsBot;

public class HumanPlayer : BasePlayers
{
    public HumanPlayer(string mark) : base(mark)
    {
    }
    public override void MakeMove(TicTacToeBoard board)
    {
        while (true)
        {
            Console.WriteLine("Your move:");
            if (!int.TryParse(Console.ReadLine(), out int playerChoice))
            {
                Console.WriteLine("Error! you should to choose number from 1 - 9!");
                continue;
            }
            if (!board.TryMakeMove(playerChoice, Mark))
            {
                Console.WriteLine("Busy Cell!");
                continue;
            }
            break;
        }
    }
}