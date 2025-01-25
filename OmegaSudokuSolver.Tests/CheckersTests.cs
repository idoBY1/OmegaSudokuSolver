using OmegaSudokuSolver;

namespace OmegaSudokuSolver.Tests
{
    [TestClass]
    public class CheckersTests
    {
        private List<SudokuBoard<int>> validBoards;
        private List<SudokuBoard<int>> invalidBoards;
        private List<SudokuBoard<int>> solvedBoards;

        public CheckersTests()
        {
            validBoards = GetValidSudokuBoards();
            invalidBoards = GetInvalidSudokuBoards();
            solvedBoards = GetSolvedSudokuBoards();
        }

        private List<SudokuBoard<int>> GetValidSudokuBoards()
        {
            string[] boardStrings = {
                "400030000000600800000000001000050090080000600070200000000102700503000040900000000",
                "708000300000201000500000000040000026300080000000100090090600004000070500000000000",
                "708000300000601000500000000040000026300080000000100090090200004000070500000000000",
                "307040000000000091800000000400000700000160000000250000000000380090000500020600000",
                "500700600003800000000000200620400000000000091700000000000035080400000100000090000",
                "400700600003800000000000200620500000000000091700000000000043080500000100000090000",
                "040010200000009070010000000000430600800000050000200000705008000000600300900000000",
                "705000002000401000300000000010600400200050000000000090000370000080000600090000080",
                "000000410900300000300050000048007000000000062010000000600200005070000800000090000",
                "705000002000401000300000000010600400200050000000000090000370000090000800080000060",
                "080010000005000030000000400000605070890000200000300000200000109006700000000400000"
            };

            var boards = new List<SudokuBoard<int>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        private List<SudokuBoard<int>> GetInvalidSudokuBoards()
        {
            string[] boardStrings = {
                "850007070000041000300000000001000406070500000000000200742060000000800030000000000",
                "020300060020070500010000000708600400000902000500000000000100093400080000000000000",
                "300801000050000609005000400500700080040060000000000020200300000000090100007000000",
                "220000000000000000000000000000005500000000000000000000000000000000000000000000000",
                "000000310700500000200040000239008000000000062010000000500600007080000900000050000",
                "070000310700500000200040000039008000000000062010000000500600007080000900000050000"
            };

            var boards = new List<SudokuBoard<int>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        private List<SudokuBoard<int>> GetSolvedSudokuBoards()
        {
            string[] boardStrings = {
                "329816754457293861681457239148975326765342918932168475274681593596734182813529647",
                "751394268369182754284576913876925341132468597495731682618253479527849136943617825",
                "468952173291347586735816492579183264643529817812764935156478329924631758387295641",
                "146389275278654319539217648713568924685492137924731586367145892852973461491826753",
                "379514826658279413214836957196452738845763291723981564581327649932648175467195382",
                "349681275876235491125479836682513947493867152517924368951742683264358719738196524"
            };

            var boards = new List<SudokuBoard<int>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < validBoards.Count; i++)
            {
                Assert.IsTrue(checker.IsLegal(validBoards[i]));
            }
        }

        [TestMethod]
        public void Checker_IsLegal_InvalidInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < invalidBoards.Count; i++)
            {
                Assert.IsFalse(checker.IsLegal(invalidBoards[i]));
            }
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < validBoards.Count; i++)
            {
                checker.AssertLegal(validBoards[i]);
            }
        }

        [TestMethod]
        public void Checker_AssertLegal_InvalidInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < invalidBoards.Count; i++)
            {
                Assert.ThrowsException<IllegalBoardException>(() => checker.AssertLegal(invalidBoards[i]));
            }
        }

        [TestMethod]
        public void Checker_IsFull_UnfilledInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < validBoards.Count; i++)
            {
                Assert.IsFalse(checker.IsFull(validBoards[i]));
            }
        }

        [TestMethod]
        public void Checker_IsFull_FilledInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < solvedBoards.Count; i++)
            {
                Assert.IsTrue(checker.IsFull(solvedBoards[i]));
            }
        }

        [TestMethod]
        public void Checker_IsSolved_SolvedInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < solvedBoards.Count; i++)
            {
                Assert.IsTrue(checker.IsSolved(solvedBoards[i]));
            }
        }

        [TestMethod]
        public void Checker_IsSolved_UnsolvedInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < validBoards.Count; i++)
            {
                Assert.IsFalse(checker.IsSolved(validBoards[i]));
            }
        }

        [TestMethod]
        public void Checker_IsSolved_InvalidInput()
        {
            var checker = new SetChecker<int>();

            for (int i = 0; i < invalidBoards.Count; i++)
            {
                Assert.IsFalse(checker.IsSolved(invalidBoards[i]));
            }
        }
    }
}
