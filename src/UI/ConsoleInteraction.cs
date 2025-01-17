﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver.src
{
    internal class ConsoleInteraction : IUserInteraction
    {
        public void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        public void PrintBoard<T>(SudokuBoard<T> board)
        {
            Console.WriteLine(board.GetStylizedString());
        }

        public void PrintBoardString<T>(SudokuBoard<T> board)
        {
            Console.WriteLine(board.ToString());
        }

        public SudokuBoard<T> ReadBoard<T>(T emptySquareObject, int blockSideLength)
        {
            if (emptySquareObject == null)
            {
                throw new ArgumentNullException(nameof(emptySquareObject));
            }

            if (typeof(T) == typeof(int))
            {
                return (SudokuBoard<T>)Convert.ChangeType(
                    ReadIntBoard((int)Convert.ChangeType(emptySquareObject, typeof(int)), blockSideLength), 
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
        /// <returns>An int SudokuBoard object</returns>
        /// <exception cref="IOException">Throws this exception if the input is not long enough to populate the entire board</exception>
        private SudokuBoard<int> ReadIntBoard(int emptySquareNumber, int blockSideLength)
        {
            int boardSideLength = blockSideLength * blockSideLength;

            var board = new SudokuBoard<int>(blockSideLength);

            for (int i = 0; i < boardSideLength; i++)
            {
                for (int j = 0; j < boardSideLength; j++)
                {
                    int currentInput = Console.Read();

                    if (currentInput == -1)
                        throw new IOException("Not enough characters to create a board");

                    board[i, j] = int.Parse(((char)currentInput).ToString());
                }
            }

            return board;
        }
    }
}
