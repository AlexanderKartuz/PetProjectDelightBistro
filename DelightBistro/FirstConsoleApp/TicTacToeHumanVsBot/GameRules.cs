
namespace FirstConsoleApp.TicTacToeHumanVsBot;

public class GameRules
{
    private bool CheckWins(TicTacToeBoard board, string mark)
    {
        if (board.Cell1 == mark && board.Cell2 == mark && board.Cell3 == mark)
        {
            return true;
        }
        if (board.Cell4 == mark && board.Cell5 == mark && board.Cell6 == mark)
        {
            return true;
        }
        if (board.Cell7 == mark && board.Cell8 == mark && board.Cell9 == mark)
        {
            return true;
        }
        if (board.Cell1 == mark && board.Cell4 == mark && board.Cell7 == mark)
        {
            return true;
        }
        if (board.Cell2 == mark && board.Cell5 == mark && board.Cell8 == mark)
        {
            return true;
        }
        if (board.Cell3 == mark && board.Cell6 == mark && board.Cell9 == mark)
        {
            return true;
        }
        if (board.Cell1 == mark && board.Cell5 == mark && board.Cell9 == mark)
        {
            return true;
        }
        if (board.Cell3 == mark && board.Cell5 == mark && board.Cell7 == mark)
        {
            return true;
        }
        return false;
    }
    public bool IsFull(TicTacToeBoard board)
    {
        if (board.MoveCount >= 9)
        {
            return true;
        }
        return false;
    }
    public string GetGameResult(TicTacToeBoard board, string player1Mark, string player2Mark)
    {
        bool player1Wins = CheckWins(board, player1Mark);
        bool player2Wins = CheckWins(board, player2Mark);
        bool isBoardFull = IsFull(board);

        if (player1Wins)
        {
            return "player Wins";
        }

        if (player2Wins)
        {
            return "Bot Wins";
        }

        if (isBoardFull)
        {
            return "No winners";
        }

        return "";
    }
}