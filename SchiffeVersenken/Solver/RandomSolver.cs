using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken.Solver
{
    /// <summary>
    /// Solver, that shoots random without any search algorithm
    /// </summary>
    public class RandomSolver : AbstractSolver
    {
        public override double DoSimulationGame(GameField gameField, int updateEvery = -1)
        {
            fieldSize = gameField.FieldSize;
            shootField = new ShootResult[fieldSize, fieldSize];

            Random rnd = new Random();
            double hitQuotient;

            do
            {
                int x = rnd.Next(0, fieldSize);
                int y = rnd.Next(0, fieldSize);

                if(!IsPossibleTarget(new Point (x, y)))
                {
                    continue;
                }

                shootField[x, y] = gameField.Shoot(x, y);
                if (updateEvery != -1)
                {
                    gameField.UpdateField();
                    Thread.Sleep(updateEvery);
                }
            } while (!gameField.PlayerHasWon(out hitQuotient));

            gameField.PlayerWonMessage();

            return hitQuotient;
        }
    }
}
