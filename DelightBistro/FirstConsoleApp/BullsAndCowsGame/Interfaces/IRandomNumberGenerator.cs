namespace FirstConsoleApp.BullsAndCowsGame.Interfaces
{
    public interface IRandomNumberGenerator
    {
        string GenerateNumber(IGameSettings settings);
    }
}