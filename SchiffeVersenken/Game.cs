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
            
            DoSimulationGame(gameField);
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

            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();
        }

        private void DoSimulationGame(GameField gameField)
        {
            ShootResult[,] shootField = new ShootResult[fieldSize, fieldSize];
            Point lastHitPoint = new Point { X = -1, Y = -1 };
            Random rnd = new Random();

            do
            {
                int x, y;
                if (lastHitPoint.Equals(Point.INVALID))
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
                    Point nextShot = GetNextShot(shootField, lastHitPoint);
                    if(nextShot.Equals(Point.INVALID))
                    {
                        lastHitPoint = Point.INVALID;
                        continue;
                    }

                    x = nextShot.X;
                    y = nextShot.Y;
                }

                shootField[x, y] = gameField.Shoot(x, y);
                if (shootField[x, y] == ShootResult.ShipHit)
                {
                    lastHitPoint = new Point { X = x, Y = y };
                } 
                else if(shootField[x, y] == ShootResult.DestroyShip)
                {
                    lastHitPoint = Point.INVALID;
                }

                gameField.UpdateField();
                Thread.Sleep(500);
            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();

        }

        private Point GetNextShot(ShootResult[,] shootField, Point lastHitPoint, int depth = 1)
        {
            if(depth > fieldSize)
            {
                return Point.INVALID;
            }

            int x = lastHitPoint.X;
            int y = lastHitPoint.Y;


            if (!IsOutsideOfField(x - 1, y))
            {
                if(shootField[x - 1, y] >= ShootResult.ShipHit)
                {
                    return GetNextShot(shootField, new Point { X = x - 1, Y = y }, depth + 1);
                } 
                else if(shootField[x - 1, y] == ShootResult.None)
                {
                    return new Point { X = x - 1, Y = y };
                }
            }

            if (!IsOutsideOfField(x, y - 1))
            {
                if (shootField[x, y - 1] >= ShootResult.ShipHit)
                {
                    return GetNextShot(shootField, new Point { X = x, Y = y - 1 }, depth + 1);
                }
                else if (shootField[x, y - 1] == ShootResult.None)
                {
                    return new Point { X = x, Y = y - 1 };
                }
            }

            if (!IsOutsideOfField(x + 1, y))
            {
                if (shootField[x + 1, y] >= ShootResult.ShipHit)
                {
                    return GetNextShot(shootField, new Point { X = x + 1, Y = y }, depth + 1);
                }
                else if (shootField[x + 1, y] == ShootResult.None)
                {
                    return new Point { X = x + 1, Y = y };
                }
            }


            if (!IsOutsideOfField(x, y + 1))
            {
                if (shootField[x, y + 1] >= ShootResult.ShipHit)
                {
                    return GetNextShot(shootField, new Point { X = x, Y = y + 1 }, depth + 1);
                }
                else if (shootField[x, y + 1] == ShootResult.None)
                {
                    return new Point { X = x, Y = y + 1 };
                }
            }

            return Point.INVALID;
        }



        private bool IsOutsideOfField(int x, int y)
        {
            return x < 0 || x >= fieldSize || y < 0 || y >= fieldSize;
        }
 
    }
}
