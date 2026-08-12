using MazeCore.Characters;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;
namespace MazeCore.Cells
{
    public class SecretRoom : BaseCell
    {
        private const int MAX_HEIGHT = 5;
        private const int MAX_WIDTH = 5;
        public IMazeController secretRoomConroller = new MazeController();

        public IMaze SecretMaze { get; set; }

        public SecretRoom(IMaze maze) : base(maze)
        {
            SecretMaze = new Maze();
            SecretMaze.IsSecretMaze = true;
        }

        public override char Symbol { get; } = 'R';
        public override bool Interaction(IBaseCharacter character)
        {
            Maze.EventHistory.Add("You find secret room!");
            Replace();

            secretRoomConroller.Play(MAX_HEIGHT, MAX_WIDTH, SecretMaze.IsSecretMaze, hero: (Hero)character);

            return true;
        }

        private void Replace()
        {
            Maze.Surface.Remove(this);
            var ground = new Ground(Maze)
            {
                X = X,
                Y = Y,
            };
            Maze.Surface.Add(ground);
        }
    }
}
