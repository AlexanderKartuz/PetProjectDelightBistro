namespace MazeCore.Cells.Interfaces;

public interface IInputReader
{
    ConsoleKey ReadKey();
    string ReadLine();
}