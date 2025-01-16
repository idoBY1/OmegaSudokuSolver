using OmegaSudokuSolver.src;
using System;
using System.Collections.Generic;
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

            SudokuBoard<int> board = io.ReadBoard<int>(0, 3);

            io.Print("The board you entered: \n");

            io.PrintBoard(board);
        }
    }
}