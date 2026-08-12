using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.SaperGame.Saper
{
    internal class MineField
    {
        public int width { get; set; } = 9;
        public int height { get; set; } = 9;
        public int maxBomb { get; set; } = 10;
        public int x { get; set; }
        public int y { get; set; }

        public Cell[,] mineField;

        public MineField()
        {
            mineField = new Cell[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    mineField[i, j] = new Cell();
                }
            }
        }

        public void InitializationBomb()
        {
            Random rand = new Random();

            for (int i = 0; i <= maxBomb; i++)
            {
                var x = rand.Next(0, width);
                var y = rand.Next(0, height);
                mineField[x, y].name = "bomb";                
            } 
        }        

        public void SearchBombAround()
        {
            for( int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    int numbOfBombAround = 0;
                    //обход вокруг ячейки
                    if (mineField[i,j].name != "bomb")
                    {
                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            if (k < 0 || k >= width)
                            {
                                continue;
                            }
                            for (int m = j - 1; m <= j + 1; m++)
                            {
                                if (m < 0 || m >= height)
                                {
                                    continue;
                                }
                                if (mineField[k,m].name == "bomb")
                                {
                                    numbOfBombAround++;
                                }
                            }
                        }
                        mineField[i,j].numbOfBombsAround = numbOfBombAround;
                    }                    
                }
            }
        }       

        public bool OpenCell()
        {
            int i = x;
            int j = y;
            if (mineField[i,j].name == "bomb")
            {
                Console.WriteLine("You lose :((("); 
                mineField[i, j].status = "open";

                return true;
            }
            //обход вокруг ячейки
            for (int k = i - 1; k <= i + 1; k++)
            {
                    if (k < 0 || k >= width)
                    {
                        continue;
                    }
                    for (int m = j - 1; m <= j + 1; m++)
                    {
                        if (m < 0 || m >= height)
                        {
                            continue;
                        }
                        else if (mineField[k, m].name == "bomb")
                        {
                            continue; 
                        }
                        else
                        {
                            mineField[k,m].status = "open";
                        }
                    }
            }
            return false;                
        }

        public void PrintMineFieldRound()
        {
            Console.Write("   ");
            for (int n = 0; n < width; n++)
            {
                Console.Write(n + " ");
            }
            Console.WriteLine();
            Console.Write("   ");
            for (int n = 0; n < width; n++)
            {
                Console.Write("- ");
            }
            Console.WriteLine();

            for (int i = 0; i < width; i++)
            {
                Console.Write(i + " |");
                for (int j = 0; j < height; j++)
                {
                    if (mineField[i,j].status == "close")
                    {
                        Console.Write(mineField[i, j].closeVisual + " ");
                    }
                    else if (mineField[i, j].status == "open" && mineField[i, j].name == "bomb")
                    {
                        Console.Write("💣" + " ");
                    }
                    else
                    {
                        Console.Write(mineField[i, j].numbOfBombsAround + " ");
                    }                    
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void InteractionWithUser()
        {
            var tryX = false;
            var tryY = false;

            do
            {
                Console.WriteLine("Select a cell and enter its coordinates in the range 0 - 8.");
                Console.WriteLine("Enter x:");
                tryX = int.TryParse(Console.ReadLine(), out int xInput);

                x = xInput;

                Console.WriteLine("Enter y:");
                tryY = int.TryParse(Console.ReadLine(), out int yInput);

                y = yInput;

                if (!tryX && !tryY)
                {
                    Console.WriteLine($"Input error try again."); 
                }
                else
                {
                    Console.WriteLine($"you have selected a cell with coordinates: ({x}, {y})");
                }      
            }  while (!tryX && !tryY);            
        }

        public bool DetermineVictory()
        {            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (mineField[i, j].name != "bomb" && mineField[i, j].status == "close")
                    {
                        return false;
                    }
                }               
            }
            Console.WriteLine();
            Console.WriteLine("YOU ARE WIN !!! !!! !!!");
            Console.WriteLine();
            return true;
        }

        //ОТЛАДКА
        public void Print()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Console.Write(mineField[i, j].name + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void PrintNumbOfBombsAround()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (mineField[i, j].name == "bomb")
                    {
                        Console.Write(mineField[i, j].closeVisual + " ");
                    }
                    else
                    {
                        Console.Write(mineField[i, j].numbOfBombsAround + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void PrintMineFieldVisual()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Console.Write(mineField[i, j].closeVisual + " ");   //'□'💣■
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        //END ОТЛАДКА
    }
}
