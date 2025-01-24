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

        public SudokuBoard<T> Solve(SudokuBoard<T> board)
        {
            Dictionary<int, int> notes = BitsSolveUtils.GenerateBoardNotes(board);

            var solved = BitwiseBackTrack(BitsSolveUtils.ConvertBoardToBitwise(board), notes);

            if (solved == null)
                return null;

            return BitsSolveUtils.ConvertBitwiseBackToBoard(solved, board.LegalValues.ToList(), board.EmptyValue);
        }

        private SudokuBoard<int> BitwiseBackTrack(SudokuBoard<int> board, Dictionary<int, int> notes)
        {
            bool updated = true;

            while (updated)
            {
                updated = false;

                ClearTrivialNotes(board, notes);

                updated = ApplySingles(board, notes) || ObviousTuples(board, notes);
            }

            int pos = BitsSolveUtils.FindLeastNotesIndex(notes);

            if (checker.IsFull(board) || pos == -1)
            {
                if (checker.IsSolved(board))
                    return board;
                else
                    return null;
            }

            int possibilities = notes[pos];

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

        private bool ApplySingles(SudokuBoard<int> board, Dictionary<int, int> notes)
        {
            bool updated = false;

            foreach (var note in notes)
            {
                if (note.Value == 0)
                    return false;

                if (BitsSolveUtils.CountActivatedBits(note.Value) == 1)
                {
                    board.Set(note.Key, note.Value);
                    updated = true;
                    notes.Remove(note.Key);
                }
            }

            return updated;
        }

        private bool ObviousTuples(SudokuBoard<int> board, Dictionary<int, int> notes)
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

        private bool ObviousTuplesInGroup(SudokuBoard<int> board, Dictionary<int, int> notes, List<int> group)
        {
            bool updated = false;

            for (int combinationSize = 2; combinationSize < group.Count; combinationSize++)
            {
                HashSet<HashSet<int>> combinations = SetSolveUtils.GetCombinations(group, combinationSize);

                foreach (HashSet<int> combination in combinations)
                {
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

            return updated;
        }
    }
}
