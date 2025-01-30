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

        private static readonly string DEFAULT_OUTPUT_FILE_NAME = "board_solutions.txt";

        private IBoardChecker<char> _boardChecker;
        private ISolver<char> _solver;
        private IUserInteraction _mainIO;

        public Application()
        {
            _boardChecker = new SetChecker<char>();
            _solver = new BitwiseSolver<char>();
            _mainIO = new ConsoleInteraction();
        }

        public void Run()
        {
            _mainIO.Print("Would you like to enter Sudoku boards through the console or through a file? (type 'f' for file and 'c' for console)\n>>> ", false);

            string? userInputMessage = _mainIO.Read();

            if (userInputMessage != null)
            {
                if (userInputMessage.ToLower().StartsWith("c"))
                    ConsoleInOut();
                else if (userInputMessage.ToLower().StartsWith("f"))
                    FileInOut();
                else
                    _mainIO.Print("Invalid input source.");
            }

            _mainIO.Print("Exited program.");
        }

        private void ConsoleInOut()
        {
            SudokuBoard<char> board;

            _mainIO.Print($"\nEnter boards to solve: (Enter '{EXIT_STR}' to quit)");

            while (true)
            {
                _mainIO.Print(">>> ", false);

                try
                {
                    board = _mainIO.ReadBoardAuto();
                }
                catch (ReadBoardFailException e)
                {
                    if (e.FailedInput.ToLower().Equals(EXIT_STR))
                        break;

                    _mainIO.Print($"Failed to read board from input: \"{e.FailedInput}\".");
                    _mainIO.Print(e.Message);

                    continue;
                }

                if (board.BlockSideLength > 5)
                {
                    _mainIO.Print("Please enter a board with a size up to 25 X 25");
                    continue;
                }

                _mainIO.Print("The board you entered: \n");
                _mainIO.PrintBoard(board);

                try
                {
                    _boardChecker.AssertLegal(board);
                }
                catch (IllegalBoardException e)
                {
                    _mainIO.Print($"Illegal board. Found a problem at row {e.FailedRow} column {e.FailedColumn}.");
                    _mainIO.Print(e.Message);

                    continue;
                }

                _mainIO.Print("Solving board... ", false);

                Stopwatch sw = Stopwatch.StartNew();

                var solvedBoard = _solver.Solve(board);

                sw.Stop();

                _mainIO.Print($"Finished.\nElapsed time: {sw.Elapsed.TotalSeconds} seconds.");

                if (solvedBoard != null)
                {
                    _mainIO.Print("Solved board: \n");
                    _mainIO.PrintBoard(solvedBoard);

                    _mainIO.Print($"Board as a character sequence: ", false);
                    _mainIO.PrintBoardString(solvedBoard);
                    _mainIO.Print("");
                }
                else
                {
                    _mainIO.Print("Unsolvable board.");
                }
            }
        }

        private void FileInOut()
        {
            _mainIO.Print("\nPlease enter the input file name. \n>>> ", false);

            string inputFilePath = _mainIO.Read();

            if (!File.Exists(inputFilePath))
            {
                _mainIO.Print("This file doesn't exist.");
                return;
            }

            _mainIO.Print($"Reading from '{inputFilePath}'.");

            _mainIO.Print($"\nPlease enter the output file name. (for the default name '{DEFAULT_OUTPUT_FILE_NAME}' leave this field empty)\n>>> ", false);

            string outputFilePath = _mainIO.Read();

            if (outputFilePath == null || outputFilePath.Length == 0) 
                outputFilePath = DEFAULT_OUTPUT_FILE_NAME;

            _mainIO.Print($"Writing to '{outputFilePath}'.\n");

            var fileIO = new FileInteraction(inputFilePath, outputFilePath);
            fileIO.ClearOutputFile();

            SudokuBoard<char> board;

            fileIO.SkipWhiteSpace();

            while (!fileIO.IsFinished())
            {
                try
                {
                    board = fileIO.ReadBoardAuto();
                }
                catch (ReadBoardFailException e)
                {
                    if (e.FailedInput.ToLower().Equals(EXIT_STR))
                        break;

                    _mainIO.Print($"Failed to read board from input: \"{e.FailedInput}\".");
                    _mainIO.Print(e.Message);

                    continue;
                }

                if (board.BlockSideLength > 5)
                {
                    _mainIO.Print("Max board size is 25 X 25, ignoring board...");
                    continue;
                }

                _mainIO.Print("Board read from file. The board: \n");
                _mainIO.PrintBoard(board);

                try
                {
                    _boardChecker.AssertLegal(board);
                }
                catch (IllegalBoardException e)
                {
                    _mainIO.Print($"Illegal board. Found a problem at row {e.FailedRow} column {e.FailedColumn}.");
                    _mainIO.Print(e.Message);

                    continue;
                }

                _mainIO.Print("Solving board... ", false);

                Stopwatch sw = Stopwatch.StartNew();

                var solvedBoard = _solver.Solve(board);

                sw.Stop();

                _mainIO.Print($"Finished.\nElapsed time: {sw.Elapsed.TotalSeconds} seconds.");

                if (solvedBoard != null)
                {
                    _mainIO.Print("Solved board: \n");
                    _mainIO.PrintBoard(solvedBoard);

                    fileIO.PrintBoardString(solvedBoard);
                    fileIO.Print("\n");
                }
                else
                {
                    _mainIO.Print("Unsolvable board.");

                    fileIO.PrintBoardString(board);
                    fileIO.Print(" (unsolvable board)\n");
                }

                fileIO.SkipWhiteSpace();
            }
        }
    }
}
