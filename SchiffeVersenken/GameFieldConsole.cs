using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public class GameFieldConsole : GameField
    {
        public GameFieldConsole(int fieldSize = 10, int nrBs = 1, int nrCr = 2, int nrDest = 3, int nrSub = 4) 
            : base(fieldSize, nrBs, nrCr, nrDest, nrSub) { }


        /// <summary>
        /// Print gamefield, fired shots and, if requested, the ships
        /// </summary>
        public override void UpdateField()
        {
            Console.Clear();
            if (nrOfShots > 0) ShowHitMessage(); else Console.WriteLine("\n");
            UpdateScore();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");
            for (int i = 0; i < fieldSize; i++)
            {
                Console.Write(String.Format("{0,2}", i));
            }
            Console.WriteLine(" X");


            for (int y = 0; y < fieldSize; y++)
            {
                //Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0,3} ", y));
                for (int x = 0; x < fieldSize; x++)
                {
                    if (field[x, y, 2].Equals('W'))
                    {
                        //Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(field[x, y, 2]);
                    }
                    else if (field[x, y, 2].Equals('X'))
                    {
                        //Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(field[x, y, 2]);
                    }
                    else if (field[x, y, 1] != 0 && ShowShips)
                    {
                        //Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(field[x, y, 1]);
                    }
                    else
                    {
                        //Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(field[x, y, 0]);
                    }
                    //Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("  Y");
            for (int i = 0; i < 2 * fieldSize + 4; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Print current score
        /// </summary>
        public override void UpdateScore()
        {
            double hitQuotient = 0;
            if (nrOfShots > 0) hitQuotient = (double)nrOfHits / nrOfShots;
            Console.WriteLine($"Schüsse: {nrOfShots} | Treffer: {nrOfHits} | Trefferquote: {hitQuotient:P2}");
            Console.WriteLine();
        }


        /// <summary>
        /// Get userinput and validate it.
        /// Exit Game on 'x'
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="maxValue">Do validation of userinput</param>
        /// <returns></returns>
        public override int GetIntInput(string message)
        {
            Console.WriteLine(@"'x' oder 'q' zum Beenden");
            do
            {
                Console.Write(message);
                string intStr = Console.ReadLine();
                // exit on 'x' or 'q'
                if (intStr.ToLower().Equals("x") || intStr.ToLower().Equals("q"))
                {
                    System.Environment.Exit(1);
                }
                else if (intStr.ToLower().Equals(""))
                {
                    // ignore empty strings
                    continue;
                }

                int result;
                if (Int32.TryParse(intStr, out result)
                    && result >= 0 && result <= fieldSize - 1)
                {
                    return result;
                }

                Console.WriteLine($"Ungültige Eingabe. Bitte nur Werte zwischen 0 und {fieldSize - 1} eingeben!");
            } while (true);
        }

        public override void ShowHitMessage()
        {
            if (lastShotWasHit)
            {
                Console.WriteLine("Treffer! :)");
                if (lastShotKilledShip != -1)
                {
                    Console.WriteLine($"{ships[lastShotKilledShip].shipType} zerstört! \\o/");
                }

            }
            else
            {
                Console.WriteLine("Leider kein Treffer! :(");
            }
            Console.WriteLine();
        }

        public override void PlayerWonMessage()
        {
            UpdateField();
            Console.WriteLine("\nHerzlichen Glueckwunsch, du hast gewonnen! :)");
        }
    }
}
