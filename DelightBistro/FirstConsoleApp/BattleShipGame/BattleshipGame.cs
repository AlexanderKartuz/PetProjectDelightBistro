namespace FirstConsoleApp.BattleShipGame
{
    public class BattleshipGame
    {
        public void Play()
        {
            var gameRule = new GameRule();
            gameRule.NumberOfRowsInGameField = GetNumberFromConsole("Insert size of game field");
            gameRule.NumberOfColumnsInGameField = gameRule.NumberOfRowsInGameField;
            gameRule.Attempt = 0;

            gameRule.FirstPlayerGameField = 
                new int[gameRule.NumberOfRowsInGameField, gameRule.NumberOfColumnsInGameField];
            gameRule.SecondPlayerGameField =
                new int[gameRule.NumberOfRowsInGameField, gameRule.NumberOfColumnsInGameField];

            gameRule.CurrentMatrix = gameRule.FirstPlayerGameField;

            PlaceTheBattleship(gameRule);
            SwitchMatrix(gameRule);
            PlaceTheBattleship(gameRule);
            PlayBattleship(gameRule);


        }
        public void PlayBattleship(GameRule gameRule)
        {
            bool HasTargets(int[,] currentMatrix)
            {
                for (int i = 0; i < currentMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < currentMatrix.GetLength(1); j++)
                    {
                        if (currentMatrix[i, j] == 1)
                            return true;
                    }
                }
                return false;
            }

            while (HasTargets(gameRule.FirstPlayerGameField) && HasTargets(gameRule.SecondPlayerGameField))
            {
                string currentPlayer;

                if (gameRule.Attempt % 2 == 0)
                {
                    currentPlayer = "Player 1";
                    gameRule.CurrentMatrix = gameRule.SecondPlayerGameField;
                    PrintMatrix(gameRule.FirstPlayerGameField);
                }
                else
                {
                    currentPlayer = "Player 2";
                    gameRule.CurrentMatrix = gameRule.FirstPlayerGameField;
                    PrintMatrix(gameRule.SecondPlayerGameField);
                }

                Console.WriteLine($"{currentPlayer} turn");

                Console.WriteLine("Choose row for attack: ");
                int hitRow = int.Parse(Console.ReadLine()) - 1;

                Console.WriteLine("Choose column for attack: ");
                int hitColomn = int.Parse(Console.ReadLine()) - 1;

                if (hitRow >= 0 && hitRow < gameRule.CurrentMatrix.GetLength(0) &&
                    hitColomn >= 0 && hitColomn < gameRule.CurrentMatrix.GetLength(1))
                {
                    if (gameRule.CurrentMatrix[hitRow, hitColomn] == 0)
                    {
                        Console.WriteLine("Missed");
                    }
                    else if (gameRule.CurrentMatrix[hitRow, hitColomn] == 1)
                    {
                        Console.WriteLine("Killed");
                        gameRule.CurrentMatrix[hitRow, hitColomn] = 0;
                    }

                    gameRule.Attempt++;
                }
                else
                {
                    Console.WriteLine("Coordinates out of bounds");
                }
            }

            if (!HasTargets(gameRule.FirstPlayerGameField))
            {
                Console.WriteLine("Player 2 wins!");
            }
            else
            {
                Console.WriteLine("Player 1 wins!");
            }

        }
        private int GetNumberFromConsole(string messageForUser)
        {
            var isThisANumberForMax = false;
            var number = 0;
            do
            {
                Console.WriteLine(messageForUser);
                var guessStr = Console.ReadLine();

                isThisANumberForMax = int.TryParse(guessStr, out number);
                if (!isThisANumberForMax)
                {
                    Console.WriteLine("It's not a number. Please enter just a number");
                }

            } while (!isThisANumberForMax);

            return number;
        }
        private void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        private void PlaceTheBattleship(GameRule gameRule)
        {
            int shipRow = GetNumberFromConsole("Сhoose row for Battleship:") - 1;
            int shipColomn = GetNumberFromConsole("Сhoose colomn for Battleship: ") - 1;
            gameRule.CurrentMatrix[shipRow, shipColomn] = 1;
        }
        private void SwitchMatrix(GameRule gameRule)
        {
            if (gameRule.CurrentMatrix == gameRule.FirstPlayerGameField)
                gameRule.CurrentMatrix = gameRule.SecondPlayerGameField;
            else
                gameRule.CurrentMatrix = gameRule.FirstPlayerGameField;
        }
    }
}
