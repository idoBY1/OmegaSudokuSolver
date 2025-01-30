using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// Class for input and output through the console.
    /// </summary>
    public class ConsoleInteraction : IUserInteraction
    {
        public ConsoleInteraction()
        {
            Console.CancelKeyPress += Console_CancelKeyPressHandler;
        }

        private void Console_CancelKeyPressHandler(object? sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Terminated program.");
        }

        public void Print(string msg, bool endWithNewLine = true)
        {
            if (endWithNewLine)
                Console.WriteLine(msg);
            else
                Console.Write(msg);
        }

        public void PrintBoard<T>(SudokuBoard<T> board)
        {
            Console.WriteLine(board.GetStylizedString());
        }

        public void PrintBoardString<T>(SudokuBoard<T> board)
        {
            Console.WriteLine(board.ToString());
        }

        public SudokuBoard<T> ReadBoard<T>(T emptySquareObject, int blockSideLength, IEnumerable<T> legalValues)
        {
            if (emptySquareObject == null)
            {
                throw new ArgumentNullException(nameof(emptySquareObject));
            }

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
        /// Read an int Sudoku board from the console.
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
                    int currentInput = Console.Read();

                    if ((char)currentInput == '\r' || (char)currentInput == '\n')
                    {
                        Console.ReadLine(); // Clear input buffer.
                        throw new ReadBoardFailException($"Not enough characters to create a board. " +
                            $"Received only {strInput.Length} characters out of {board.Width * board.Width}.", strInput);
                    }

                    if (currentInput == -1)
                        throw new ReadBoardFailException($"Not enough characters to create a board. " +
                            $"Received only {strInput.Length} characters out of {board.Width * board.Width}.", strInput);

                    strInput += (char)currentInput;

                    try
                    {
                        board[i, j] = int.Parse(((char)currentInput).ToString());
                    }
                    catch (FormatException e)
                    {
                        Console.ReadLine(); // Clear input buffer.
                        throw new ReadBoardFailException($"Invalid symbol '{(char)currentInput}'.", strInput);
                    }
                }
            }

            Console.ReadLine(); // Clear input buffer.

            return board;
        }

        public SudokuBoard<char> ReadBoardAuto()
        {
            string userInput = Console.ReadLine();

            if (userInput == null || userInput.Equals(""))
                throw new ReadBoardFailException("Empty input.", "");

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

        public string? Read()
        {
            return Console.ReadLine();
        }
    }
}
