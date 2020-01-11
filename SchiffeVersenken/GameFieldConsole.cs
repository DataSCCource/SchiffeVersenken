﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public class GameFieldConsole : GameField
    {
        public GameFieldConsole(int fieldSize = 10, int nrBs = 1, int nrCr = 2, int nrDest = 3, int nrSub = 4) 
            : base(fieldSize, nrBs, nrCr, nrDest, nrSub) { }


        /// <summary>
        /// Print gamefield, fired shots and, if requested, the ships
        /// </summary>
        public override void UpdateField()
        {
            Console.Clear();
            if (nrOfShots > 0) ShowHitMessage(); else Console.WriteLine("\n\n");
            UpdateScore();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");
            for (int i = 0; i < FieldSize; i++)
            {
                Console.Write(String.Format("{0,2}", i));
            }
            Console.WriteLine(" X");


            for (int y = 0; y < FieldSize; y++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0,3} ", y));
                for (int x = 0; x < FieldSize; x++)
                {
                    if (field[x, y, 2].Equals('W'))
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(field[x, y, 2]);
                        Console.Write(field[x, y, 2]);
                    }
                    else if (field[x, y, 2].Equals('X'))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(field[x, y, 2]);
                        Console.Write(field[x, y, 2]);
                    }
                    else if (field[x, y, 1] != 0 && ShowShips)
                    {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(field[x, y, 1]);
                        Console.Write(field[x, y, 1]);
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(field[x, y, 0]);
                        Console.Write(field[x, y, 0]);
                    }
                    //Console.BackgroundColor = ConsoleColor.Black;
                    //Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  Y");
            for (int i = 0; i < 2 * FieldSize + 4; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Print current score
        /// </summary>
        public override void UpdateScore()
        {
            double hitQuotient = 0;
            if (nrOfShots > 0) hitQuotient = (double)nrOfHits / nrOfShots;
            Console.WriteLine($"# Schüsse: {nrOfShots} | Treffer: {nrOfHits} | Trefferquote: {hitQuotient:P2}");
            Console.WriteLine();
        }

        public override void ShowHitMessage()
        {
            if (lastShotWasHit)
            {
                Console.WriteLine("* Treffer! :) *");
                if (lastShotKilledShip != -1)
                {
                    Console.WriteLine($"*** {ships[lastShotKilledShip].ShipType} zerstört! ***");
                } 
                else
                {
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Leider kein Treffer! ¯\\_(ツ)_/¯");
                
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public override void PlayerWonMessage()
        {
            UpdateField();
            Console.WriteLine("\nHerzlichen Glueckwunsch, du hast gewonnen! \\o/");
        }
    }
}