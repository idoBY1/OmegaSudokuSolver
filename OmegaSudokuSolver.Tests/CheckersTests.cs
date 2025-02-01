using OmegaSudokuSolver;

namespace OmegaSudokuSolver.Tests
{
    [TestClass]
    public class CheckersTests
    {
        private List<SudokuBoard<char>> validBoards;
        private List<SudokuBoard<char>> invalidBoards;
        private List<SudokuBoard<char>> solvedBoards;

        public CheckersTests()
        {
            validBoards = GetValidSudokuBoards();
            invalidBoards = GetInvalidSudokuBoards();
            solvedBoards = GetSolvedSudokuBoards();
        }

        /// <summary>
        /// Creates a list of valid boards for the tests.
        /// </summary>
        /// <returns>The list created.</returns>
        private List<SudokuBoard<char>> GetValidSudokuBoards()
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

            var boards = new List<SudokuBoard<char>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        /// <summary>
        /// Creates a list of invalid boards for the tests.
        /// </summary>
        /// <returns>The list created.</returns>
        private List<SudokuBoard<char>> GetInvalidSudokuBoards()
        {
            string[] boardStrings = {
                "850007070000041000300000000001000406070500000000000200742060000000800030000000000",
                "020300060020070500010000000708600400000902000500000000000100093400080000000000000",
                "300801000050000609005000400500700080040060000000000020200300000000090100007000000",
                "220000000000000000000000000000005500000000000000000000000000000000000000000000000",
                "000000310700500000200040000239008000000000062010000000500600007080000900000050000",
                "070000310700500000200040000039008000000000062010000000500600007080000900000050000"
            };

            var boards = new List<SudokuBoard<char>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        /// <summary>
        /// Creates a list of solved boards for the tests.
        /// </summary>
        /// <returns>The list created.</returns>
        private List<SudokuBoard<char>> GetSolvedSudokuBoards()
        {
            string[] boardStrings = {
                "329816754457293861681457239148975326765342918932168475274681593596734182813529647",
                "751394268369182754284576913876925341132468597495731682618253479527849136943617825",
                "468952173291347586735816492579183264643529817812764935156478329924631758387295641",
                "146389275278654319539217648713568924685492137924731586367145892852973461491826753",
                "379514826658279413214836957196452738845763291723981564581327649932648175467195382",
                "349681275876235491125479836682513947493867152517924368951742683264358719738196524"
            };

            var boards = new List<SudokuBoard<char>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput1()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[0]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput2()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[1]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput3()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[2]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput4()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[3]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput5()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[4]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput6()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[5]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput7()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[6]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput8()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[7]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput9()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[8]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput10()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[9]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_ValidInput11()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(validBoards[10]);
            Assert.IsTrue(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_InvalidInput1()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(invalidBoards[0]);
            Assert.IsFalse(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_InvalidInput2()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(invalidBoards[1]);
            Assert.IsFalse(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_InvalidInput3()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(invalidBoards[2]);
            Assert.IsFalse(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_InvalidInput4()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(invalidBoards[3]);
            Assert.IsFalse(isLegal);
        }

        [TestMethod]
        public void Checker_IsLegal_InvalidInput5()
        {
            var checker = new SetChecker<char>();
            bool isLegal = checker.IsLegal(invalidBoards[4]);
            Assert.IsFalse(isLegal);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput1()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[0]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput2()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[1]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput3()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[2]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput4()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[3]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput5()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[4]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput6()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[5]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput7()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[6]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput8()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[7]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput9()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[8]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput10()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[9]);
        }

        [TestMethod]
        public void Checker_AssertLegal_ValidInput11()
        {
            var checker = new SetChecker<char>();
            checker.AssertLegal(validBoards[10]);
        }

        [TestMethod]
        public void Checker_AssertLegal_InvalidInput1()
        {
            var checker = new SetChecker<char>();
            Assert.ThrowsException<IllegalBoardException>(() => checker.AssertLegal(invalidBoards[0]));
        }

        [TestMethod]
        public void Checker_AssertLegal_InvalidInput2()
        {
            var checker = new SetChecker<char>();
            Assert.ThrowsException<IllegalBoardException>(() => checker.AssertLegal(invalidBoards[1]));
        }

        [TestMethod]
        public void Checker_AssertLegal_InvalidInput3()
        {
            var checker = new SetChecker<char>();
            Assert.ThrowsException<IllegalBoardException>(() => checker.AssertLegal(invalidBoards[2]));
        }

        [TestMethod]
        public void Checker_AssertLegal_InvalidInput4()
        {
            var checker = new SetChecker<char>();
            Assert.ThrowsException<IllegalBoardException>(() => checker.AssertLegal(invalidBoards[3]));
        }

        [TestMethod]
        public void Checker_AssertLegal_InvalidInput5()
        {
            var checker = new SetChecker<char>();
            Assert.ThrowsException<IllegalBoardException>(() => checker.AssertLegal(invalidBoards[4]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput1()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[0]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput2()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[1]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput3()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[2]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput4()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[3]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput5()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[4]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput6()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[5]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput7()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[6]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput8()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[7]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput9()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[8]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput10()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[9]));
        }

        [TestMethod]
        public void Checker_IsFull_ValidInput11()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsFull(validBoards[10]));
        }


        [TestMethod]
        public void Checker_IsFull_SolvedInput1()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsFull(solvedBoards[0]));
        }

