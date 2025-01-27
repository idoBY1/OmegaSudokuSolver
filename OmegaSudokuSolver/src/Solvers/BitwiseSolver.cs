using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class BitwiseSolver<T> : ISolver<T>
    {
        private static readonly IBoardChecker<int> checker = new SetChecker<int>();

        private List<FrozenSet<int>> boardGroups;

        private Dictionary<Tuple<int, int>, HashSet<HashSet<int>>> combinationsList;

        public SudokuBoard<T> Solve(SudokuBoard<T> board)
        {
            Dictionary<int, int> notes = BitsSolveUtils.GenerateBoardNotes(board);

            boardGroups = GenerateGroups(board);

            combinationsList = new Dictionary<Tuple<int, int>, HashSet<HashSet<int>>>(); // StoreCombinations(board.Width);

            var solved = BitwiseBackTrack(BitsSolveUtils.ConvertBoardToBitwise(board), notes);

            if (solved == null)
                return null;

            return BitsSolveUtils.ConvertBitwiseBackToBoard(solved, board.LegalValues.ToList(), board.EmptyValue);
        }

        /// <summary>
        /// The recursive function for solving the board. This function apllies the rules <br />
        /// to the board and then solves the board by filling in the square with the least <br />
        /// possible options first.
        /// </summary>
        /// <param name="board">The board to solve.</param>
        /// <param name="notes">Dictionary for storing the possible values of each square of the board.</param>
        /// <returns>The solved board. Returns null if failed to solve the board.</returns>
        private SudokuBoard<int> BitwiseBackTrack(SudokuBoard<int> board, Dictionary<int, int> notes)
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
            int pos = BitsSolveUtils.FindLeastNotesIndex(notes);

            // Check if finished.
            if (checker.IsFull(board) || pos == -1)
            {
                if (checker.IsSolved(board))
                    return board;
                else
                    return null;
            }

            int possibilities = notes[pos];

            // Try all of the possible values for this square using backtracking.
            for (int i = 0; (possibilities >> i) != 0; i++)
            {
                notes.Remove(pos);

                // skip this value if it is not possible
                if ((possibilities & (1 << i)) == 0)
                    continue;

                board.Set(pos, (possibilities & (1 << i)));

                var result = BitwiseBackTrack(new SudokuBoard<int>(board), BitsSolveUtils.CopyNotes(notes));

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
        private void ClearTrivialNotes(SudokuBoard<int> board, Dictionary<int, int> notes)
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
                    notes[note.Key] &= ~board[blockRow * board.BlockSideLength + (i / board.BlockSideLength),
                        blockColumn * board.BlockSideLength + (i % board.BlockSideLength)];

                    // Remove from column
                    notes[note.Key] &= ~board[i, col];

                    // Remove from row
                    notes[note.Key] &= ~board[row, i];
                }
            }
        }

        /// <summary>
        /// Write to the board all of the values in the squares with only one possibility left.
        /// </summary>
        /// <param name="board">The board to write to.</param>
        /// <param name="notes">The notes to read and change.</param>
        /// <returns>'true' if the function changed the board and 'false' if it didn't</returns>
        private bool ApplySingles(SudokuBoard<int> board, Dictionary<int, int> notes)
        {
            bool updated = false;

            foreach (var note in notes)
            {
                if (note.Value == 0)
                    return false;

                // If only one possibility left for this square, write this value to the board.
                if (BitsSolveUtils.CountActivatedBits(note.Value) == 1)
                {
                    board.Set(note.Key, note.Value);
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
        private bool ObviousTuples(SudokuBoard<int> board, Dictionary<int, int> notes)
        {
            bool updated = false;

            for (int i = 0; i < boardGroups.Count; i++)
            {
                updated |= ObviousTuplesInGroup(board, notes, boardGroups[i]);
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
        private bool ObviousTuplesInGroup(SudokuBoard<int> board, Dictionary<int, int> notes, IEnumerable<int> groupIndexes)
        {
            bool updated = false;

            HashSet<int> group = groupIndexes.ToHashSet();

            foreach (var item in group)
            {
                if (!notes.ContainsKey(item))
                    group.Remove(item);
            }

            // Apply naked tuples.
            for (int combinationSize = 2; combinationSize < Math.Min(group.Count() / 2, 3); combinationSize++)
            {
                HashSet<HashSet<int>> indexCombinations;

                var combinationKey = new Tuple<int, int>(group.Count, combinationSize);

                if (combinationsList.ContainsKey(combinationKey))
                    indexCombinations = combinationsList[combinationKey];
                else
                {
                    indexCombinations = StoreCombinations(combinationKey.Item1, combinationKey.Item2);
                    combinationsList.Add(combinationKey, indexCombinations);
                }

                // Check every combination.
                foreach (HashSet<int> icombination in indexCombinations)
                {
                    var combination = new HashSet<int>();

                    foreach (int index in icombination)
                    {
                        combination.Add(group.ElementAt(index));
                    }

                    // Stores all of the possibilities of every square in the combination.
                    int combinationValues = 0;

                    // Add all of the possible values of every square in this combination to the combinationValues set.
                    foreach (int index in combination)
                    {
                        combinationValues |= notes[index];
                    }

                    // Found an obvious tuple.
                    if (BitsSolveUtils.CountActivatedBits(combinationValues) <= combinationSize)
                    {
                        foreach (int index in group)
                        {
                            // If the index was not a part of the combination.
                            if (!combination.Contains(index))
                            {
                                // Remove the possibilities that were a part of the obvious tuple
                                // (It is not possible for this square to have these values).
                                notes[index] &= ~combinationValues;
                            }
                        }
                    }
                }
            }

            // Apply hidden tuples.
            for (int missingAmount = 1; missingAmount < Math.Min(group.Count() / 2, 3); missingAmount++)
            {
                // Choose the values to exclude from the combination
                HashSet<HashSet<int>> missingCombinations;

                var combinationKey = new Tuple<int, int>(group.Count, missingAmount);

                if (combinationsList.ContainsKey(combinationKey))
                    missingCombinations = combinationsList[combinationKey];
                else
                {
                    missingCombinations = StoreCombinations(combinationKey.Item1, combinationKey.Item2);
                    combinationsList.Add(combinationKey, missingCombinations);
                }

                // Check every combination.
                foreach (HashSet<int> mCombination in missingCombinations)
                {
                    var combination = group.ToHashSet();

                    // Remove to squares that are supposed to be missing from this group.
                    foreach (int index in mCombination)
                    {
                        combination.Remove(group.ElementAt(index));
                    }

                    // Stores all of the possibilities of every square in the combination.
                    int combinationValues = 0;

                    // Add all of the possible values of every square in this combination to the combinationValues set.
                    foreach (int index in combination)
                    {
                        combinationValues |= notes[index];
                    }

                    // Found an obvious tuple.
                    if (BitsSolveUtils.CountActivatedBits(combinationValues) <= combination.Count)
                    {
                        foreach (int index in group)
                        {
                            // If the index was not a part of the combination.
                            if (!combination.Contains(index))
                            {
                                // Remove the possibilities that were a part of the obvious tuple
                                // (It is not possible for this square to have these values).
                                notes[index] &= ~combinationValues;
                            }
                        }
                    }
                }
            }

            return updated;
        }

        private List<FrozenSet<int>> GenerateGroups(SudokuBoard<T> board)
        {
            var groups = new List<FrozenSet<int>>();

            // A list to store the positions of squares in the current group.
            var groupIndexes = new HashSet<int>();

            // Generate blocks.
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

                    groupIndexes.Add(row * board.Width + col);
                }

                groups.Add(groupIndexes.ToFrozenSet());
            }

            // Generate rows.
            for (int i = 0; i < board.Width; i++)
            {
                // Reset groupIndexes for the next group.
                groupIndexes.Clear();

                // Add all positions in the current row.
                for (int j = 0; j < board.Width; j++)
                {
                    groupIndexes.Add(i * board.Width + j);
                }

                groups.Add(groupIndexes.ToFrozenSet());
            }

            // Generate columns.
            for (int i = 0; i < board.Width; i++)
            {
                // Reset groupIndexes for the next group.
                groupIndexes.Clear();

                // Add all positions in the current column.
                for (int j = 0; j < board.Width; j++)
                {
                    groupIndexes.Add(j * board.Width + i);
                }

                groups.Add(groupIndexes.ToFrozenSet());
            }

            return groups;
        }

        private HashSet<HashSet<int>> StoreCombinations(int n, int k)
        {
            return BitsSolveUtils.GetCombinations(Enumerable.Range(0, n), k);
        }
    }
}
