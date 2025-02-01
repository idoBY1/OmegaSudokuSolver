using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// Class for getting input from files and writing to files.
    /// </summary>
    public class FileInteraction : IUserInteraction
    {
        private string _input;
        private int _inputIndex;

        private string _outputFileName;

        public FileInteraction() { }

        public FileInteraction(string inputFileName, string outputFileName) 
        {
            if (File.Exists(inputFileName))
            {
                try
                {
                    _input = File.ReadAllText(inputFileName);
                    _inputIndex = 0;
                }
                catch 
                {
                    throw new IOException("Failed to read input file.");
                }
            }
            else
            {
                throw new IOException("Input file does not exist.");
            }

            _outputFileName = outputFileName;
        }

        /// <summary>
        /// Load the contents of the file in the given path and store them.
        /// </summary>
        /// <param name="inputFileName">The path to the input file.</param>
        /// <exception cref="IOException">Failed to read from input file.</exception>
        public void LoadFileContents(string inputFileName)
        {
            try
            {
                _input = File.ReadAllText(inputFileName);
                _inputIndex = 0;
            }
            catch
            {
                throw new IOException("Failed to read input file.");
            }
        }

        /// <summary>
        /// Skips the current whitespace in the file.
        /// </summary>
        public void SkipWhiteSpace()
        {
            while (_inputIndex < _input.Length && Char.IsWhiteSpace(_input[_inputIndex]))
                _inputIndex++;
        }

        /// <summary>
        /// Checks if finished reading the file.
        /// </summary>
        /// <returns>'true' if finished reading and 'false' if not.</returns>
        public bool IsFinished()
        {
            return _inputIndex >= _input.Length;
        }

        public void Print(string msg, bool endWithNewLine = true)
        {
            try
            {
                if (endWithNewLine)
                    msg += "\n";

                File.AppendAllText(_outputFileName, msg);
            }
            catch
            {
                throw new IOException("Failed to append to file.");
            }
        }

        public string Read()
        {
            return _input;
        }

        public void PrintBoard<T>(SudokuBoard<T> board)
        {
            try
            {
                File.AppendAllText(_outputFileName, board.GetStylizedString() + "\n");
            }
            catch
            {
                throw new IOException("Failed to append to file.");
            }
        }

        public void PrintBoardString<T>(SudokuBoard<T> board)
        {
            try
            {
                File.AppendAllText(_outputFileName, board.ToString());
            }
            catch
            {
                throw new IOException("Failed to append to file.");
            }
        }

        /// <summary>
        /// Deletes all of the content of the output file if already exists. <br/>
        /// If the file doesn't exist, creates a new empty file.
        /// </summary>
        public void ClearOutputFile()
        {
            File.WriteAllText(_outputFileName, "");
        }

        public SudokuBoard<T> ReadBoard<T>(T emptySquareObject, int blockSideLength, IEnumerable<T> legalValues)
        {
            if (emptySquareObject == null)
            {
                throw new ArgumentNullException(nameof(emptySquareObject));
            }

            SkipWhiteSpace();

            if (typeof(T) == typeof(int)) // Read a board of integers
            {
                // Create a new int Sudoku board
                return (SudokuBoard<T>)Convert.ChangeType(
                    ReadIntBoard((int)Convert.ChangeType(emptySquareObject, typeof(int)), blockSideLength, legalValues.Cast<int>()),
                    typeof(SudokuBoard<T>));
            }
            else
            {
                throw new ArgumentException("Type not supported");
            }
        }

        /// <summary>
        /// Read an int Sudoku board from a file.
        /// </summary>
        /// <param name="emptySquareNumber">The number that should be read as an empty square in the board.</param>
        /// <param name="blockSideLength">The length of a side of an inner block of the board. <br/>
        /// The final full board will be of size (blockSideLength^2 X blockSideLength^2).</param>
        /// <param name="legalValues">A Collection with all of the legal values a square can take.</param>
        /// <returns>An int SudokuBoard object</returns>
        /// <exception cref="ReadBoardFailException">Throws this exception if there was a problem while reading a value and assigning it to the board.</exception>
        private SudokuBoard<int> ReadIntBoard(int emptySquareNumber, int blockSideLength, IEnumerable<int> legalValues)
        {
            int boardSideLength = blockSideLength * blockSideLength;

            var board = new SudokuBoard<int>(blockSideLength, legalValues, emptySquareNumber);
            string strInput = "";

            for (int i = 0; i < boardSideLength; i++)
            {
                for (int j = 0; j < boardSideLength; j++)
                {
                    if (_input.Length <= _inputIndex)
                        throw new ReadBoardFailException($"Not enough characters to create a board. " +
                            $"Received only {strInput.Length} characters out of {board.Width * board.Width}.", strInput);

                    char currentInput = _input[_inputIndex++];

                    if (currentInput == '\r' || currentInput == '\n')
                    {
                        throw new ReadBoardFailException($"Not enough characters to create a board. " +
                            $"Received only {strInput.Length} characters out of {board.Width * board.Width}.", strInput);
                    }

                    strInput += currentInput;

                    try
                    {
                        board[i, j] = int.Parse(currentInput.ToString());
                    }
                    catch (FormatException e)
                    {
                        throw new ReadBoardFailException($"Invalid symbol '{currentInput}'.", strInput);
                    }
                }
            }

            return board;
        }

        public SudokuBoard<char> ReadBoardAuto()
        {
            var userInput = "";

            while (_inputIndex < _input.Length && (!Char.IsWhiteSpace(_input[_inputIndex])))
            { 
                userInput += _input[_inputIndex++];
            }

            if (userInput == null || userInput.Equals(""))
            {
                SkipWhiteSpace();
                throw new ReadBoardFailException("Empty input.", "");
            }

            int blockWidth = (int)Math.Sqrt((int)Math.Sqrt(userInput.Length));

            int boardWidth = blockWidth * blockWidth;

            var legalValues = new HashSet<char>();

            foreach (int i in Enumerable.Range('1', boardWidth))
            {
                legalValues.Add((char)i);
            }

            var board = new SudokuBoard<char>(blockWidth, legalValues, '0');

            for (int i = 0; i < boardWidth * boardWidth; i++)
            {
                if (userInput[i] == '0' || legalValues.Contains(userInput[i]))
                {
                    board.Set(i, userInput[i]);
                }
                else
                {
                    throw new ReadBoardFailException($"Invalid symbol '{userInput[i]}'", userInput);
                }
            }

            return board;
        }
    }
}
