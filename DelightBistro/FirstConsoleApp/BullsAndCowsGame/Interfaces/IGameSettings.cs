namespace FirstConsoleApp.BullsAndCowsGame.Interfaces
{
    public interface IGameSettings
    {
        int MaxValue { get; }
        int MinValue { get; }
        int NumberLength { get; }
    }
}