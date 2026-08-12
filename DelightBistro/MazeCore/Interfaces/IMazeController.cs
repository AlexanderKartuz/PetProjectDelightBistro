using MazeCore.Characters;

namespace MazeCore.Interfaces
{
    public interface IMazeController
    {
        void Play(int width = 24, int height = 12, bool isSecretMaze = false, Hero hero = null);
    }
}