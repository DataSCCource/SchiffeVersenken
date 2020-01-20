using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken.Solver
{
    /// <summary>
    /// Solver, that shoots random until a ship is hit HOWEVER,
    /// if the guessed shot is already next to a ship, searches for other coordinates.
    /// That hit goes into a list and has now 4 possibilities (above, below, left, right)
    /// Also checks if ship is vertical or horizontal and guesses appropriately
    /// 
    /// </summary>
    public class FullSolver : AbstractSolver
    {
        public override double DoSimulationGame(GameField gameField, int updateEvery = -1)
        {
            fieldSize = gameField.FieldSize;
            shootField = new ShootResult[fieldSize, fieldSize];

            List<Point> listOfHits = new List<Point>();
            Random rnd = new Random();
            double hitQuotient;

            do
            {
                int x, y;
                if (listOfHits.Count == 0)
                {
                    x = rnd.Next(0, fieldSize);
                    y = rnd.Next(0, fieldSize);
                    Point nextTarget = new Point (x, y);

                    if (shootField[x, y] != ShootResult.None
                        || IsVerticalShip(nextTarget) || IsHorizontalShip(nextTarget))
                    {
                        continue;
                    }
                }
                else
                {
                    Point firstHit = listOfHits.ElementAt(0);
                    Point nextTarget = Point.INVALID;

                    Point targetLeft = GetPointOf(firstHit, Direction.Left);
                    Point targetRight = GetPointOf(firstHit, Direction.Right);
                    Point targetUp = GetPointOf(firstHit, Direction.Up);
                    Point targetDown = GetPointOf(firstHit, Direction.Down);

                    if (IsPossibleTarget(targetLeft) && !IsVerticalShip(firstHit))
                    {
                        nextTarget = targetLeft;
                    }
                    else if (IsPossibleTarget(targetRight) && !IsVerticalShip(firstHit))
                    {
                        nextTarget = targetRight;
                    }
                    else if (IsPossibleTarget(targetUp) && !IsHorizontalShip(firstHit))
                    {
                        nextTarget = targetUp;
                    }
                    else if (IsPossibleTarget(targetDown) && !IsHorizontalShip(firstHit))
                    {
                        nextTarget = targetDown;
                    }

                    if (nextTarget.Equals(Point.INVALID))
                    {
                        listOfHits.RemoveAt(0);
                        continue;
                    }
                    x = nextTarget.X;
                    y = nextTarget.Y;

                }

                shootField[x, y] = gameField.Shoot(x, y);
                if (shootField[x, y] == ShootResult.ShipHit)
                {
                    listOfHits.Add(new Point(x, y));
                }
                else if (shootField[x, y] == ShootResult.DestroyShip)
                {
                    listOfHits.Clear();
                }

                if (updateEvery != -1)
                {
                    gameField.UpdateField();
                    Thread.Sleep(updateEvery);
                }
            } while (!gameField.PlayerHasWon(out hitQuotient));

            gameField.PlayerWonMessage();

            return hitQuotient;
        }
    }
}
