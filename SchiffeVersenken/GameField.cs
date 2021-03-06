﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken
{
    public abstract class GameField : IUpdateUI
    {
        public int FieldSize { get; }
        protected char[,,] field;
        protected int nrOfShots;
        protected int nrOfHits;
        protected int maxHits;
        protected bool lastShotWasHit;
        protected int lastShotKilledShip;
        protected Ship[] ships;

        private readonly Random rnd;

        // For debug purposes
        public bool ShowShips { get; set; }

        /// <summary>
        /// Construct a gamefield
        /// </summary>
        /// <param name="fieldSize">length and height of gamefield</param>
        /// <param name="noOfBattleships">Number of Battleships; default = 1</param>
        /// <param name="noOfCruisers">Number of Cruisers; default = 2</param>
        /// <param name="noOfDestroyers">Number of Destroyers; default = 3</param>
        /// <param name="noOfSubmarines">Number of Submarines; default = 4</param>
        public GameField(int fieldSize = 10, int noOfBattleships = 1, int noOfCruisers = 2, int noOfDestroyers = 3, int noOfSubmarines = 4)
        {
            rnd = new Random();
            this.FieldSize = fieldSize;
            field = new char[fieldSize, fieldSize,3];
            nrOfShots = 0;
            nrOfHits = 0;
            maxHits = 0;
            lastShotWasHit = false;
            lastShotKilledShip = -1;
            ShowShips = false;

            // Initialize field
            for (int y = 0; y < fieldSize; y++)
            {
                for (int x = 0; x < fieldSize; x++)
                {
                    field[x, y, 0] = '#';
                }
            }

            // Create and place ships
            ships = new Ship[noOfBattleships + noOfCruisers + noOfDestroyers + noOfSubmarines];
            SetRandomShips(noOfBattleships, noOfCruisers, noOfDestroyers, noOfSubmarines);
        }

        /// <summary>
        /// Attempt to shoot
        /// </summary>
        /// <param name="x">X-Coordinate to shoot at</param>
        /// <param name="y">Y-Coordinate to shoot at</param>
        /// <returns>true if ship is hit</returns>
        internal ShootResult Shoot(int x, int y)
        {
            this.nrOfShots++;
            // If was not already hit
            if(!field[x, y, 2].Equals('X'))
            {
                field[x, y, 2] = 'W';
                // if is a ship
                if (field[x,y,1] != 0)
                {
                    field[x, y, 2] = 'X';
                    nrOfHits++;
                    lastShotWasHit = true;
                    lastShotKilledShip = -1;

                    for (int i = 0; i < ships.Length; i++)
                    {
                        // checked all ships to see if the last shot sunk one
                        if(ships[i].ShootShip(x, y) && !ships[i].IsAlive)
                        {
                            // if so, set variable for ship destroyed notification
                            lastShotKilledShip = i;
                            return ShootResult.DestroyShip;
                        }
                    }
                    return ShootResult.ShipHit;
                }
            }

            lastShotWasHit = false;
            lastShotKilledShip = -1;
            return ShootResult.WaterHit;
        }

        /// <summary>
        /// Create ships and place to random  position
        /// </summary>
        /// <param name="noOfBattleships">Number of Battleships; default = 1</param>
        /// <param name="noOfCruisers">Number of Cruisers; default = 2</param>
        /// <param name="noOfDestroyers">Number of Destroyers; default = 3</param>
        /// <param name="noOfSubmarines">Number of Submarines; default = 4</param>
        public void SetRandomShips(int noOfBattleships, int noOfCruisers, int noOfDestroyers, int noOfSubmarines)
        {
            int createdShips = 0;

            for(int i=0; i<noOfBattleships; i++)
            {
                ships[createdShips++] = CreateRandomShip((int)ShipType.Battleship);
            }

            for (int i = 0; i < noOfCruisers; i++)
            {
                ships[createdShips++] = CreateRandomShip((int)ShipType.Cruiser);
            }

            for (int i = 0; i < noOfDestroyers; i++)
            {
                ships[createdShips++] = CreateRandomShip((int)ShipType.Destroyer);
            }

            for (int i = 0; i < noOfSubmarines; i++)
            {
                ships[createdShips++] = CreateRandomShip((int)ShipType.Submarine);
            }
        }

        /// <summary>
        /// Create ship an random Coordinates and
        /// check if it intersects with other ships
        /// </summary>
        /// <param name="size">Size of the ship</param>
        /// <returns>The placed ship</returns>
        private Ship CreateRandomShip(int size)
        {
            Ship returnShip;
            do
            {
                bool vertical = rnd.Next() % 2 == 0;
                int startX = rnd.Next(0, vertical ? FieldSize : FieldSize - size);
                int startY = rnd.Next(0, vertical ? FieldSize - size : FieldSize);
                int stopX = vertical ? startX : startX + size-1;
                int stopY = vertical ? startY + size-1 : startY;

                returnShip = new Ship(startX, startY, stopX, stopY);
                for (int i=0; i < ships.Length; i++)
                {
                    if(ships[i] != null && ships[i].IntersectsOrTouchesShip(returnShip))
                    {
                        returnShip = null;
                        break;
                    }
                }

            } while (returnShip == null);

            // Add created Ship to field
            for (int i = 0; i < returnShip.Points.Length; i++)
            {
                field[returnShip.Points[i].X, returnShip.Points[i].Y, 1] =  Convert.ToChar(size-1+"");
            }
            maxHits += size;

            return returnShip;
        }


        /// <summary>
        /// Check if game is over
        /// </summary>
        public bool PlayerHasWon(out double hitQuotient)
        {
            hitQuotient = 0;
            if (nrOfShots > 0) hitQuotient = (double)nrOfHits / nrOfShots;

            return maxHits == nrOfHits;
        }

        /// <summary>
        /// Print gamefield, fired shots and, if requested, the ships
        /// </summary>
        public abstract void UpdateField();

        /// <summary>
        /// Update current score
        /// </summary>
        public abstract void UpdateScore();

        public abstract void ShowHitMessage();

        public abstract void PlayerWonMessage();
    }
}
