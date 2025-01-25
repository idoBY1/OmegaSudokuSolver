using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class IllegalBoardException : Exception
    {
        public int FailedRow { get; }
        public int FailedColumn { get; }

        public IllegalBoardException(string message, int failedRow, int failedColumn) : base(message) 
        {
            FailedRow = failedRow;
            FailedColumn = failedColumn;
        }
    }
}
