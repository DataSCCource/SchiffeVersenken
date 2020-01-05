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
            //GameField gameField = new GameField();
            GameField gameField = new GameField(10, 0, 0, 0, 1);
            //GameField gameField = new GameField(20, 4, 8, 12, 16);

            do
            {
                gameField.PrintField();
                gameField.PrintHorizentalLine();

                Console.WriteLine(@"'x' zum Beenden");
                int x = GetIntInput("Bitte X-Koordinate eingeben: ", gameField.fieldSize - 1);
                int y = GetIntInput("Bitte Y-Koordinate eingeben: ", gameField.fieldSize - 1);

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

        private static int GetIntInput(string message, int maxValue)
        {
            do
            {
                Console.Write(message);
                string intStr = Console.ReadLine();
                if(intStr.ToLower().Equals("x"))
                {
                    System.Environment.Exit(1);
                } else if (intStr.ToLower().Equals(""))
                {
                    continue;
                }

                int result;
                Int32.TryParse(intStr, out result);
                if(result >= 0 && result <= maxValue)
                {
                    return result;
                }

                Console.WriteLine($"Ungültige Eingabe. Bitte nur Werte zwischen 0 und {maxValue} eingeben!");
            } while (true);
        }
    }
}
