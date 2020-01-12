using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            do
            {
                gameField.UpdateField();
                Console.WriteLine(@"'x' oder 'q' zum Beenden");

                int x = Helper.GetIntInput("Bitte X-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.CheckExitValue(x);
                int y = Helper.GetIntInput("Bitte Y-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.CheckExitValue(y);

                gameField.Shoot(x, y);

            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();
        }
    }
}
