using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public interface IUserInteraction
    {
        /// <summary>
        /// Prints a message to the user.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        public void Print(string msg);

        /// <summary>
        /// Reads a Sudoku board from the user.
        /// </summary>
        /// <typeparam name="T">The type of data at each square of the board.</typeparam>
        /// <param name="emptySquareObject">The object that should be read as an empty square in the board.</param>
        /// <param name="blockSideLength">The length of a side of an inner block of the board. <br/>
        /// The final full board will be of size (blockSideLength^2 X blockSideLength^2).</param>
        /// <returns>A SudokuBoard object</returns>
        /// <exception cref="ArgumentException">Throws this if the type is not supported</exception>
        /// <exception cref="ArgumentNullException">Throws this exception if receives a null 'emptySquareObject'</exception>
        public SudokuBoard<T> ReadBoard<T>(T emptySquareObject, int blockSideLength);

        /// <summary>
        /// Print the board to the user.
        /// </summary>
        /// <typeparam name="T">The type of data at each square of the board.</typeparam>
        /// <param name="board">The board to print.</param>
        public void PrintBoard<T>(SudokuBoard<T> board);
    }
}
