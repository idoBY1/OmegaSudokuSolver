using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

            io.Print("The board string: ");
            io.PrintBoardString(board);
        }
    }
}