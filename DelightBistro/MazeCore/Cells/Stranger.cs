using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Stranger : BaseCell
    {
        private Random _random;
        private double[] _weights = { 100.0, 100.0, 100.0 };
        public const double MIN_WEIGHT = 10.0;
        public const double FACTOR = 0.3;

        public Stranger(IMaze maze, Random random) : base(maze)
        {
            _random = random;
        }

        public override char Symbol => '.';

        public override bool Interaction(IBaseCharacter character)
        {
            Maze.EventHistory.Add("Suddenly appeared some Stranger.");
            Maze.EventHistory.Add("He is doing something to you.");
            string action = WeightedRandomSelection(character);
            Maze.EventHistory.Add(action);
            return true;
        }

        private string WeightedRandomSelection(IBaseCharacter character)
        {
            var _weightedIndex = GetWeightedIndex();
            var reduction = _weights[_weightedIndex] * FACTOR;
            if (_weights[_weightedIndex] - reduction < MIN_WEIGHT)
            {
                reduction = _weights[_weightedIndex] - MIN_WEIGHT;
            }

            _weights[_weightedIndex] -= reduction;
            var bonus = reduction / (_weights.Length - 1);
            for (int i = 0; i < _weights.Length; i++)
            {
                if (i != _weightedIndex)
                {
                    _weights[i] += bonus;
                }
            }

            switch (_weightedIndex)
            {
                case 0:
                    {
                        character.Coins++;
                        return "He gave you 1 coin.";
                    }
                case 1:
                    {
                        character.Hp--;
                        return "He hit you.";
                    }
                default:
                    {
                        character.Hp++;
                        return "He heal you";
                    }
            }
        }

        private int GetWeightedIndex()
        {
            var totalWeight = _weights.Sum();
            var randomPoint = _random.NextDouble() * totalWeight;
            var currentSum = 0.00;
            for (int i = 0; i < _weights.Length; i++)
            {
                currentSum += _weights[i];
                if (randomPoint <= currentSum)
                {
                    return i;
                }
            }

            return _weights.Length - 1;
        }
    }
}
