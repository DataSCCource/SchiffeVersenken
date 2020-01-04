using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(Point point)
        {
            return point.X == X && point.Y == Y;
        }
    }

    public class Ship
    {
        private Point[] points;
        private bool vertical;

        public Ship(int xStart, int yStart, int xStop, int yStop)
        {
            vertical = xStart == xStop;
            points = new Point[Math.Max(xStop+1 - xStart, yStop+1 - yStart)];

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

        public bool IntersectsShip(Ship other)
        {
            for (int i = 0; i < points.Length; i++)
            {
                for(int j=0; j < other.points.Length; j++)
                {
                    // if point of ship intersects with point of other ship
                    if (points[i].X == other.points[j].X && points[i].Y == other.points[j].Y)
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

        public bool IntersectsShip(Point point)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if(points[i].Equals(point))
                {
                    return true;
                }

            }
            return false;
        }


        internal void SetShipOnField(char[,,] field)
        {
            for(int i=0; i<points.Length; i++)
            {
                field[points[i].X, points[i].Y, 1] = 'O';
            }
        }
    }

}
