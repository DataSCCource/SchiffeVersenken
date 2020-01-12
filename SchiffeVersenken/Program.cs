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
            int fieldSize = Helper.GetIntInput("Bitte Feldgröße angeben ({min}-{max}) Standard ist {defaultValue}: ", 10, 20, 10);
            Helper.CheckExitValue(fieldSize);

            Game schiffeVersenken = new Game(fieldSize, true);
            schiffeVersenken.Run();
        }
    }
}
