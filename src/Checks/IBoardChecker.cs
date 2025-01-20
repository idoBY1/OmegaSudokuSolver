using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// Interface for checking a Sudoku board.
    /// </summary>
    /// <typeparam name="T">The type of data at each square of the board.</typeparam>
    public interface IBoardChecker<T>
    {
        /// <summary>
        /// Check if the board is legal. A board is illegal if it has an illegal object <br/>
        /// or if it has at least two equal objects in the same row, column or inner <br/>
        /// block. 
        /// </summary>
        /// <param name="board">The board to check.</param>
        /// <returns>'true' if the board is legal and 'false' if it's not.</returns>
        public bool IsLegal(SudokuBoard<T> board);

        /// <summary>
        /// Check if the board is solved. A board is solved if it's legal <br/>
        /// and it has no empty squares left to fill.
        /// </summary>
        /// <param name="board">The board to check.</param>
        /// <returns>'true' if the board is solved and 'false' if it's not.</returns>
        public bool IsSolved(SudokuBoard<T> board);
    }
}
