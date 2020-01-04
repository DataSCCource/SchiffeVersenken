﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public class GameField
    {
        private char[,,] field;
        private int fieldSize;

        private Ship[] ships;
        private Random rnd;

        private bool showShips = false;


        public GameField(int fieldSize = 10, int nrBs = 1, int nrCr = 2, int nrDest = 3, int nrSub = 4)
        {
            rnd = new Random();
            this.fieldSize = fieldSize;
            field = new char[fieldSize, fieldSize,3];

            for (int y = 0; y < fieldSize; y++)
            {
                for (int x = 0; x < fieldSize; x++)
                {
                    field[x, y, 0] = '#';
                }
            }

            ships = new Ship[nrBs + nrCr + nrDest + nrSub];
            SetRandomShips(nrBs, nrCr, nrDest, nrSub);
        }

        public void SetRandomShips(int nrBs, int nrCr, int nrDest, int nrSub)
        {
            int createdShips = 0;

            for(int i=0; i<nrBs; i++)
            {
                ships[createdShips] = CreateRandomBattleship();
                createdShips++;
            }

            for (int i = 0; i < nrCr; i++)
            {
                ships[createdShips] = CreateRandomCruiser();
                createdShips++;
            }

            for (int i = 0; i < nrDest; i++)
            {
                ships[createdShips] = CreateRandomDestroyer();
                createdShips++;
            }

            for (int i = 0; i < nrSub; i++)
            {
                ships[createdShips] = CreateRandomSubmarine();
                createdShips++;
            }
        }

        private Ship CreateRandomShip(int size)
        {
            Ship returnShip = null;
            do
            {
                bool vertical = rnd.Next() % 2 == 0;
                int startX = rnd.Next(0, vertical ? fieldSize : fieldSize - size);
                int startY = rnd.Next(0, vertical ? fieldSize - size : fieldSize);
                int stopX = vertical ? startX : startX + size-1;
                int stopY = vertical ? startY + size-1 : startY;

                returnShip = new Ship(startX, startY, stopX, stopY);
                for (int i=0; i < ships.Length; i++)
                {
                    if(ships[i] != null)
                    {                    
                        if(ships[i].IntersectsShip(returnShip))
                        {
                            returnShip = null;
                            break;
                        }
                    }
                }

            } while (returnShip == null);

            returnShip.SetShipOnField(field);

            return returnShip;
        }

        private Ship CreateRandomBattleship()
        {
            return CreateRandomShip(5);
        }

        private Ship CreateRandomCruiser()
        {
            return CreateRandomShip(4);
        }

        private Ship CreateRandomDestroyer()
        {
            return CreateRandomShip(3);
        }

        private Ship CreateRandomSubmarine()
        {
            return CreateRandomShip(2);
        }


        public void PrintField()
        {
            Console.Write("   ");
            for (int i = 0; i < fieldSize; i++)
            {
                Console.Write(i<10?" ":"");
                Console.Write(i);
            }
            Console.WriteLine();


            for (int y = 0; y < fieldSize; y++)
            {
                //Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0,3} ", y));
                for (int x = 0; x < fieldSize; x++)
                {
                    if(field[x, y, 2].Equals('X'))
                    {
                        //Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(field[x, y, 1]);
                    }
                    else if(field[x, y, 1].Equals('O') && showShips)
                    {
                        //Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(field[x, y, 1]);
                    } else
                    {
                        //Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(field[x, y, 0]);
                    }
//                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public bool IsPointInShip(Point point)
        {
            for(int i=0; i<ships.Length; i++)
            {
                if(ships[i].IntersectsShip(point))
                {
                    return true;
                }
            }

            return false;
        }

        public void PrintHorizentalLine()
        {
            for(int i = 0; i<2* fieldSize + 4; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine();
        }
    }
}
