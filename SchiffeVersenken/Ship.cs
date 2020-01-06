using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
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
    }

    /// <summary>
    /// Class that represents one Ship
    /// </summary>
    public class Ship
    {
        public Point[] points { get; }
        private bool[] pointsAlive;
        private bool vertical;
        public bool IsAlive {
            get
            {
                // Ship is alive, if any point is still alive
                return pointsAlive.Any(alive => alive);
            }
        }
        public string shipType
        {
            get
            {
                switch (points.Length)
                {
                    case 5:
                        return "Schlachtschiff";
                    case 4:
                        return "Kreuzer";
                    case 3:
                        return "Zerstörer";
                    case 2:
                        return "U-Boot";
                    default:
                        return "unbekanntes Schiff";
                }
            }
        }


        public Ship(int xStart, int yStart, int xStop, int yStop)
        {
            vertical = xStart == xStop;
            points = new Point[Math.Max(xStop+1 - xStart, yStop+1 - yStart)];
            pointsAlive = Enumerable.Repeat(true, points.Length).ToArray();

            for (int i = 0; i < points.Length; i++)
            {
                if (vertical)
                {
                    points[i].X = xStart;
                    points[i].Y = yStart + i;
                }
                else
                {
                    points[i].X = xStart + i;
                    points[i].Y = yStart;
                }
            }
        }

        /// <summary>
        /// Check if another ship intersects with this ship
        /// </summary>
        /// <param name="other">The other ship</param>
        /// <returns></returns>
        public bool IntersectsShip(Ship other)
        {
            for (int i = 0; i < points.Length; i++)
            {
                for(int j=0; j < other.points.Length; j++)
                {
                    // if point of ship intersects with point of other ship
                    if(points[i].Equals(other.points[j]))
                    {
                        return true;
                    }

                    // ships should not touch each other
                    if(  points[i].X + 1 == other.points[j].X && points[i].Y == other.points[j].Y
                      || points[i].X == other.points[j].X && points[i].Y + 1 == other.points[j].Y
                      || points[i].X - 1 == other.points[j].X && points[i].Y == other.points[j].Y
                      || points[i].X == other.points[j].X && points[i].Y - 1 == other.points[j].Y
                      )
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if a Point intersects with this ship
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns></returns>
        public bool ShootShip(Point point)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].Equals(point))
                {
                    pointsAlive[i] = false;
                    return true;
                }
            }

            return false;
        }
    }
}
