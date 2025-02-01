using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// Class with helper functions for SudokuBoard usage.
    /// </summary>
    public class BoardUtils
    {
        public static SudokuBoard<int> CreateBoardFromString(string boardString)
        {
            int blockLength = (int)Math.Sqrt(Math.Sqrt(boardString.Length));

            var board = new SudokuBoard<int>(blockLength, Enumerable.Range(1, blockLength * blockLength), 0);

            for (int i = 0; i < board.Width * board.Width; i++)
            {
                board.Set(i, boardString[i] - '0');
            }

            return board;
        }
    }
}
