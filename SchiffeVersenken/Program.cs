using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    class Program
    {
        static void Main(string[] args)
        {
            GameField gameField = new GameField();
            //GameField gameField = new GameField(20, 4, 8, 12, 16);

            //do
            //{
                gameField.PrintField();
                gameField.PrintHorizentalLine();
            //} while (true);


        }
    }
}
