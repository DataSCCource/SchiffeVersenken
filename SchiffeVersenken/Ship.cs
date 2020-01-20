using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken
{
    // Shiptypes and sizes
    public enum ShipType
    {
        Battleship = 5,
        Cruiser = 4,
        Destroyer = 3,
        Submarine = 2
    }


    /// <summary>
    /// Class that represents one Ship
    /// </summary>
    public class Ship
    {
        public Point[] Points { get; }
        private readonly bool[] pointsAlive;

        public bool IsAlive {
            get
            {
                // Ship is alive, if any point is still alive
                return pointsAlive.Any(alive => alive);
            }
        }

        public string ShipTypeString
        {
            get
            {
                switch ((ShipType)Points.Length)
                {
                    case ShipType.Battleship:
                        return "Schlachtschiff";
                    case ShipType.Cruiser:
                        return "Kreuzer";
                    case ShipType.Destroyer:
                        return "Zerstörer";
                    case ShipType.Submarine:
                        return "U-Boot";
                    default:
                        return "unbekannter Schiffstyp";
                }
            }
        }

        public Ship(int xStart, int yStart, int xStop, int yStop)
            : this (new Point { X = xStart, Y = yStart}, new Point { X = xStop, Y = yStop }) { }

        public Ship(Point startPoint, Point stopPoint) 
        {
            bool vertical = startPoint.X == stopPoint.X;
            Points = new Point[Math.Max(stopPoint.X + 1 - startPoint.X, stopPoint.Y + 1 - startPoint.Y)];
            pointsAlive = Enumerable.Repeat(true, Points.Length).ToArray();

            for (int i = 0; i < Points.Length; i++)
            {
                if (vertical)
                {
                    Points[i].X = startPoint.X;
                    Points[i].Y = startPoint.Y + i;
                }
                else
                {
                    Points[i].X = startPoint.X + i;
                    Points[i].Y = startPoint.Y;
                }
            }
        }

        /// <summary>
        /// Check if another ship would intersect or touch  this ship
        /// </summary>
        /// <param name="other">The other ship</param>
        /// <returns>True if the given Ship intersects or touches this ship</returns>
        public bool IntersectsOrTouchesShip(Ship other)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                for(int j=0; j < other.Points.Length; j++)
                {
                    // if point of ship intersects with point of other ship
                    if(Points[i].Equals(other.Points[j]))
                    {
                        return true;
                    }

                    // check if points touch eath other
                    if(  Points[i].X + 1 == other.Points[j].X && Points[i].Y == other.Points[j].Y
                      || Points[i].X == other.Points[j].X && Points[i].Y + 1 == other.Points[j].Y
                      || Points[i].X - 1 == other.Points[j].X && Points[i].Y == other.Points[j].Y
                      || Points[i].X == other.Points[j].X && Points[i].Y - 1 == other.Points[j].Y
                      )
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Shoot at this point and check if a point intersects with this ship
        /// </summary>
        /// <param name="x">X-Coordinate to shoot at</param>
        /// <param name="y">Y-Coordinate to shoot at</param>
        /// <returns>True if Coordinate hits this ship, False otherwise</returns>
        public bool ShootShip(int x, int y)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                if (Points[i].Equals(x, y))
                {
                    pointsAlive[i] = false;
                    return true;
                }
            }

            return false;
        }
    }
}
