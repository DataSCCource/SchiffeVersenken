using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    class Program
    {
        static void Main(string[] args)
        {
            //GameField gameField = new GameFieldConsole();
            GameField gameField = new GameFieldConsole(10, 1, 1, 1, 1);
            //GameField gameField = new GameFieldConsole(20, 4, 8, 12, 16);

            // TODO: Remove
            gameField.ShowShips = true;

            do
            {
                gameField.UpdateField();

                int x = gameField.GetIntInput("Bitte X-Koordinate eingeben: ");
                int y = gameField.GetIntInput("Bitte Y-Koordinate eingeben: ");

                gameField.Shoot(x, y);

            } while (!gameField.PlayerHasWon());

            gameField.PlayerWonMessage();


        }
    }
}
