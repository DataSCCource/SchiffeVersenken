using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken
{
    public class Game
    {
        private readonly int fieldSize;
        private readonly bool testMode;

        /// <summary>
        /// Create a game of "Schiffe versenken"
        /// </summary>
        /// <param name="fieldSize">Size of the square field</param>
        /// <param name="testMode">Optional: Testmode only sets one submarine and shows it on the gamefield</param>
        public Game(int fieldSize, bool testMode = false)
        {
            this.fieldSize = fieldSize;
            this.testMode = testMode;
        }

        /// <summary>
        /// Do a game of "Schiffe versenken"
        /// </summary>
        public void Run()
        {
            //GameField gameField = new GameFieldConsole(20, 4, 8, 12, 16);
            GameField gameField;
            if (testMode)
            {
                gameField = new GameFieldConsole(fieldSize, 0, 0, 0, 1)
                {
                    ShowShips = true
                };
            }
            else
            {
                gameField = new GameFieldConsole(fieldSize);
            }

            List<double> hitQuotients = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                hitQuotients.Add(DoSimulationGame(new GameFieldConsole(fieldSize)));
            }

            Console.WriteLine($"HITQUOTIENTS: {hitQuotients.Average():P2}");
            //DoHumanGame(gameField);

        }

        private void DoHumanGame(GameField gameField)
        {
            do
            {
                gameField.UpdateField();
                Console.WriteLine(@"'x' oder 'q' zum Beenden");

                int x = Helper.GetIntInput("Bitte X-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.ExitOnExitValue(x);
                int y = Helper.GetIntInput("Bitte Y-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.ExitOnExitValue(y);

                gameField.Shoot(x, y);

            } while (!gameField.PlayerHasWon(out double hitQuotient));
            
            gameField.PlayerWonMessage();
        }

        private double DoSimulationGame(GameField gameField)
        {
            ShootResult[,] shootField = new ShootResult[fieldSize, fieldSize];
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
                    if (shootField[x, y] != ShootResult.None)
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
                    
                    if (IsPossibleTarget(targetLeft, shootField) && !IsVerticalShip(firstHit, shootField))
                    {
                        nextTarget = targetLeft;
                    }
                    else if (IsPossibleTarget(targetRight, shootField) && !IsVerticalShip(firstHit, shootField))
                    {
                        nextTarget = targetRight;
                    }
                    else if (IsPossibleTarget(targetUp, shootField) && !IsHorizontalShip(firstHit, shootField))
                    {
                        nextTarget = targetUp;
                    }
                    else if (IsPossibleTarget(targetDown, shootField) && !IsHorizontalShip(firstHit, shootField))
                    {
                        nextTarget = targetDown;
                    }

                    if(nextTarget.Equals(Point.INVALID))
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
                    listOfHits.Add(new Point { X = x, Y = y });
                }
                else if (shootField[x, y] == ShootResult.DestroyShip)
                {
                    listOfHits.Clear();
                }

                //gameField.UpdateField();
                //Thread.Sleep(100);
            } while (!gameField.PlayerHasWon(out hitQuotient));

            gameField.PlayerWonMessage();

            return hitQuotient;
        }

        private bool IsVerticalShip(Point point, ShootResult[,] shootField)
        {
            int x = point.X;
            int y = point.Y;
            Point up = GetPointOf(point, Direction.Up);
            Point down = GetPointOf(point, Direction.Down);

            return !IsOutsideOfField(up) && shootField[up.X, up.Y] >= ShootResult.ShipHit
                || !IsOutsideOfField(down) && shootField[down.X, down.Y] >= ShootResult.ShipHit;
        }

        private bool IsHorizontalShip(Point point, ShootResult[,] shootField)
        {
            int x = point.X;
            int y = point.Y;
            Point left = GetPointOf(point, Direction.Left);
            Point right = GetPointOf(point, Direction.Right);

            return !IsOutsideOfField(left) && shootField[left.X, left.Y] >= ShootResult.ShipHit
                || !IsOutsideOfField(right) && shootField[right.X, right.Y] >= ShootResult.ShipHit;
        }

        private Point GetPointOf(Point point, Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return new Point { X = point.X, Y = point.Y - 1 };
                case Direction.Left:
                    return new Point { X = point.X - 1, Y = point.Y };
                case Direction.Down:
                    return new Point { X = point.X, Y = point.Y + 1 };
                case Direction.Right:
                    return new Point { X = point.X + 1, Y = point.Y };
                default: 
                    return Point.INVALID;
            }
        }

        private bool IsPossibleTarget(Point guessPoint, ShootResult[,] shootField)
        {
            return !IsOutsideOfField(guessPoint) && !WasAlreadyShot(guessPoint, shootField);
        }

        private bool WasAlreadyShot(Point guessPoint, ShootResult[,] shootField)
        {
            return shootField[guessPoint.X, guessPoint.Y] != ShootResult.None;
        }

        private bool IsOutsideOfField(Point guessPoint)
        {
            return guessPoint.X < 0 || guessPoint.X >= fieldSize || guessPoint.Y < 0 || guessPoint.Y >= fieldSize;
        }
 
    }
}
