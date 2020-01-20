using SchiffeVersenken.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken
{
    public class Game
    {
        private readonly int fieldSize;
        private readonly int nrOfRounds;
        private readonly bool testMode;

        /// <summary>
        /// Create a game of "Schiffe versenken"
        /// </summary>
        /// <param name="fieldSize">Size of the square field</param>
        /// <param name="testMode">Optional: Testmode only sets one submarine and shows it on the gamefield</param>
        public Game(int fieldSize, int nrOfRounds, bool testMode = false)
        {
            this.fieldSize = fieldSize;
            this.nrOfRounds = nrOfRounds;
            this.testMode = testMode;
        }

        /// <summary>
        /// Do a game of "Schiffe versenken"
        /// </summary>
        public void Run()
        {
            if(nrOfRounds > 0)
            {
                List<double> hitQuotients = new List<double>();
                for (int i = 1; i <= nrOfRounds; i++)
                {
                    /*
                    AbstractSolver solver = new HuntSolver();
                    AbstractSolver solver = new RandomSolver();
                     */
                    AbstractSolver solver = new FullSolver();
                    hitQuotients.Add(solver.DoSimulationGame(CreateGameField(), nrOfRounds == 1 ? 100 : -1));

                    Console.WriteLine();
                    Console.WriteLine($"### Durchschnittliche Trefferquote: {hitQuotients.Average():P2} (in {i}/{nrOfRounds} Runden) ###");
                    Thread.Sleep(10);
                }
            } else
            {
                DoHumanGame(CreateGameField());
            }
        }

        /// <summary>
        /// Create a default gamefield object
        /// </summary>
        /// <returns></returns>
        private GameField CreateGameField()
        {
                 //new GameFieldConsole(20, 4, 8, 12, 16) { ShowShips = testMode };
            return new GameFieldConsole(fieldSize) { ShowShips = testMode };
        }

        /// <summary>
        /// Starts a manual game played by a human
        /// </summary>
        /// <param name="gameField"></param>
        private void DoHumanGame(GameField gameField)
        {
            do
            {
                gameField.UpdateField();
                Console.WriteLine(@"'x' oder 'q' zum Beenden");

                int x = Helper.GetIntInput("Bitte X-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.ExitOnExitValue(x);
                int y = Helper.GetIntInput("Bitte Y-Koordinate eingeben: ", 0, fieldSize - 1);
                Helper.ExitOnExitValue(y);

                gameField.Shoot(x, y);

            } while (!gameField.PlayerHasWon(out _));

            gameField.PlayerWonMessage();
        }

    }
}
