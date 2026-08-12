using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.SaperGame.Saper
{
    internal class Saper
    {
        public void Game() 
        {
            var mineField = new MineField();

            mineField.InitializationBomb();
            mineField.SearchBombAround();


            var flagLose = false;
            var flagWin = false;
            mineField.PrintMineFieldRound();
            do
            {
                mineField.InteractionWithUser();
                flagLose = mineField.OpenCell();
                flagWin = mineField.DetermineVictory();
                mineField.PrintMineFieldRound();

            } while (!flagLose && !flagWin);




            //отладка
            //mineField.PrintNumbOfBombsAround();
            //mineField.Print();
            //mineField.PrintMineFieldVisual();

        }
    }
}
