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
            //GameField gameField = new GameField(10, 0, 0, 0, 1);
            //GameField gameField = new GameField(20, 4, 8, 12, 16);

            do
            {
                gameField.PrintField();
                gameField.PrintHorizentalLine();

                Console.Write("Bitte X-Koordinate eingeben: ");
                string xStr = Console.ReadLine();
                Console.Write("Bitte Y-Koordinate eingeben: ");
                string yStr = Console.ReadLine();

                int x = int.Parse(xStr);
                int y = int.Parse(yStr);

                if (gameField.Shoot(x, y))
                {
                    Console.WriteLine("Treffer! :)");
                }
                else
                {
                    Console.WriteLine("Leider kein Treffer! :(");
                }
                Console.WriteLine();
            } while (!gameField.PlayerHasWon());


            Console.WriteLine("\nHerzlichen Glueckwunsch, du hast gewonnen! :)");

        }
    }
}
