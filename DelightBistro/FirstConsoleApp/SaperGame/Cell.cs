using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp2.SaperGame.Saper
{
    internal class Cell
    {
        public string name { get; set; } = "cell";
        public string status { get; set; } = "close";
        public string closeVisual { get; set; } = "■"; ////💣

        public string bombVisual { get; set; } = "💣";

        public int numbOfBombsAround { get; set; } = 0;

        public Cell()
        {            
            
        }

    }
}
