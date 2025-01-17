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
            throw new NotImplementedException();
        }

        public bool IsSolved(SudokuBoard<T> board)
        {
            throw new NotImplementedException();
        }
    }
}
