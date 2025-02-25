﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// A basic recursive sudoku solver. The solver guesses a value for a square and checks if the guess leads <br/>
    /// to a solved board. If it doesn't, the solver tries another value for that square and checks again until <br/>
    /// it succeeds or runs out of options. This solver is slower than both RuleBasedSolver and BitwiseSolver.
    /// </summary>
    /// <typeparam name="T">The type of data at each square of the board.</typeparam>
    public class BasicBacktrackingSolver<T> : ISolver<T>
    {
        public SudokuBoard<T> Solve(SudokuBoard<T> board)
        {
            return SolveSquare(board, 0);
        }

        /// <summary>
        /// A recursive function for solving the board square by square.
        /// </summary>
        /// <param name="board">The board to solve.</param>
        /// <param name="pos">The position of the current square in the board from the upper left corner.</param>
        /// <returns>'true' if the board is solved and 'false' if it's not.</returns>
        private SudokuBoard<T> SolveSquare(SudokuBoard<T> board, int pos)
        {
            if (pos >= board.Width * board.Width)
            {
                if ((new SetChecker<T>()).IsSolved(board))
                    return board;
                else
                    return null;
            }
                

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

                    var result = SolveSquare(board, pos + 1);

                    if (result != null)
                        return result;
                }

                board[pos / board.Width, pos % board.Width] = board.EmptyValue;
                return null;
            }
            else
                return SolveSquare(board, pos + 1);
        }
    }
}
