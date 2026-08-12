using MazeCore.Cells;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Characters
{
    public abstract class BaseCharacter : BaseCell, IBaseCharacter
    {
        protected BaseCharacter(IMaze maze) : base(maze)
        {
        }

        public string Name { get; set; }
        public int Hp { get; set; }
        public int Coins { get; set; }
        public int Speed { get; set; }
        public int Burning { get; set; }
        public int EnemiesKilled { get; set; }
        public int Keys { get; set; }
        public int SuperPower { get; set; } 

        public bool HasKey(int amount)
        {
            if (Keys >= amount)
            {
                return true;
            }
            return false;
        }

        public void UseKey(int amount)
        {
            Keys -= amount;
        }

        public void SpendCoins(int amount)
        {
            Coins -= amount;
        }

        public void CollectKey(int amount)
        {
            Keys += amount;
        }

        public void ProcessBurnEffect()
        {
            if (Burning < 0)
            {
                return;
            }

            Hp--;
            Maze.EventHistory.Add($"You've lost 1 HP from burning. Your HP: {Hp}");

            Burning--;

            if (Burning == 0)
            {
                Maze.EventHistory.Add($"You put out the fire");
            }
        }
    }
}
