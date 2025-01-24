using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class BitsSolveUtils
    {
        // Array for conversion from nible to amount of bits activated.
        private static readonly int[] num_to_bits = new int[16] { 0, 1, 1, 2, 1, 2, 2,
                                             3, 1, 2, 2, 3, 2, 3, 3, 4 };

        /// <summary>
        /// Counts the amount of activated bits in the value.
        /// </summary>
        /// <param name="bits">The value to count the activated bits from.</param>
        /// <returns>The number of activated bits.</returns>
        public static int CountActivatedBits(int bits)
        {
            int count = 0;

            while (bits != 0)
            {
                count += num_to_bits[bits & 0x0f];
                bits >>= 4;
            }

            return count;
        }

        /// <summary>
        /// Returns the position of the first activated bit (from the right).
        /// </summary>
        /// <param name="bits">The value to find the bit in.</param>
        /// <returns>The index from the right of the number where the bit was found. Returns -1 if no bits are activated.</returns>
        public static int FirstActivatedBitPos(int bits)
        {
            int pos = -1;

            while (bits != 0)
            {
                pos++;

                if ((bits & 1) != 0)
                    break;

                bits >>= 1;
            }

            return pos;
        }

        /// <summary>
        /// Converts a SudokuBoard to a SudokuBoard containing bits as values. <br/>
        /// Built to work with bitwise notes.
        /// </summary>
        /// <typeparam name="T">The type of data at each square of the board.</typeparam>
        /// <param name="board">The board to convert.</param>
        /// <returns>The converted board.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static SudokuBoard<int> ConvertBoardToBitwise<T>(SudokuBoard<T> board)
        {
            if (board.LegalValues.Count > 32)
                throw new ArgumentException("Board values must fit into a 32 bit representation.");

            List<int> bitValues = new List<int>();

            for (int i = 0; i < board.LegalValues.Count; i++)
            {
                bitValues.Add(1 << i);
            }

            var bitwiseBoard = new SudokuBoard<int>(board.BlockSideLength, bitValues, 0);

            var legalValuesList = board.LegalValues.ToList();

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    int valueIndex = legalValuesList.IndexOf(board[i, j]);

                    if (valueIndex != -1)
                        bitwiseBoard[i, j] = bitValues[valueIndex];
                    else
                        bitwiseBoard[i, j] = 0;
                }
            }

            return bitwiseBoard;
        }

        /// <summary>
        /// Converts a bitwise SudokuBoard back to a normal board.
        /// </summary>
        /// <typeparam name="T">The type of data at each square of the board.</typeparam>
        /// <param name="bitwiseBoard">The bitwise board to convert.</param>
        /// <param name="legalValuesList">The list of the legal values in the board</param>
        /// <param name="emptyValue">The value for representing an empty square in the board.</param>
        /// <returns>The converted board.</returns>
        public static SudokuBoard<T> ConvertBitwiseBackToBoard<T>(SudokuBoard<int> bitwiseBoard, List<T> legalValuesList, T emptyValue)
        {
            var board = new SudokuBoard<T>(bitwiseBoard.BlockSideLength, legalValuesList, emptyValue);

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    board[i, j] = legalValuesList[FirstActivatedBitPos(bitwiseBoard[i, j])];
                }
            }

            return board;
        }

        /// <summary>
        /// Generates and returns a notes dictionary for the current board. <br/>
        /// A notes dictionary is a dictionary that contains all of the possible <br/>
        /// values each of the empty squares in the board can take.
        /// </summary>
        /// <typeparam name="T">The type of data at each square of the board.</typeparam>
        /// <param name="board">The board to generate notes to.</param>
        /// <returns>The notes dictionary generated containing bit maps of posssible values.</returns>
        public static Dictionary<int, int> GenerateBoardNotes<T>(SudokuBoard<T> board)
        {
            var notes = new Dictionary<int, int>();

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    if (board[i, j].Equals(board.EmptyValue))
                    {
                        int bits = 0;

                        // Activate positions for all of the possible values
                        for (int k = 0; k < board.LegalValues.Count; k++)
                        {
                            bits <<= 1;
                            bits |= 1;
                        }

                        notes.Add(i * board.Width + j, bits);
                    }                       
                }
            }

            return notes;
        }

        /// <summary>
        /// Creates a copy of the notes dictionary.
        /// </summary>
        /// <param name="notes">The dictionary to copy.</param>
        /// <returns>The copy created.</returns>
        public static Dictionary<int, int> CopyNotes(Dictionary<int, int> notes)
        {
            return new Dictionary<int, int>(notes);
        }

        /// <summary>
        /// Finds The index of the square with the least possibilties left.
        /// </summary>
        /// <param name="notes">The notes dictionary of the board.</param>
        /// <returns>Index in the board starting from the upper left corner. <br/> 
        /// index / width is row and index % width is column.</returns>
        public static int FindLeastNotesIndex(Dictionary<int, int> notes)
        {
            int minIndex = -1;
            int minSize = int.MaxValue;

            foreach (var note in notes)
            {
                int count = CountActivatedBits(note.Value);

                if (count < minSize)
                {
                    minIndex = note.Key;
                    minSize = count;
                }
            }

            return minIndex;
        }

        /// <summary>
        /// Get all of the combinations of certain size of a group of elements without repetition.
        /// </summary>
        /// <typeparam name="T">The type of data at each square of the board.</typeparam>
        /// <param name="values">Group of elements to produce combinations of.</param>
        /// <param name="combinationSize">The number of elements is the combinations returned.</param>
        /// <returns>A HashSet containing HashSets of all the combinations.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static HashSet<HashSet<T>> GetCombinations<T>(IEnumerable<T> values, int combinationSize)
        {
            var combinations = new HashSet<HashSet<T>>();
            var currComb = new HashSet<T>();

            if (combinationSize > values.Count())
                throw new ArgumentException("Combination size must be smaller than the number of possible values");

            if (values.Count() == 0 || combinationSize <= 0)
                return new HashSet<HashSet<T>>();

            GetCombinationsRecursive(values, combinationSize, combinations, currComb);

            return combinations;
        }

        // Helper function for GetCombinations()
        private static void GetCombinationsRecursive<T>(IEnumerable<T> values, int combinationSize,
            HashSet<HashSet<T>> combinations, HashSet<T> currComb, int i = 0)
        {
            if (currComb.Count == combinationSize)
            {
                combinations.Add(new HashSet<T>(currComb));
                return;
            }

            if (i >= values.Count())
                return;

            currComb.Remove(values.ElementAt(i));
            GetCombinationsRecursive(values, combinationSize, combinations, currComb, i + 1);

            currComb.Add(values.ElementAt(i));
            GetCombinationsRecursive(values, combinationSize, combinations, currComb, i + 1);
        }
    }
}
