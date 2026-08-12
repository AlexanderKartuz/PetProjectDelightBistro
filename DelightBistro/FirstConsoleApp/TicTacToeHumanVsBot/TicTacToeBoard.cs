
namespace FirstConsoleApp.TicTacToeHumanVsBot
{
    public class TicTacToeBoard
    {
        public string Cell1 { get; set; } = "1";
        public string Cell2 { get; set; } = "2";
        public string Cell3 { get; set; } = "3";
        public string Cell4 { get; set; } = "4";
        public string Cell5 { get; set; } = "5";
        public string Cell6 { get; set; } = "6";
        public string Cell7 { get; set; } = "7";
        public string Cell8 { get; set; } = "8";
        public string Cell9 { get; set; } = "9";
        public int MoveCount { get; set; } = 0;

        public void Draw()
        {
            Console.WriteLine($" {Cell1} | {Cell2} | {Cell3} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {Cell4} | {Cell5} | {Cell6} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {Cell7} | {Cell8} | {Cell9} ");
        }
        public void Clear()
        {
            Cell1 = "1";
            Cell2 = "2";
            Cell3 = "3";
            Cell4 = "4";
            Cell5 = "5";
            Cell6 = "6";
            Cell7 = "7";
            Cell8 = "8";
            Cell9 = "9";
            MoveCount = 0;
        }
        public bool TryMakeMove(int position, string playerMark)
        {
            playerMark = playerMark.ToUpper();
            if (position == 1 && Cell1 == "1")
            {
                Cell1 = playerMark;
                return true;
            }
            if (position == 2 && Cell2 == "2")
            {
                Cell2 = playerMark;
                return true;
            }
            if (position == 3 && Cell3 == "3")
            {
                Cell3 = playerMark;
                return true;
            }
            if (position == 4 && Cell4 == "4")
            {
                Cell4 = playerMark;
                return true;
            }
            if (position == 5 && Cell5 == "5")
            {
                Cell5 = playerMark;
                return true;
            }
            if (position == 6 && Cell6 == "6")
            {
                Cell6 = playerMark;
                return true;
            }
            if (position == 7 && Cell7 == "7")
            {
                Cell7 = playerMark;
                return true;
            }
            if (position == 8 && Cell8 == "8")
            {
                Cell8 = playerMark;
                return true;
            }
            if (position == 9 && Cell9 == "9")
            {
                Cell9 = playerMark;
                return true;
            }
            return false;
        }
    

    }

}

