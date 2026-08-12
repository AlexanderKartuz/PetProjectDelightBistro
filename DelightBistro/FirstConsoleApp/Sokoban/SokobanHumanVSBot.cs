namespace FirstConsoleApp.Sokoban
{
    public class SokobanHumanVSBot : BaseSokoban
    {
        protected override void SetupBeforeStart()
        {
            var random = new Random();
            var size = random.Next(5, 10);
            var boxes = size / 2;
            _settings = new DifficultySettings(size, boxes);
            SetupMap(size);
            SetupTargets(boxes, 1, size - 2, 'X');
            SetupTargets(boxes, 0, size - 1, 'O');
            TrySetupTarget(0, size - 1, '@');
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(_settings.Map[i, j]);
                }

                Console.WriteLine();
            }
        }

        private void SetupMap(int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _settings.Map[i, j] = '.';
                }
            }
        }

        private void SetupTargets(int boxes, int from, int to, char target)
        {
            for (int i = 0; i < boxes; i++)
            {
                TrySetupTarget(from, to, target);
            }
        }

        private void TrySetupTarget(int from, int to, char target)
        {
            var random = new Random();
            var check = true;
            do
            {
                var playerX = random.Next(from, to);
                var playerY = random.Next(from, to);
                if (_settings.Map[playerX, playerY] == '.')
                {
                    _settings.Map[playerX, playerY] = target;
                    _settings.PlayerX = playerX;
                    _settings.PlayerY = playerY;
                    check = false;
                }

            } while (check);
        }
    }
}
