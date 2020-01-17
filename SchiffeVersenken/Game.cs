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
            int[,] shootField = new int[fieldSize, fieldSize];
            Point lastHitPoint = new Point();
            Direction lastDirection = Direction.None;
            Direction priorToLastDirection = Direction.None;
            Random rnd = new Random();

            do
            {
                gameField.UpdateField();
                int x = 0, y = 0;
                do
                {
                    if(lastDirection == Direction.None)
                    {
                        x = rnd.Next(0, fieldSize);
                        y = rnd.Next(0, fieldSize);
                    }
                    else
                    {
                        switch (lastDirection)
                        {
                            case Direction.Up:
                                x = lastHitPoint.X;
                                y = lastHitPoint.Y - 1;
                                if (IsOutsideOfField(x, y))
                                {
                                    lastDirection = Direction.Down;
                                    y = lastHitPoint.Y+1;
                                    if (IsAlreadyShot(shootField, x, y))
                                    {
                                        if(rnd.Next()%2 == 0)
                                        {
                                            lastDirection = Direction.Left;
                                        }
                                        else
                                        {
                                            lastDirection = Direction.Right;
                                        }
                                    }
                                }
                                break;
                            case Direction.Down:
                                x = lastHitPoint.X;
                                y = lastHitPoint.Y + 1;
                                if (IsOutsideOfField(x, y))
                                {
                                    lastDirection = Direction.Up;
                                    y = lastHitPoint.Y-1;
                                }
                                break;
                            case Direction.Left:
                                x = lastHitPoint.X - 1;
                                y = lastHitPoint.Y;
                                if (IsOutsideOfField(x, y))
                                {
                                    lastDirection = Direction.Right;
                                    x = lastHitPoint.X+1;
                                }
                                break;
                            case Direction.Right:
                                x = lastHitPoint.X + 1;
                                y = lastHitPoint.Y;
                                if (IsOutsideOfField(x, y))
                                {
                                    lastDirection = Direction.Left;
                                    x = lastHitPoint.X-1;
                                }
                                break;
                        }
                        //if(IsOutsideOfField(x, y))
                        //{
                        //    if(priorToLastDirection != Direction.None)
                        //    {
                        //        switch (lastDirection)
                        //        {
                        //            case Direction.Up:
                        //                lastHitPoint.Y++;
                        //                lastDirection = Direction.Down;
                        //                break;
                        //            case Direction.Down:
                        //                lastHitPoint.Y--;
                        //                lastDirection = Direction.Up;
                        //                break;
                        //            case Direction.Left:
                        //                lastHitPoint.X++;
                        //                lastDirection = Direction.Right;
                        //                break;
                        //            case Direction.Right:
                        //                lastHitPoint.X--;
                        //                lastDirection = Direction.Left;
                        //                break;
                        //        }
                        //        priorToLastDirection = Direction.None;
                        //    } 
                        //    else
                        //    {
                        //    }
                        //    continue;
                        //}
                    } 

                    if(!IsAlreadyShot(shootField, x, y))
                    {
                        shootField[x, y] = 1;
                        break;
                    }
                } while (true);

                ShootResult sRes = gameField.Shoot(x, y);
                if (sRes == ShootResult.Hit)
                {
                    priorToLastDirection = lastDirection;

                    if(lastDirection == Direction.None)
                    {
                        lastDirection = (Direction)rnd.Next(1, 5);
                    }

                    lastHitPoint = new Point { X = x, Y = y };
                } 
                else if(sRes == ShootResult.DestroyShip)
                {
                    lastDirection = Direction.None;
                }

                Thread.Sleep(10);
            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();

        }

        private bool IsOutsideOfField(int x, int y)
        {
            return x < 0 || x >= fieldSize || y < 0 || y >= fieldSize;
        }

        private bool IsAlreadyShot(int[,] shootField, int x, int y)
        {
            return shootField[x, y] == 1;
        }
 
    }
}
