using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class BasicBacktrackingSolver<T> : ISolver<T>
    {
        public void Solve(SudokuBoard<T> board)
        {
            SolveSquare(board, 0);
        }

        private bool SolveSquare(SudokuBoard<T> board, int pos)
        {
            int boardWidth = board.BlockSideLength * board.BlockSideLength;

            if (pos >= boardWidth * boardWidth)
                return (new SetChecker<T>()).IsSolved(board);

            if (board[pos / boardWidth, pos % boardWidth].Equals(board.EmptyValue))
            {
                HashSet<T> possibilities = board.LegalValues.ToHashSet();
                possibilities.Remove(board.EmptyValue);

                // The number of row of the current block (for a 9 X 9 board can be 0, 1 or 2. if pos was 35 for example, it will be 1 (because second block-row))
                int blockRow = pos / (boardWidth * board.BlockSideLength);

                // The number of column of the current block (for a 9 X 9 board can be 0, 1 or 2. if pos was 35 for example, it will be 2 (because third block-column))
                int blockColumn = ((pos - blockRow) % boardWidth) / board.BlockSideLength; 

                for (int i = 0; i < boardWidth; i++)
                {
                    // Remove from block
                    possibilities.Remove(board[blockRow * board.BlockSideLength + (i / board.BlockSideLength),
                        blockColumn * board.BlockSideLength + (i % board.BlockSideLength)]);

                    // Remove from column
                    possibilities.Remove(board[i, pos % boardWidth]);

                    // Remove from row
                    possibilities.Remove(board[pos / boardWidth, i]);
                }

                foreach (T val in possibilities)
                {
                    board[pos / boardWidth, pos % boardWidth] = val;

                    if (SolveSquare(board, pos + 1))
                        return true;
                }

                board[pos / boardWidth, pos % boardWidth] = board.EmptyValue;
                return false;
            }
            else
                return SolveSquare(board, pos + 1);
        }
    }
}
