using MazeCore.Cells.Interfaces;

namespace MazeCore.Cells.ConsoleInputReader;

public class ConsoleInputReader : IInputReader
{
    public ConsoleKey ReadKey()
    {
        return Console.ReadKey(true).Key;
    }

    public string ReadLine()
    {
        return Console.ReadLine();
    }
}