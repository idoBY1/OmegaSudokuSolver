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
            Dictionary<int, HashSet<T>> notes = SolveUtils.GenerateBoardNotes(board);

            return RuleBasedBackTrack(new SudokuBoard<T>(board), notes);
        }

        private SudokuBoard<T> RuleBasedBackTrack(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            bool updated = true;

            while (updated)
            {
                updated = false;

                ClearTrivialNotes(board, notes);

                updated = ApplySingles(board, notes);
            }

            // TODO implement and call obvious tuples here

            int pos = SolveUtils.FindLeastNotesIndex(notes);

            if (checker.IsFull(board) || pos == -1)
            {
                if (checker.IsSolved(board))
                    return board;
                else
                    return null;
            }

            int row = pos / board.Width;
            int col = pos % board.Width;

            foreach (T val in notes[pos])
            {
                notes.Remove(pos);
                board[row, col] = val;

                var result = RuleBasedBackTrack(new SudokuBoard<T>(board), SolveUtils.CopyNotes(notes));

                if (result != null)
                    return result;
            }

            board[row, col] = board.EmptyValue;
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
                    board[note.Key / board.Width, note.Key % board.Width] = note.Value.ElementAt(0);
                    updated = true;
                    notes.Remove(note.Key);
                }
            }

            return updated;
        }

        private bool ObviousTuples(SudokuBoard<T> board, Dictionary<int, HashSet<T>> notes)
        {
            throw new NotImplementedException();
        }
    }
}
