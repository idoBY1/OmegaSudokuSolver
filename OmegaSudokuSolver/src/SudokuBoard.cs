﻿using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    /// <summary>
    /// Class for representing a Sudoku board.
    /// </summary>
    /// <typeparam name="T">The type of data at each square of the board.</typeparam>
    public class SudokuBoard<T>
    {
        // The 2d array representing the board.
        private T[,] _board;

        // A set of the legal values a square can take.
        public FrozenSet<T> LegalValues { get; }

        // The value that represents an empty square.
        public T EmptyValue { get; }

        // The length of an inner block of the board.
        public int BlockSideLength { get; private set; }

        // The width of the board or BlockSideLength^2.
        public int Width { get; private set; }

        /// <summary>
        /// Create a new empty SudokuBoard object.
        /// The size of the board will be [ blockSideLength ^ 2 X blockSideLength ^ 2 ]
        /// </summary>
        /// <param name="blockSideLength">The number to generate the board from. 
        /// Represents a row/column size of an inner block of the board.</param>
        /// <param name="legalValues">A Collection with all of the legal values a square can take.</param>
        /// <param name="emptyValue">The value that represents an empty square.</param>
        public SudokuBoard(int blockSideLength, IEnumerable<T> legalValues, T emptyValue)
        {
            _board = new T[blockSideLength * blockSideLength, blockSideLength * blockSideLength];
            BlockSideLength = blockSideLength;
            Width = BlockSideLength * blockSideLength;

            LegalValues = legalValues.ToFrozenSet();

            EmptyValue = emptyValue;
        }

        /// <summary>
        /// Create a new SudokuBoard object from a 2 dimensional array.<br/>
        /// The amount of rows and columns of the array must be equal and must be a square of a whole number.
        /// </summary>
        /// <param name="values">The 2 dimensional array to create the board from. The board will point to 
        /// the array given (The array will not be copied).</param>
        /// <param name="legalValues">A Collection with all of the legal values a square can take.</param>
        /// <param name="emptyValue">The value that represents an empty square.</param>
        /// <exception cref="ArgumentException">throws an exception if the size of the 2 dimensional 
        /// array is invalid.</exception>
        public SudokuBoard(T[,] values, IEnumerable<T> legalValues, T emptyValue)
        {
            if (values.GetLength(0) != values.GetLength(1))
                throw new ArgumentException("The amount of rows must be equal to the amount of columns!");
            else if (Math.Sqrt(values.GetLength(0)) % 1 != 0)
                throw new ArgumentException("The amount of rows and columns must be a square of a whole number!");

            _board = values;
            BlockSideLength = (int)Math.Sqrt(values.GetLength(0));
            Width = values.GetLength(0);

            LegalValues = legalValues.ToFrozenSet();

            EmptyValue = emptyValue;
        }

        public SudokuBoard(SudokuBoard<T> other)
        {
            _board = new T[other.Width, other.Width];

            for (int i = 0; i < other.Width; i++)
            {
                for (int j = 0; j < other.Width; j++)
                {
                    _board[i, j] = other._board[i, j];
                }
            }
            
            LegalValues = other.LegalValues;
            EmptyValue = other.EmptyValue;
            BlockSideLength = other.BlockSideLength;
            Width = other.Width;
        }

        /// <summary>
        /// Access the item at row 'row' and column 'col' of the board.
        /// </summary>
        /// <param name="row">The index of row to access.</param>
        /// <param name="col">The index of column to access.</param>
        /// <returns></returns>
        public T this[int row, int col]
        {
            get { return _board[row, col]; }
            set { _board[row, col] = value; }
        }

        /// <summary>
        /// Index the board with a single value. The index is from the top-left corner of the board.
        /// </summary>
        /// <param name="index">The index to get the value from.</param>
        /// <returns>Returns the value at the given index.</returns>
        public T Get(int index)
        {
            return _board[index / Width, index % Width];
        }

        /// <summary>
        /// Set the value of a square of the board using a single index.
        /// </summary>
        /// <param name="index">The index of the position to set.</param>
        /// <param name="value">The value to set the square to.</param>
        public void Set(int index, T value)
        {
            _board[index / Width, index % Width] = value;
        }

        /// <summary>
        /// Returns a 2 dimensional array representing the state of the board.
        /// </summary>
        /// <returns>A 2 dimensional array.</returns>
        public T[,] ToArray()
        {
            return _board;
        }

        /// <summary>
        /// Get a stylized and readable string representation of the board.
        /// </summary>
        /// <returns>A string representing the board.</returns>
        public string GetStylizedString()
        {
            // The string to return from the function
            string retStr = " ";

            // The length of the first row (used to create the row dividers)
            int strRowLength = 0;

            if (_board == null)
                return retStr;

            for (var i = 0; i < _board.GetLength(0); i++)
            {
                // Create a row block divider.
                if (i % BlockSideLength == 0 && i != 0)
                {
                    // Every row starts with a " " so subtract 1 to account for this
                    for (var k = 0; k < strRowLength - 1; k++)
                    {
                        retStr += "-";
                    }
                    retStr += "\n ";
                }

                for (var j = 0; j < _board.GetLength(1); j++)
                {
                    // Create a column block divider.
                    if (j % BlockSideLength == 0 && j != 0)
                        retStr += "| ";

                    // Append element from the board to the string.
                    retStr += _board[i, j].ToString() + " ";
                }
                retStr += "\n ";

                // Assign value to strRowLength after the first row.
                if (i == 0)
                {
                    // Subtract by 3 to not count the "\n " at the end of the line and " " at the start of a new line.
                    strRowLength = retStr.Length - 3;
                }
            }

            return retStr;
        }

        public override string? ToString()
        {
            string retStr = "";

            if (_board == null)
                return null;

            for (var i = 0; i < _board.GetLength(0); i++)
            {
                for (var j = 0; j < _board.GetLength(1); j++)
                {
                    retStr += _board[i, j].ToString();
                }
            }

            return retStr;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is SudokuBoard<T>))
                return false;

            SudokuBoard<T> other = obj as SudokuBoard<T>;

            if (this.BlockSideLength != other.BlockSideLength) 
                return false;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (!this._board[i, j].Equals(other._board[i, j]))
                        return false;
                }
            }

            return true;
        }
    }
}
