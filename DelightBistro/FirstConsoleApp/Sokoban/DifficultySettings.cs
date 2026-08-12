namespace FirstConsoleApp.Sokoban
{
    public class DifficultySettings
    {
        public int Size { get; set; }

        public int BoxesAndMarks { get; set; }

        public char[,] Map {  get; set; }

        public int PlayerX { get; set; }

        public int PlayerY { get; set; }

        public DifficultySettings(int size, int boxesAndMarks)
        {
            Size = size;
            BoxesAndMarks = boxesAndMarks;
            Map = new char[size, size];
        }
    }
}
