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
        private readonly int nrOfRounds;
        private readonly bool testMode;

        /// <summary>
        /// Create a game of "Schiffe versenken"
        /// </summary>
        /// <param name="fieldSize">Size of the square field</param>
        /// <param name="testMode">Optional: Testmode only sets one submarine and shows it on the gamefield</param>
        public Game(int fieldSize, int nrOfRounds, bool testMode = false)
        {
            this.fieldSize = fieldSize;
            this.nrOfRounds = nrOfRounds;
            this.testMode = testMode;
        }

        /// <summary>
        /// Do a game of "Schiffe versenken"
        /// </summary>
        public void Run()
        {
            if(nrOfRounds > 0)
            {
                List<double> hitQuotients = new List<double>();
                for (int i = 1; i <= nrOfRounds; i++)
                {
                    hitQuotients.Add(DoSimulationGame(CreateGameField(), nrOfRounds == 1?100:-1));
                    Console.WriteLine();
                    Console.WriteLine($"### Average Hitquotient: {hitQuotients.Average():P2} (on {i}/{nrOfRounds} Runs) ###");
                    Thread.Sleep(10);
                }
            } else
            {
                DoHumanGame(CreateGameField());
            }
        }

        private GameField CreateGameField()
        {
                 //new GameFieldConsole(20, 4, 8, 12, 16) { ShowShips = testMode };
            return new GameFieldConsole(fieldSize) { ShowShips = testMode };
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

        private double DoSimulationGame(GameField gameField, int updateEvery = -1)
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
                    Point nextTarget = new Point { X = x, Y = y };
                    
                    if (shootField[x, y] != ShootResult.None
                        || IsVerticalShip(nextTarget, shootField) || IsHorizontalShip(nextTarget, shootField))
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

                if(updateEvery != -1)
                {
                    gameField.UpdateField();
                    Thread.Sleep(updateEvery);
                }
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
