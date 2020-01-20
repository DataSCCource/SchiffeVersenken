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
            //Console.OutputEncoding = System.Text.Encoding.UTF8;

            do
            {
                int fieldSize = Helper.GetIntInput("Bitte Feldgröße angeben ({min}-{max}) Standard ist {defaultValue}: ", 10, 20, 10);
                Helper.ExitOnExitValue(fieldSize);

                int nrOfRounds = Helper.GetIntInput("Spielen (0) oder automatisch lösen lassen (1-1000 Runden)? ", 0, 1000, 0);

                Game schiffeVersenken = new Game(fieldSize, nrOfRounds);
                schiffeVersenken.Run();
            } while (true);
        }
    }
}
