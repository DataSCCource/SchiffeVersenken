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

        public Game(int fieldSize)
        {
            this.fieldSize = fieldSize;
        }

        public void Run()
        {
            //GameField gameField = new GameFieldConsole(fieldSize);
            //GameField gameField = new GameFieldConsole(20, 4, 8, 12, 16);

            // TODO: Remove
            GameField gameField = new GameFieldConsole(fieldSize, 0, 0, 0, 1)
            {
                ShowShips = true
            };

            do
            {
                gameField.UpdateField();
                Console.WriteLine(@"'x' oder 'q' zum Beenden");

                int x = Helper.GetIntInput("Bitte X-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.CheckExitInt(x);
                int y = Helper.GetIntInput("Bitte Y-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.CheckExitInt(y);

                gameField.Shoot(x, y);

            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();
        }
    }
}
