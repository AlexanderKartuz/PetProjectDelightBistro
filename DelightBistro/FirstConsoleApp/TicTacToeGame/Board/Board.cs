namespace FirstConsoleApp.TicTacToeGame
{
    internal class Board
    {

        private char[,] _field;
        private int _size = 3;
        private char _emptyField = '-';

        public Board()
        {
            _field = new char[_size, _size];
            CreateBoard();
        }
        //Create Empty Board
        private void CreateBoard()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _field[i, j] = _emptyField;
                }
            }
        }

        public void ShowBoard()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Console.Write(_field[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        
        public bool SetField(int row, int column, char mark)
        {

            if (_field[row - 1, column - 1] == _emptyField)
            {
                _field[row - 1, column - 1] = mark;
                return true;
            }
            else
            {
                Console.WriteLine("The field is not empty. Enter new coordinates.");
                return false;
            }
        }
        
        public bool IsBoardFull()
        {
            foreach (char value in _field)
            {
                if (value == _emptyField)
                {
                    return false;
                }
            }
            return true;
        }
        //Check Winner
        public bool CheckWinner(char mark)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_field[i, 0] == mark && _field[i, 1] == mark && _field[i, 2] == mark)
                {
                    return true;
                }
            }
            for (int j = 0; j < _size; j++)
            {
                if (_field[0, j] == mark && _field[1, j] == mark && _field[2, j] == mark)
                {
                    return true;
                }
            }
            if (_field[0, 0] == mark && _field[1, 1] == mark && _field[2, 2] == mark)
            {
                return true;
            }
            if (_field[0, 2] == mark && _field[1, 1] == mark && _field[2, 0] == mark)
            {
                return true;
            }
            return false;
        }
    }
}
