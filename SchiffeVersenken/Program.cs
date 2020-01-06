using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    class Program
    {
        static void Main()
        {
            int fieldSize = Helper.GetIntInput("Bitte Feldgröße angeben ({min}-{max}) Standard ist {defaultValue}: ", 10, 20, 10);

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

                int x = Helper.GetIntInput("Bitte X-Koordinate eingeben: ", 0, fieldSize - 1);
                int y = Helper.GetIntInput("Bitte Y-Koordinate eingeben: ", 0, fieldSize - 1);

                gameField.Shoot(x, y);

            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();
        }
    }
}
