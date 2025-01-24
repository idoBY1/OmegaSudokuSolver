using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudokuSolver
{
    public class BitwiseSolver<T> : ISolver<T>
    {
        private static readonly IBoardChecker<T> checker = new SetChecker<T>();

        public SudokuBoard<T> Solve(SudokuBoard<T> board)
        {
            //Dictionary<int, HashSet<T>> notes = SetSolveUtils.GenerateBoardNotes(board);

            //return BitwiseBackTrack(new SudokuBoard<T>(board), notes);

            throw new NotImplementedException(); // TODO implement solver
        }

        private SudokuBoard<T> BitwiseBackTrack(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            bool updated = true;

            while (updated)
            {
                updated = false;

                ClearTrivialNotes(board, notes);

                updated = ApplySingles(board, notes) || ObviousTuples(board, notes);
            }

            int pos = SetSolveUtils.FindLeastNotesIndex(notes);

            if (checker.IsFull(board) || pos == -1)
            {
                if (checker.IsSolved(board))
                    return board;
                else
                    return null;
            }

            foreach (T val in notes[pos])
            {
                notes.Remove(pos);
                board.Set(pos, val);

                var result = BitwiseBackTrack(new SudokuBoard<T>(board), SetSolveUtils.CopyNotes(notes));

                if (result != null)
                    return result;
            }

            board.Set(pos, board.EmptyValue);
            return null;
        }

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

        private bool ApplySingles(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            bool updated = false;

            foreach (var note in notes)
            {
                if (note.Value.Count == 0)
                    return false;

                if (note.Value.Count == 1)
                {
                    board.Set(note.Key, note.Value.ElementAt(0));
                    updated = true;
                    notes.Remove(note.Key);
                }
            }

            return updated;
        }

        private bool ObviousTuples(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            List<int> groupIndexes = new List<int>();

            bool updated = false;

            // Perform on blocks
            for (int i = 0; i < board.Width; i++)
            {
                groupIndexes.Clear();

                int blockRow = i / board.BlockSideLength;
                int blockColumn = i % board.BlockSideLength;

                for (int j = 0; j < board.Width; j++)
                {
                    int row = blockRow * board.BlockSideLength + (j / board.BlockSideLength);
                    int col = blockColumn * board.BlockSideLength + (j % board.BlockSideLength);

                    if (board[row, col].Equals(board.EmptyValue))
                        groupIndexes.Add(row * board.Width + col);
                }

                updated = updated || ObviousTuplesInGroup(board, notes, groupIndexes);
            }

            // Perform on rows
            for (int i = 0; i < board.Width; i++)
            {
                groupIndexes.Clear();

                for (int j = 0; j < board.Width; j++)
                {
                    if (board[i, j].Equals(board.EmptyValue))
                        groupIndexes.Add(i * board.Width + j);
                }

                updated = updated || ObviousTuplesInGroup(board, notes, groupIndexes);
            }

            // Perform on columns
            for (int i = 0; i < board.Width; i++)
            {
                groupIndexes.Clear();

                for (int j = 0; j < board.Width; j++)
                {
                    if (board[j, i].Equals(board.EmptyValue))
                        groupIndexes.Add(j * board.Width + i);
                }

                updated = updated || ObviousTuplesInGroup(board, notes, groupIndexes);
            }

            return updated;
        }

        private bool ObviousTuplesInGroup(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes, List<int> group)
        {
            bool updated = false;

            for (int combinationSize = 2; combinationSize < group.Count; combinationSize++)
            {
                HashSet<HashSet<int>> combinations = SetSolveUtils.GetCombinations(group, combinationSize);

                foreach (HashSet<int> combination in combinations)
                {
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
