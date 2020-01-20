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
                    Console.WriteLine($"### Durchschnittliche Trefferquote: {hitQuotients.Average():P2} (in {i}/{nrOfRounds} Runden) ###");
                    Thread.Sleep(10);
                }
            } else
            {
                DoHumanGame(CreateGameField());
            }
        }

        /// <summary>
        /// Create a default gamefield object
        /// </summary>
        /// <returns></returns>
        private GameField CreateGameField()
        {
                 //new GameFieldConsole(20, 4, 8, 12, 16) { ShowShips = testMode };
            return new GameFieldConsole(fieldSize) { ShowShips = testMode };
        }

        /// <summary>
        /// Starts a manual game played by a human
        /// </summary>
        /// <param name="gameField"></param>
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

            } while (!gameField.PlayerHasWon(out _));
            
            gameField.PlayerWonMessage();
        }

        /// <summary>
        /// Start a simulation game with the given parameters
        /// </summary>
        /// <param name="gameField"></param>
        /// <param name="updateEvery"></param>
        /// <returns>Hitquotient of the played game</returns>
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

        /// <summary>
        /// Return true if given Point is located in a vertical oriented ship,
        /// meaning there is another hit above or below the given Point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        private bool IsVerticalShip(Point point, ShootResult[,] shootField)
        {
            Point up = GetPointOf(point, Direction.Up);
            Point down = GetPointOf(point, Direction.Down);

            return !IsOutsideOfField(up) && shootField[up.X, up.Y] >= ShootResult.ShipHit
                || !IsOutsideOfField(down) && shootField[down.X, down.Y] >= ShootResult.ShipHit;
        }

        /// <summary>
        /// Return true if given Point is located in a horizontal oriented ship.
        /// meaning there is another hit left or right of the given Point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        private bool IsHorizontalShip(Point point, ShootResult[,] shootField)
        {
            Point left = GetPointOf(point, Direction.Left);
            Point right = GetPointOf(point, Direction.Right);

            return !IsOutsideOfField(left) && shootField[left.X, left.Y] >= ShootResult.ShipHit
                || !IsOutsideOfField(right) && shootField[right.X, right.Y] >= ShootResult.ShipHit;
        }

        /// <summary>
        /// Get a Point that is above, below, right or left of the given Point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction">Direction.[Up|Down|Left|Right]</param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if Point is inside gamefield and was not already shot
        /// </summary>
        /// <param name="guessPoint"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        private bool IsPossibleTarget(Point guessPoint, ShootResult[,] shootField)
        {
            return !IsOutsideOfField(guessPoint) && !WasAlreadyShot(guessPoint, shootField);
        }

        /// <summary>
        /// Returns true if position of given Point was already shot (hit or miss)
        /// </summary>
        /// <param name="guessPoint"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        private bool WasAlreadyShot(Point guessPoint, ShootResult[,] shootField)
        {
            return shootField[guessPoint.X, guessPoint.Y] != ShootResult.None;
        }

        /// <summary>
        /// Returns true if given Point is outside of the gamefield
        /// </summary>
        /// <param name="guessPoint"></param>
        /// <returns></returns>
        private bool IsOutsideOfField(Point guessPoint)
        {
            return guessPoint.X < 0 || guessPoint.X >= fieldSize || guessPoint.Y < 0 || guessPoint.Y >= fieldSize;
        }
 
    }
}
