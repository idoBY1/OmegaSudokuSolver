using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var io = new ConsoleInteraction();

            io.Print("Enter a board: ");

            SudokuBoard<int> board = io.ReadBoard(0, 3, Enumerable.Range(1, 9));

            io.Print("The board you entered: \n");
            io.PrintBoard(board);

            var solver = new BitwiseSolver<int>();

            io.Print("Solving board...");

            Stopwatch sw = Stopwatch.StartNew();

            var solvedBoard = solver.Solve(board);

            sw.Stop();

            io.Print($"Finished.\nElapsed time: {sw.Elapsed.TotalMilliseconds} milliseconds\nResult: ");

            if (solvedBoard != null)
            {
                io.PrintBoard(solvedBoard);

                var checker = new SetChecker<int>();

                io.Print($"Is board legal? {checker.IsLegal(solvedBoard)}");
                io.Print($"Is board solved? {checker.IsSolved(solvedBoard)}");
            }
            else
            {
                io.Print("Failed to solve board");
            }
        }
    }
}