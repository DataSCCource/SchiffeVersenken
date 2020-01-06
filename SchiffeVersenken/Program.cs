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
            int fieldSize = AskForFieldSize();

            //GameField gameField = new GameFieldConsole(fieldSize);
            GameField gameField = new GameFieldConsole(fieldSize, 0, 0, 0, 1);
            //GameField gameField = new GameFieldConsole(20, 4, 8, 12, 16);

            // TODO: Remove
            gameField.ShowShips = true;

            do
            {
                gameField.UpdateField();

                int x = gameField.GetIntInput("Bitte X-Koordinate eingeben: ");
                int y = gameField.GetIntInput("Bitte Y-Koordinate eingeben: ");

                gameField.Shoot(x, y);

            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();
        }

        private static int AskForFieldSize()
        {
            int defaultSize = 10;
            int min = 10;
            int max = 20;
            Console.WriteLine(@"'x' oder 'q' zum Beenden");
            do
            {
                Console.Write($"Bitte Feldgröße angeben ({min}-{max}) Standard ist {defaultSize}: ");
                string intStr = Console.ReadLine();
                // exit on 'x' or 'q'
                if (intStr.ToLower().Equals("x") || intStr.ToLower().Equals("q"))
                {
                    System.Environment.Exit(1);
                }
                else if (intStr.ToLower().Equals(""))
                {
                    return defaultSize;
                }

                int result;
                if (Int32.TryParse(intStr, out result)
                    && result >= min && result <= max)
                {
                    return result;
                }

                Console.WriteLine($"Ungültige Eingabe. Bitte nur Werte zwischen {min} und {max} eingeben!");
            } while (true);

        }
    }
}
