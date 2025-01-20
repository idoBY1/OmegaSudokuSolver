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

            SudokuBoard<int> board = io.ReadBoard(0, 3, Enumerable.Range(0, 10));

            io.Print("The board you entered: \n");
            io.PrintBoard(board);

            var solver = new RuleBasedSolver<int>();

            io.Print("Solving board...");

            Stopwatch sw = Stopwatch.StartNew();

            solver.Solve(board);

            sw.Stop();

            io.Print($"Finished.\nElapsed time: {sw.Elapsed.TotalMilliseconds} milliseconds\nResult: ");

            io.PrintBoard(board);

            var checker = new SetChecker<int>();

            io.Print($"Is board legal? {checker.IsLegal(board)}");
            io.Print($"Is board solved? {checker.IsSolved(board)}");
        }
    }
}