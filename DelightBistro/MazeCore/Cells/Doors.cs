using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Doors : BaseCell
    {
        public const char SYMBOL = 'D';
        public string DoorType { get; set; }
        private const int _COIN_COST = 1;

        public override bool IsBonusCell { get; init; } = true;

        public Doors(IMaze maze) : base(maze)
        {
            
        }

        public override char Symbol => SYMBOL;
        public override bool Interaction(IBaseCharacter character)
        {
            var minChoices = 1;
            var maxChoices = 3;
           
            Console.WriteLine($"\n  Door is locked!");
            Console.WriteLine("Choose how to open it:");
            Console.WriteLine("1. Use Key");
            Console.WriteLine($"2. Pay {_COIN_COST} coins");
            Console.WriteLine("3. Cancle");

            while (true)
            {
                var choice = ReadMenuChoice(minChoices, maxChoices);
                if (choice == maxChoices)
                {
                    return false;
                }
                var isOpenDoor = choice switch
                {
                    1 => TryOpenWithKey(character, _COIN_COST),
                    2 => TryOpenWithCoins(character, _COIN_COST),
                    _ => false
                };
                if (!isOpenDoor)
                {
                    continue;
                }

                return isOpenDoor;
            }

        }
        private bool TryOpenWithKey(IBaseCharacter character, int cost)
        {
            if (!character.HasKey(cost))
            {
                Console.WriteLine(" You don't have a key!");
                return false;
            }

            character.UseKey(cost);
            Open();
            Maze.EventHistory.Add(" Door unlocked with key!");
            return true;
        }

        private bool TryOpenWithCoins(IBaseCharacter character, int cost)
        {
            if (character.Coins < cost)
            {
                Console.WriteLine($" Not enough coins! Need {cost}, you have {character.Coins}");
                return false;
            }

            character.SpendCoins(cost);
            Open();
            Maze.EventHistory.Add($" Paid {cost} coins. Door is now open.");
            return true;
        }

        private int ReadMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.Write($"Your choice ({min}-{max}): ");
                var input = Maze.InputReader.ReadLine();

                if (int.TryParse(input, out var choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine("Invalid choice. Try again.");
            }

        }
        private void Open()
        {
            MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
            soundPlayer.PlayMusic("door_sound.mp3", 0.7f);

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