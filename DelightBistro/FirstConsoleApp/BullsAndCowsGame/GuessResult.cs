
namespace FirstConsoleApp.BullsAndCowsGame
{
    public class GuessResult
    {
        public int Bulls { get; set; }
        public int Cows { get; set; }

        public override string ToString()
        {
            return $"Bulls: {Bulls}, Cows: {Cows}";
        }
    }
}
