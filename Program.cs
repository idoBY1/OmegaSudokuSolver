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
            var board = new SudokuBoard<int>(new int[,] {
                { 0, 1, 0, 4 },
                { 1, 2, 0, 4 },
                { 0, 0, 0, 0 },
                { 1, 3, 0, 0 }
            });

            Console.WriteLine(board.GetStylizedString());
        }
    }
}