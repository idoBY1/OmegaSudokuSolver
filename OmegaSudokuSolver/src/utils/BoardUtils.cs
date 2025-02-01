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
        public static SudokuBoard<char> CreateBoardFromString(string boardString)
        {
            int blockLength = (int)Math.Sqrt(Math.Sqrt(boardString.Length));

            var legalValues = new HashSet<char>();

            foreach (int i in Enumerable.Range('1', blockLength * blockLength))
            {
                legalValues.Add((char)i);
            }

            var board = new SudokuBoard<char>(blockLength, legalValues, '0');

            for (int i = 0; i < board.Width * board.Width; i++)
            {
                board.Set(i, boardString[i]);
            }

            return board;
        }
    }
}
