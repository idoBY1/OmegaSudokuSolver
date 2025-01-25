using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class RuleBasedSolver<T> : ISolver<T>
    {
        private static readonly IBoardChecker<T> checker = new SetChecker<T>();

        public SudokuBoard<T> Solve(SudokuBoard<T> board)
        {
            Dictionary<int, HashSet<T>> notes = SetSolveUtils.GenerateBoardNotes(board);

            return RuleBasedBackTrack(new SudokuBoard<T>(board), notes);
        }

        /// <summary>
        /// The recursive function for solving the board. This function apllies the rules <br />
        /// to the board and then solves the board by filling in the square with the least <br />
        /// possible options first.
        /// </summary>
        /// <param name="board">The board to solve.</param>
        /// <param name="notes">Dictionary for storing the possible values of each square of the board.</param>
        /// <returns>The solved board. Returns null if failed to solve the board.</returns>
        private SudokuBoard<T> RuleBasedBackTrack(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            bool updated = true;

            // Apply rules.
            while (updated)
            {
                updated = false;

                ClearTrivialNotes(board, notes);

                updated = ApplySingles(board, notes) || ObviousTuples(board, notes);
            }

            // Find the square with the least possibilities.
            int pos = SetSolveUtils.FindLeastNotesIndex(notes);

            // Check if finished.
            if (checker.IsFull(board) || pos == -1)
            {
                if (checker.IsSolved(board))
                    return board;
                else
                    return null;
            }

            // Try all of the possible values for this square using backtracking.
            foreach (T val in notes[pos])
            {
                notes.Remove(pos);
                board.Set(pos, val);

                var result = RuleBasedBackTrack(new SudokuBoard<T>(board), SetSolveUtils.CopyNotes(notes));

                if (result != null)
                    return result;
            }

            board.Set(pos, board.EmptyValue);
            return null;
        }

        /// <summary>
        /// Removes from the notes all of the values that already apear in the group elsewhere on the board.
        /// </summary>
        /// <param name="board">The board to read from.</param>
        /// <param name="notes">The notes to change based on the current state of the board.</param>
        private void ClearTrivialNotes(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            foreach (var note in notes)
            {
                int row = note.Key / board.Width;
                int col = note.Key % board.Width;

                // The number of row of the current block (for a 9 X 9 board can be 0, 1 or 2. if pos was 35 for example, it will be 1 (because second block-row))
                int blockRow = row / board.BlockSideLength;

                // The number of column of the current block (for a 9 X 9 board can be 0, 1 or 2. if pos was 35 for example, it will be 2 (because third block-column))
                int blockColumn = col / board.BlockSideLength;

                for (int i = 0; i < board.Width; i++)
                {
                    // Remove from block
                    note.Value.Remove(board[blockRow * board.BlockSideLength + (i / board.BlockSideLength),
                        blockColumn * board.BlockSideLength + (i % board.BlockSideLength)]);

                    // Remove from column
                    note.Value.Remove(board[i, col]);

                    // Remove from row
                    note.Value.Remove(board[row, i]);
                }
            }
        }

        /// <summary>
        /// Write to the board all of the values in the squares with only one possibility left.
        /// </summary>
        /// <param name="board">The board to write to.</param>
        /// <param name="notes">The notes to read and change.</param>
        /// <returns>'true' if the function changed the board and 'false' if it didn't</returns>
        private bool ApplySingles(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            bool updated = false;

            foreach (var note in notes)
            {
                if (note.Value.Count == 0)
                    return false;

                // If only one possibility left for this square, write this value to the board.
                if (note.Value.Count == 1)
                {
                    board.Set(note.Key, note.Value.ElementAt(0));
                    updated = true;
                    notes.Remove(note.Key);
                }
            }

            return updated;
        }

        /// <summary>
        /// Apllies the rules of obvious tuples to the notes. This function clears notes <br/>
        /// from squares if they are not possible based on other squares in the same group. <br/>
        /// </summary>
        /// <param name="board">The board to read.</param>
        /// <param name="notes">The notes to change.</param>
        /// <returns>'true' if the function changed the notes and 'false' if it didn't.</returns>
        private bool ObviousTuples(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            // A list to store the positions of squares in the current group.
            List<int> groupIndexes = new List<int>();

            bool updated = false;

            // Perform on blocks
            for (int i = 0; i < board.Width; i++)
            {
                // Reset groupIndexes for the next group.
                groupIndexes.Clear();

                int blockRow = i / board.BlockSideLength;
                int blockColumn = i % board.BlockSideLength;

                // Add all positions in the current block.
                for (int j = 0; j < board.Width; j++)
                {
                    int row = blockRow * board.BlockSideLength + (j / board.BlockSideLength);
                    int col = blockColumn * board.BlockSideLength + (j % board.BlockSideLength);

                    if (board[row, col].Equals(board.EmptyValue))
                        groupIndexes.Add(row * board.Width + col);
                }

                // Apply the rule on this group.
                updated = updated || ObviousTuplesInGroup(board, notes, groupIndexes);
            }

            // Perform on rows
            for (int i = 0; i < board.Width; i++)
            {
                // Reset groupIndexes for the next group.
                groupIndexes.Clear();

                // Add all positions in the current row.
                for (int j = 0; j < board.Width; j++)
                {
                    if (board[i, j].Equals(board.EmptyValue))
                        groupIndexes.Add(i * board.Width + j);
                }

                // Apply the rule on this group.
                updated = updated || ObviousTuplesInGroup(board, notes, groupIndexes);
            }

            // Perform on columns
            for (int i = 0; i < board.Width; i++)
            {
                // Reset groupIndexes for the next group.
                groupIndexes.Clear();

                // Add all positions in the current column.
                for (int j = 0; j < board.Width; j++)
                {
                    if (board[j, i].Equals(board.EmptyValue))
                        groupIndexes.Add(j * board.Width + i);
                }

                // Apply the rule on this group.
                updated = updated || ObviousTuplesInGroup(board, notes, groupIndexes);
            }

            return updated;
        }

        /// <summary>
        /// Applies the rules of obvious tuples on a group of squares (row / column / block). <br/>
        /// The function checks different combinations of the squares and if it finds a <br/>
        /// combination that has the same number of possible values as the number of squares <br/>
        /// in the combination, it clears the possible values of the combination from the <br/>
        /// notes of every square that is not in the combination because the values must be <br/>
        /// assigned to a square from the combination (or else there will be a square without <br/>
        /// a value, which is not possible).
        /// </summary>
        /// <param name="board">The board to read.</param>
        /// <param name="notes">The notes to change.</param>
        /// <param name="group">The list of the possitions of every square in the group that has not already been filled.</param>
        /// <returns>'true' if the function changed the notes and 'false' if it didn't.</returns>
        private bool ObviousTuplesInGroup(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes, List<int> group)
        {
            bool updated = false;

            // For every combination size.
            for (int combinationSize = 2; combinationSize < group.Count; combinationSize++)
            {
                HashSet<HashSet<int>> combinations = SetSolveUtils.GetCombinations(group, combinationSize);

                // Check every combination.
                foreach (HashSet<int> combination in combinations)
                {
                    // Stores all of the possibilities of every square in the combination.
                    var combinationValues = new HashSet<T>();

                    // Add all of the possible values of every square in this combination to the combinationValues set.
                    foreach (int index in combination)
                    {
                        foreach (T value in notes[index])
                        {
                            combinationValues.Add(value);
                        }
                    }

                    // Found an obvious tuple.
                    if (combinationValues.Count <= combinationSize)
                    {
                        foreach (int index in group)
                        {
                            // If the index was not a part of the combination.
                            if (!combination.Contains(index))
                            {
                                // Remove the possibilities that were a part of the obvious tuple
                                // (It is not possible for this square to have these values).
                                foreach (T value in notes[index])
                                {
                                    if (combinationValues.Contains(value))
                                    {
                                        notes[index].Remove(value);
                                        updated = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return updated;
        }
    }
}
