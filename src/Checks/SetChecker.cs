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
        public bool IsLegal(SudokuBoard<T> board)
        {
            int boardWidth = board.BlockSideLength * board.BlockSideLength;

            var blockSets = new HashSet<T>[board.BlockSideLength, board.BlockSideLength];
            var colsSets = new HashSet<T>[boardWidth];
            var rowsSets = new HashSet<T>[boardWidth];

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    if (blockSets[i / 3, j / 3].Contains(board[i, j])
                        || colsSets[j].Contains(board[i, j])
                        || rowsSets[i].Contains(board[i, j]))
                        return false;

                    if (!board.LegalValues.Contains(board[i, j])) 
                        return false;

                    blockSets[i / 3, j / 3].Add(board[i, j]);
                    colsSets[j].Add(board[i, j]);
                    rowsSets[i].Add(board[i, j]);
                }
            }

            return true;
        }

        public bool IsSolved(SudokuBoard<T> board)
        {
            throw new NotImplementedException();
        }
    }
}
