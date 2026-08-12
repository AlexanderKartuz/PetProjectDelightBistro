
namespace FirstConsoleApp.TicTacToeHumanVsBot
{
    internal class BotPlayer : BasePlayers
    {
        private Random _random = new();

        public BotPlayer(string mark) : base(mark)
        {
            _random = new Random();
        }
        public override void MakeMove(TicTacToeBoard board)
        {

            while (true)
            {
                var botChoice = _random.Next(1, 10);
                if (board.Cell5 == "5")
                {
                    board.Cell5 = Mark;
                    break;
                }
                else
                {

                    if (!board.TryMakeMove(botChoice, Mark))
                    {
                        continue;
                    }
                    break;
                }
            }
        }
    }
}
