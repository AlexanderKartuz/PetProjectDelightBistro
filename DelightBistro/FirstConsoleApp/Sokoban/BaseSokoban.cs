namespace FirstConsoleApp.Sokoban
{
    public abstract class BaseSokoban
    {
        protected DifficultySettings _settings;

        public void Play()
        {
            SetupBeforeStart();
            StartTheGame();
        }

        protected abstract void SetupBeforeStart();

        private void StartTheGame()
        {
            var standInGoal = false;
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                var dx = 0;
                var dy = 0;
                if (key == ConsoleKey.UpArrow)
                {
                    dx = -1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    dx = 1;
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    dy = -1;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    dy = 1;
                }
                else
                {
                    continue;
                }

                var goalX = _settings.PlayerX + dx;
                var goalY = _settings.PlayerY + dy;
                if (goalX >= _settings.Size || goalY >= _settings.Size || goalX < 0 || goalY < 0 || _settings.Map[goalX, goalY] == '*')
                {
                    continue;
                }
                
                if (_settings.Map[goalX, goalY] == '.' || _settings.Map[goalX, goalY] == 'O')
                {
                    if (standInGoal)
                    {
                        _settings.Map[_settings.PlayerX, _settings.PlayerY] = 'O';
                        standInGoal = false;
                    }
                    else
                    {
                        _settings.Map[_settings.PlayerX, _settings.PlayerY] = '.';
                    }

                    if (_settings.Map[goalX, goalY] == 'O')
                    {
                        standInGoal = true;
                    }

                    _settings.Map[goalX, goalY] = '@';

                }
                else if (_settings.Map[goalX, goalY] == 'X')
                {
                    var goalBoxX = goalX + dx;
                    var goalBoxY = goalY + dy;
                    if (goalBoxX >= _settings.Size || goalBoxY >= _settings.Size || goalBoxX < 0 || goalBoxY < 0)
                    {
                        continue;
                    }

                    if (_settings.Map[goalBoxX, goalBoxY] == '.')
                    {
                        _settings.Map[goalBoxX, goalBoxY] = 'X';
                        _settings.Map[goalX, goalY] = '@';
                        _settings.Map[_settings.PlayerX, _settings.PlayerY] = '.';
                    }
                    else if (_settings.Map[goalBoxX, goalBoxY] == 'O')
                    {
                        _settings.Map[goalBoxX, goalBoxY] = '*';
                        _settings.Map[goalX, goalY] = '@';
                        _settings.Map[_settings.PlayerX, _settings.PlayerY] = '.';
                    }
                }

                _settings.PlayerX = goalX;
                _settings.PlayerY = goalY;
                Console.Clear();
                var victory = VictoryCheck();
                if (victory)
                {
                    Console.WriteLine("You are win!");
                    return;
                }
            }
        }

        private bool VictoryCheck()
        {
            var victory = true;
            for (int i = 0; i < _settings.Size; i++)
            {
                for (int j = 0; j < _settings.Size; j++)
                {
                    if (_settings.Map[i, j] == 'X')
                    {
                        victory = false;
                    }

                    Console.Write(_settings.Map[i, j]);
                }

                Console.WriteLine();
            }

            return victory;
        }
    }
}
