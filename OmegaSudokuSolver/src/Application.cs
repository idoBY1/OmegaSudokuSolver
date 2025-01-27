using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class Application
    {
        private static readonly string EXIT_STR = "q";

        private IBoardChecker<char> _boardChecker;
        private ISolver<char> _solver;
        private IUserInteraction _consoleIO;

        public Application()
        {
            _boardChecker = new SetChecker<char>();
            _solver = new BitwiseSolver<char>();
            _consoleIO = new ConsoleInteraction();
        }

        public void Run()
        {
            SudokuBoard<char> board;

            _consoleIO.Print($"Enter boards to solve: (Enter '{EXIT_STR}' to quit)");

            while (true)
            {
                _consoleIO.Print(">>> ", false);

                try
                {
                    board = _consoleIO.ReadBoardAuto();
                }
                catch (ReadBoardFailException e)
                {
                    if (e.FailedInput.Equals(EXIT_STR))
                        break;

                    _consoleIO.Print($"Failed to read board from input: \"{e.FailedInput}\".");
                    _consoleIO.Print(e.Message);

                    continue;
                }

                if (board.BlockSideLength > 5)
                {
                    _consoleIO.Print("Please enter a board with a size up to 25 X 25");
                    continue;
                }

                _consoleIO.Print("The board you entered: \n");
                _consoleIO.PrintBoard(board);

                try
                {
                    _boardChecker.AssertLegal(board);
                }
                catch (IllegalBoardException e)
                {
                    _consoleIO.Print($"Illegal board. Found a problem at row {e.FailedRow} column {e.FailedColumn}.");
                    _consoleIO.Print(e.Message);

                    continue;
                }

                _consoleIO.Print("Solving board... ", false);

                Stopwatch sw = Stopwatch.StartNew();

                var solvedBoard = _solver.Solve(board);

                sw.Stop();

                _consoleIO.Print($"Finished.\nElapsed time: {sw.Elapsed.TotalSeconds} seconds.");

                if (solvedBoard != null)
                {
                    _consoleIO.Print("Solved board: \n");
                    _consoleIO.PrintBoard(solvedBoard);

                    _consoleIO.Print($"Board as a character sequence: ", false);
                    _consoleIO.PrintBoardString(solvedBoard);
                    _consoleIO.Print("");
                }
                else
                {
                    _consoleIO.Print("Unsolvable board.");
                }
            }

            _consoleIO.Print("Exited program.");
        }
    }
}
