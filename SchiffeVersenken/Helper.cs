using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public class Helper
    {
        /// <summary>
        /// Simple Point structure with X, Y Coordinate and an Equals-Method
        /// </summary>
        public struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public bool Equals(Point point)
            {
                return point.X == X && point.Y == Y;
            }
            public bool Equals(int x, int y)
            {
                return x == X && y == Y;
            }
        }

        public static int GetIntInput(string message, int min, int max, int defaultValue = -1)
        {
            Console.WriteLine(@"'x' oder 'q' zum Beenden");
            message = message.Replace("{min}", min.ToString());
            message = message.Replace("{max}", max.ToString());
            message = message.Replace("{defaultValue}", defaultValue.ToString());
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
                    if (defaultValue == -1)
                        continue;
                    else
                        return defaultValue;
                }

                if (Int32.TryParse(intStr, out int result)
                    && result >= min && result <= max)
                {
                    return result;
                }

                Console.WriteLine($"Ungültige Eingabe. Bitte nur Ganzzahlen zwischen {min} und {max} eingeben!");
            } while (true);

        }
    }
}
