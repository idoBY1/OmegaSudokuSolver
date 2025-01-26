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
        private IUserInteraction _io;

        public Application()
        {
            _boardChecker = new SetChecker<char>();
            _solver = new BitwiseSolver<char>();
            _io = new ConsoleInteraction();
        }

        public void Run()
        {
            SudokuBoard<char> board;

            _io.Print($"Enter boards to solve: (Enter '{EXIT_STR}' to quit)");

            while (true)
            {
                _io.Print(">>> ", false);

                try
                {
                    board = _io.ReadBoardAuto();
                }
                catch (ReadBoardFailException e)
                {
                    if (e.FailedInput.Equals(EXIT_STR))
                        break;

                    _io.Print($"Failed to read board from input: \"{e.FailedInput}\".");
                    _io.Print(e.Message);

                    continue;
                }

                _io.Print("The board you entered: \n");
                _io.PrintBoard(board);

                try
                {
                    _boardChecker.AssertLegal(board);
                }
                catch (IllegalBoardException e)
                {
                    _io.Print($"Illegal board. Found a problem at row {e.FailedRow} column {e.FailedColumn}.");
                    _io.Print(e.Message);

                    continue;
                }

                _io.Print("Solving board... ", false);

                Stopwatch sw = Stopwatch.StartNew();

                var solvedBoard = _solver.Solve(board);

                sw.Stop();

                _io.Print($"Finished.\nElapsed time: {sw.Elapsed.TotalSeconds} seconds.");

                if (solvedBoard != null)
                {
                    _io.Print("Solved board: \n");
                    _io.PrintBoard(solvedBoard);

                    _io.Print($"Board as a character sequence: ", false);
                    _io.PrintBoardString(solvedBoard);
                    _io.Print("");
                }
                else
                {
                    _io.Print("Unsolvable board.");
                }
            }

            _io.Print("Exited program.");
        }
    }
}
