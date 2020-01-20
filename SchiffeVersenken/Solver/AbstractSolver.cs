using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken.Solver
{
    public abstract class AbstractSolver
    {
        private protected int fieldSize;
        protected ShootResult[,] shootField;

        public abstract double DoSimulationGame(GameField gameField, int updateEvery = -1);

        /// <summary>
        /// Return true if given Point is located in a vertical oriented ship,
        /// meaning there is another hit above or below the given Point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        protected bool IsVerticalShip(Point point)
        {
            Point up = GetPointOf(point, Direction.Up);
            Point down = GetPointOf(point, Direction.Down);

            return !IsOutsideOfField(up) && shootField[up.X, up.Y] >= ShootResult.ShipHit
                || !IsOutsideOfField(down) && shootField[down.X, down.Y] >= ShootResult.ShipHit;
        }

        /// <summary>
        /// Return true if given Point is located in a horizontal oriented ship.
        /// meaning there is another hit left or right of the given Point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        protected bool IsHorizontalShip(Point point)
        {
            Point left = GetPointOf(point, Direction.Left);
            Point right = GetPointOf(point, Direction.Right);

            return !IsOutsideOfField(left) && shootField[left.X, left.Y] >= ShootResult.ShipHit
                || !IsOutsideOfField(right) && shootField[right.X, right.Y] >= ShootResult.ShipHit;
        }

        /// <summary>
        /// Get a Point that is above, below, right or left of the given Point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction">Direction.[Up|Down|Left|Right]</param>
        /// <returns></returns>
        protected Point GetPointOf(Point point, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point(point.X, point.Y - 1);
                case Direction.Left:
                    return new Point(point.X - 1, point.Y);
                case Direction.Down:
                    return new Point(point.X, point.Y + 1);
                case Direction.Right:
                    return new Point(point.X + 1, point.Y);
                default:
                    return Point.INVALID;
            }
        }

        /// <summary>
        /// Checks if Point is inside gamefield and was not already shot
        /// </summary>
        /// <param name="guessPoint"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        protected bool IsPossibleTarget(Point guessPoint)
        {
            return !IsOutsideOfField(guessPoint) && !WasAlreadyShot(guessPoint);
        }

        /// <summary>
        /// Returns true if position of given Point was already shot (hit or miss)
        /// </summary>
        /// <param name="guessPoint"></param>
        /// <param name="shootField"></param>
        /// <returns></returns>
        protected bool WasAlreadyShot(Point guessPoint)
        {
            return shootField[guessPoint.X, guessPoint.Y] != ShootResult.None;
        }

        /// <summary>
        /// Returns true if given Point is outside of the gamefield
        /// </summary>
        /// <param name="guessPoint"></param>
        /// <returns></returns>
        protected bool IsOutsideOfField(Point guessPoint)
        {
            return guessPoint.X < 0 || guessPoint.X >= fieldSize || guessPoint.Y < 0 || guessPoint.Y >= fieldSize;
        }

    }
}
