namespace FirstConsoleApp.BullsAndCowsGameBySleepaidyAndYato
{
    public abstract class Player
    {
        public Player()
        {
           
        }
        public abstract string MakeGuess();

        public abstract void ProcessGuessResult(int Bulls, int Cows);
    }
}
