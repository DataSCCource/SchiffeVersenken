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

            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            /// <summary>
            /// Test if given Object is a point and if it equals this object
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj is Point point)
                {
                    return this.Equals(point.X, point.Y);
                }
                return false;
            }

            /// <summary>
            /// Test if given point is equal to this point
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            private bool Equals(Point point)
            {
                return this.Equals(point);
            }

            /// <summary>
            /// Test if given coordinates belong to this point
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(int x, int y)
            {
                return x == this.X && y == this.Y;
            }
        }

        /// <summary>
        /// Ask User von an integer input
        /// </summary>
        /// <param name="message">Message to show (can use Variables {min}, {max} and {defaultValue})</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximal value</param>
        /// <param name="defaultValue">Default value, if no input is done</param>
        /// <returns></returns>
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

        /// <summary>
        /// If Value -2147483648 (int.MinValue) is given, exit program
        /// </summary>
        /// <param name="value"></param>
        public static void CheckExitValue(int value)
        {
            if(value == int.MinValue)
            {
                System.Environment.Exit(1);
            }
        }
    }
}
