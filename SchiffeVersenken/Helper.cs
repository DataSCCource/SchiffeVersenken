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

            public override bool Equals(object obj)
            {
                if (obj is Point point)
                {
                    return this.Equals(point);
                }
                return false;
            }

            private bool Equals(Point point)
            {
                return point.X == this.X && point.Y == this.Y;
            }
            public bool Equals(int x, int y)
            {
                return x == this.X && y == this.Y;
            }
        }

        public static int GetIntInput(string message, int min, int max, int defaultValue = -1)
        {
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
                    return int.MinValue;
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

        public static void CheckExitInt(int value)
        {
            if(value == int.MinValue)
            {
                System.Environment.Exit(1);
            }
        }
    }
}
