using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class ReadBoardFailException : Exception
    {
        public string FailedInput { get; }

        public ReadBoardFailException(string message, string failedInput) : base(message)
        {
            FailedInput = failedInput;
        }
    }
}
