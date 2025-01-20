using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class BasicBacktrackingSolver<T> : ISolver<T>
    {
        public bool Solve(SudokuBoard<T> board)
        {
            return SolveSquare(board, 0);
        }

        /// <summary>
        /// A recursive function for solving the board square by square.
        /// </summary>
        /// <param name="board">The board to solve.</param>
        /// <param name="pos">The position of the current square in the board from the upper left corner.</param>
        /// <returns>'true' if the board is solved and 'false' if it's not.</returns>
        private bool SolveSquare(SudokuBoard<T> board, int pos)
        {
            if (pos >= board.Width * board.Width)
                return (new SetChecker<T>()).IsSolved(board);

            if (board[pos / board.Width, pos % board.Width].Equals(board.EmptyValue))
            {
                HashSet<T> possibilities = board.LegalValues.ToHashSet();
                possibilities.Remove(board.EmptyValue);

                // The number of row of the current block (for a 9 X 9 board can be 0, 1 or 2. if pos was 35 for example, it will be 1 (because second block-row))
                int blockRow = pos / (board.Width * board.BlockSideLength);

                // The number of column of the current block (for a 9 X 9 board can be 0, 1 or 2. if pos was 35 for example, it will be 2 (because third block-column))
                int blockColumn = (pos % board.Width) / board.BlockSideLength;

                for (int i = 0; i < board.Width; i++)
                {
                    // Remove from block
                    possibilities.Remove(board[blockRow * board.BlockSideLength + (i / board.BlockSideLength),
                        blockColumn * board.BlockSideLength + (i % board.BlockSideLength)]);

                    // Remove from column
                    possibilities.Remove(board[i, pos % board.Width]);

                    // Remove from row
                    possibilities.Remove(board[pos / board.Width, i]);
                }

                foreach (T val in possibilities)
                {
                    board[pos / board.Width, pos % board.Width] = val;

                    if (SolveSquare(board, pos + 1))
                        return true;
                }

                board[pos / board.Width, pos % board.Width] = board.EmptyValue;
                return false;
            }
            else
                return SolveSquare(board, pos + 1);
        }
    }
}
