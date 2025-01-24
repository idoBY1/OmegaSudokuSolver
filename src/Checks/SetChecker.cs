using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// Class for checking the board using sets.
    /// </summary>
    /// <typeparam name="T">The type of data at each square of the board.</typeparam>
    public class SetChecker<T> : IBoardChecker<T>
    {
        public bool IsFull(SudokuBoard<T> board)
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    if (board[i, j].Equals(board.EmptyValue))
                        return false;
                }
            }

            return true;
        }

        public bool IsLegal(SudokuBoard<T> board)
        {
            var blockSets = new HashSet<T>[board.BlockSideLength, board.BlockSideLength];
            var colsSets = new HashSet<T>[board.Width];
            var rowsSets = new HashSet<T>[board.Width];

            for (int i = 0; i < board.Width; i++)
            {
                blockSets[i / 3, i % 3] = new HashSet<T>();
                colsSets[i] = new HashSet<T>();
                rowsSets[i] = new HashSet<T>();
            }

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    if (blockSets[i / 3, j / 3].Contains(board[i, j])
                        || colsSets[j].Contains(board[i, j])
                        || rowsSets[i].Contains(board[i, j]))
                        return false;

                    if (!(board.LegalValues.Contains(board[i, j]) || board[i, j].Equals(board.EmptyValue))) 
                        return false;

                    if (!board[i, j].Equals(board.EmptyValue))
                    {
                        blockSets[i / 3, j / 3].Add(board[i, j]);
                        colsSets[j].Add(board[i, j]);
                        rowsSets[i].Add(board[i, j]);
                    }
                }
            }

            return true;
        }

        public bool IsSolved(SudokuBoard<T> board)
        {
            var blockSets = new HashSet<T>[board.BlockSideLength, board.BlockSideLength];
            var colsSets = new HashSet<T>[board.Width];
            var rowsSets = new HashSet<T>[board.Width];

            for (int i = 0; i < board.Width; i++)
            {
                blockSets[i / 3, i % 3] = new HashSet<T>();
                colsSets[i] = new HashSet<T>();
                rowsSets[i] = new HashSet<T>();
            }

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    if (board[i, j].Equals(board.EmptyValue))
                        return false;

                    if (blockSets[i / 3, j / 3].Contains(board[i, j])
                        || colsSets[j].Contains(board[i, j])
                        || rowsSets[i].Contains(board[i, j]))
                        return false;

                    if (!(board.LegalValues.Contains(board[i, j]) || board[i, j].Equals(board.EmptyValue)))
                        return false;

                    blockSets[i / 3, j / 3].Add(board[i, j]);
                    colsSets[j].Add(board[i, j]);
                    rowsSets[i].Add(board[i, j]);
                }
            }

            return true;
        }
    }
}
