using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public class Player
    {
        private List<Point> shotsFired;
        private GameField gameField;

        public Player(GameField field)
        {
            gameField = field;
            shotsFired = new List<Point>();
        }

        public bool shoot(Point point)
        {


            return false;
        }
    }
}