        [TestMethod]
        public void Checker_IsFull_SolvedInput2()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsFull(solvedBoards[1]));
        }

        [TestMethod]
        public void Checker_IsFull_SolvedInput3()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsFull(solvedBoards[2]));
        }

        [TestMethod]
        public void Checker_IsFull_SolvedInput4()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsFull(solvedBoards[3]));
        }

        [TestMethod]
        public void Checker_IsFull_SolvedInput5()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsFull(solvedBoards[4]));
        }

        [TestMethod]
        public void Checker_IsFull_SolvedInput6()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsFull(solvedBoards[5]));
        }


        [TestMethod]
        public void Checker_IsSolved_SolvedInput1()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsSolved(solvedBoards[0]));
        }

        [TestMethod]
        public void Checker_IsSolved_SolvedInput2()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsSolved(solvedBoards[1]));
        }

        [TestMethod]
        public void Checker_IsSolved_SolvedInput3()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsSolved(solvedBoards[2]));
        }

        [TestMethod]
        public void Checker_IsSolved_SolvedInput4()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsSolved(solvedBoards[3]));
        }

        [TestMethod]
        public void Checker_IsSolved_SolvedInput5()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsSolved(solvedBoards[4]));
        }

        [TestMethod]
        public void Checker_IsSolved_SolvedInput6()
        {
            var checker = new SetChecker<char>();
            Assert.IsTrue(checker.IsSolved(solvedBoards[5]));
        }


        [TestMethod]
        public void Checker_IsSolved_ValidInput1()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[0]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput2()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[1]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput3()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[2]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput4()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[3]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput5()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[4]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput6()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[5]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput7()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[6]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput8()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[7]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput9()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[8]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput10()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[9]));
        }

        [TestMethod]
        public void Checker_IsSolved_ValidInput11()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(validBoards[10]));
        }


        [TestMethod]
        public void Checker_IsSolved_InvalidInput1()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(invalidBoards[0]));
        }

        [TestMethod]
        public void Checker_IsSolved_InvalidInput2()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(invalidBoards[1]));
        }

        [TestMethod]
        public void Checker_IsSolved_InvalidInput3()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(invalidBoards[2]));
        }

        [TestMethod]
        public void Checker_IsSolved_InvalidInput4()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(invalidBoards[3]));
        }

        [TestMethod]
        public void Checker_IsSolved_InvalidInput5()
        {
            var checker = new SetChecker<char>();
            Assert.IsFalse(checker.IsSolved(invalidBoards[4]));
        }
    }
}
