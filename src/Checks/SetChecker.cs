using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void AssertLegal(SudokuBoard<T> board)
        {
            // Create a set for every group in the board.
            var blockSets = new HashSet<T>[board.BlockSideLength, board.BlockSideLength];
            var colsSets = new HashSet<T>[board.Width];
            var rowsSets = new HashSet<T>[board.Width];

            // Initialize sets.
            for (int i = 0; i < board.Width; i++)
            {
                blockSets[i / board.BlockSideLength, i % board.BlockSideLength] = new HashSet<T>();
                colsSets[i] = new HashSet<T>();
                rowsSets[i] = new HashSet<T>();
            }

            // For every square in the board.
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    int blockRow = i / board.BlockSideLength;

                    int blockColumn = j / board.BlockSideLength;

                    // Check if the block / row / column already contains the value of the square.
                    if (blockSets[blockRow, blockColumn].Contains(board[i, j]))
                    {
                        string blockStr = "";

                        for (int k = 0; k < board.BlockSideLength; k++)
                        {
                            for (int l = 0; l < board.BlockSideLength; l++)
                            {
                                blockStr += board[blockRow * board.BlockSideLength + k,
                                blockColumn * board.BlockSideLength + l].ToString() + " ";
                            }

                            blockStr += "\n";
                        }

                        throw new IllegalBoardException($"The value {board[i, j]} apears in a block more than one time. " +
                            $"The block: \n\n{blockStr}", i, j);
                    }

                    if (colsSets[j].Contains(board[i, j]))
                    {
                        string colStr = "";

                        for (int k = 0; k < board.Width; k++)
                        {
                            if (k % board.BlockSideLength == 0 && k != 0)
                                colStr += "---\n";

                            colStr += " " + board[k, j].ToString() + "\n";
                        }

                        throw new IllegalBoardException($"The value {board[i, j]} apears in a column more than one time. " +
                            $"The column: \n\n{colStr}", i, j);
                    }
                    
                    if (rowsSets[i].Contains(board[i, j]))
                    {
                        string rowStr = "";

                        for (int k = 0; k < board.Width; k++)
                        {
                            if (k % board.BlockSideLength == 0 && k != 0)
                                rowStr += "| ";

                            rowStr += board[i, k].ToString() + " ";
                        }

                        throw new IllegalBoardException($"The value {board[i, j]} apears in a row more than one time. " +
                            $"The row: \n\n{rowStr}\n", i, j);
                    }

                    // Check if the square contains a legal value.
                    if (!(board.LegalValues.Contains(board[i, j]) || board[i, j].Equals(board.EmptyValue)))
                    {
                        throw new IllegalBoardException("Illegal value.", i, j);
                    }

                    // If the square is not empty, add its value to his groups' sets.
                    if (!board[i, j].Equals(board.EmptyValue))
                    {
                        blockSets[i / board.BlockSideLength, j / board.BlockSideLength].Add(board[i, j]);
                        colsSets[j].Add(board[i, j]);
                        rowsSets[i].Add(board[i, j]);
                    }
                }
            }
        }

        public bool IsLegal(SudokuBoard<T> board)
        {
            // Create a set for every group in the board.
            var blockSets = new HashSet<T>[board.BlockSideLength, board.BlockSideLength];
            var colsSets = new HashSet<T>[board.Width];
            var rowsSets = new HashSet<T>[board.Width];

            // Initialize sets.
            for (int i = 0; i < board.Width; i++)
            {
                blockSets[i / board.BlockSideLength, i % board.BlockSideLength] = new HashSet<T>();
                colsSets[i] = new HashSet<T>();
                rowsSets[i] = new HashSet<T>();
            }

            // For every square in the board.
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    // Check if the block / row / column already contains the value of the square.
                    if (blockSets[i / board.BlockSideLength, j / board.BlockSideLength].Contains(board[i, j])
                        || colsSets[j].Contains(board[i, j])
                        || rowsSets[i].Contains(board[i, j]))
                        return false;

                    // Check if the square contains a legal value.
                    if (!(board.LegalValues.Contains(board[i, j]) || board[i, j].Equals(board.EmptyValue))) 
                        return false;

                    // If the square is not empty, add its value to his groups' sets.
                    if (!board[i, j].Equals(board.EmptyValue))
                    {
                        blockSets[i / board.BlockSideLength, j / board.BlockSideLength].Add(board[i, j]);
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
                blockSets[i / board.BlockSideLength, i % board.BlockSideLength] = new HashSet<T>();
                colsSets[i] = new HashSet<T>();
                rowsSets[i] = new HashSet<T>();
            }

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    if (board[i, j].Equals(board.EmptyValue))
                        return false;

                    if (blockSets[i / board.BlockSideLength, j / board.BlockSideLength].Contains(board[i, j])
                        || colsSets[j].Contains(board[i, j])
                        || rowsSets[i].Contains(board[i, j]))
                        return false;

                    if (!(board.LegalValues.Contains(board[i, j]) || board[i, j].Equals(board.EmptyValue)))
                        return false;

                    blockSets[i / board.BlockSideLength, j / board.BlockSideLength].Add(board[i, j]);
                    colsSets[j].Add(board[i, j]);
                    rowsSets[i].Add(board[i, j]);
                }
            }

            return true;
        }
    }
}
