using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// Interface for Sudoku solving algorithms.
    /// </summary>
    /// <typeparam name="T">The type of data at each square of the board.</typeparam>
    public interface ISolver<T>
    {
        /// <summary>
        /// Solve the Sudoku board by filling in all of the missing values.
        /// </summary>
        /// <param name="board">The board to solve.</param>
        /// <returns>'true' if solved the board and 'false' if failed</returns>
        public bool Solve(SudokuBoard<T> board);
    }
}
