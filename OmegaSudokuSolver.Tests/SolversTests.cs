using OmegaSudokuSolver;
using System.Diagnostics;

namespace OmegaSudokuSolver.Tests
{
    [TestClass]
    public class SolversTests
    {
        private List<SudokuBoard<char>> validBoards;
        private List<SudokuBoard<char>> unsolvableBoards;
        private IBoardChecker<char> checker;

        public SolversTests()
        {
            validBoards = GetValidSudokuBoards();
            unsolvableBoards = GetUnsolvableSudokuBoards();
            checker = new SetChecker<char>();
        }

        /// <summary>
        /// Creates a list of valid boards for the tests.
        /// </summary>
        /// <returns>The list created.</returns>
        private List<SudokuBoard<char>> GetValidSudokuBoards()
        {
            string[] boardStrings = {
                "974236158638591742125487936316754289742918563589362417867125394253649871491873625",
                "256489173374615982981723456593274861712806549468591327635147298127958634849362715",
                "305420810487901506029056374850793041613208957074065280241309065508670192096512408",
                "400030000000600800000000001000050090080000600070200000000102700503000040900000000",
                "708000300000201000500000000040000026300080000000100090090600004000070500000000000",
                "307040000000000091800000000400000700000160000000250000000000380090000500020600000",
                "500700600003800000000000200620400000000000091700000000000035080400000100000090000",
                "400700600003800000000000200620500000000000091700000000000043080500000100000090000",
                "002600800000010400050000030000005000001000704060038200007006000000000000098200560",
                "000000801600200000000705000000600020010000300080000000200000070030080000500040000",
                "070810000900070000300400008005000430060001200000900007000000602080000750000524000",
                "902401500004008000000200000000000000005307100300010460007000004089000320500600000",
                "100000000020000005906800000000030000600000409070029600000005900000100706830000004",
                "000000003090604000000010280000060001730000600050400070005000700309100000600970002",
                "800406000030200400040050078069005020300000000000600017520000030000003600000002009",
                "306070000000000051800000000010405000700000600000200000020000040000080300000500000",
                "050000047000080020020000006400350070065090400038000000000510600000009004009070002",
                "004700000300045000007000600900236070050000080002000000000018027000900100700000003",
                "003500080800001070000000301200010000070024900600007000302006008450008000000000460",
                "205006000090070800006050200002000000980000570000100004020608000500040700000000130",
                "000063470600040891004080003720614059415030000096270004940000000060000745807001000",
                "007000200310705406600040100000430527200607900000800010700004000190500860003080740",
                "400051000573062900000000507800009153130600804000003000926800430054090208000024609",
                "000039208812006004409812750243050060100000000500328490706180000051794680084205100",
                "001065079003000060629030800816047503930010007270000601108306050560491738307028000",
                "0000000000000000200000000000000000000000000003000000000000000000000004000000000000" +
                "0000000000026000000000000000000100000000000700000000000000000080000000000000000000" +
                "0000",
                "240109380639400051851300290090003502308000400560940130410785903925630040083200010",
                "0000:=000000000?70050;01:00@90<8900800700004600=60:=080000070002=00030890>?500012;" + // 16 x 16
                "01@:000008007>00001000@0000900000<>?0740000000006@900000>0100002;0600=800<00500070" +
                "002000000000<0900>?5;4020=0@0020=0@0<0907000>?500400=6000<803<0000002001@:0000=680" +
                "00?0004020",
                "0040700000?0000003:050000000000<0000600009000007000000000010000000000=0000000004320" + // 16 x 16
                "000;000001800000600000>000000000800015000000000001030<000=000@0000000000700500010>0" +
                "000?000:0000300000005600200?00;000000:0000200004090;000006000100000000@00000=000700" +
                "04000?0000",
                "0E487:009200I300000=<;0?0090:50>00G=1B00;60A<87FE003000=1BC00;0?070008:5@9200D=1<0" + // 25 x 25
                "0?080FE450920000006?A0;80FE400092>I0G0010C0E48705I000003G00000100?0<90:0I>B30H10C0" +
                "0F00<;00E4000H>B1000=0000<@E4075I92:CD=060F?A07@E40I00:50B30H00000000000I02:B0GH>1" +
                "0000D=00000A00@94070000I00GH>A00FE00487030:5C0H00600=000009030:00C000?0006000<;2:5" +
                "I0B000>6?0=1EA<;0@9000G0>B060D01FEA<;94870002:0:5000CD00B?A=004<0F092870H0BC00A010" +
                "040;02800930:50=1000000000200@0:0I3CDH>000FE40087030:000000C0A=108709230:0IC00>BA=" +
                "06000<007090:GH5I0D0>B00160A08;F05I00H00>00A006080FE00:000>000=A000048;0E00002G000" +
                "006?A000;FE2:7@905I3G000000FE402:0@90H003000CDA<100",
                "0", // 1 x 1
                "0000000000000000" // 4 x 4
            };

            var boards = new List<SudokuBoard<char>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        /// <summary>
        /// Creates a list of unsolvable boards for the tests.
        /// </summary>
        /// <returns>The list created.</returns>
        private List<SudokuBoard<char>> GetUnsolvableSudokuBoards()
        {
            string[] boardStrings = {
                "000005080000601043000000000010500000000106000300000005530000061000000004000000000",
                "000030000060000400007050800000406000000900000050010300400000020000300000000000000",
                "009028700806004005003000004600000000020713450000000002300000500900400807001250300",
                "090300001000080046000000800405060030003275600060010904001000000580020000200007060",
                "000041000060000020002000000320600000000050041700000002000000230048000000501002000",
                "900100004014030800003000090000708001800003000000000030021000070009040500500016003"
            };

            var boards = new List<SudokuBoard<char>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput1()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[0])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput2()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[1])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput3()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[2])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput4()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[3])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput5()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[4])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput6()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[5])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput7()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[6])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput8()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[7])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput9()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[8])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput10()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[9])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput11()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[10])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput12()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[11])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput13()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[12])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput14()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[13])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput15()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[14])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput16()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[15])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput17()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[16])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput18()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[17])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput19()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[18])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput20()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[19])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput21()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[20])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput22()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[21])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput23()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[22])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput24()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[23])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput25()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[24])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput26()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[25])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput27()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[26])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput28()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[27])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput29()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[28])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput30()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[29])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput31()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[30])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput32()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[31])));
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_UnsolvableInput1()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.AreEqual(solver.Solve(unsolvableBoards[0]), null);
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_UnsolvableInput2()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.AreEqual(solver.Solve(unsolvableBoards[1]), null);
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_UnsolvableInput3()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.AreEqual(solver.Solve(unsolvableBoards[2]), null);
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_UnsolvableInput4()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.AreEqual(solver.Solve(unsolvableBoards[3]), null);
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_UnsolvableInput5()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.AreEqual(solver.Solve(unsolvableBoards[4]), null);
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_UnsolvableInput6()
        //{
        //    var solver = new RuleBasedSolver<char>();
        //    Assert.AreEqual(solver.Solve(unsolvableBoards[5]), null);
        //}

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput1()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[0])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput2()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[1])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput3()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[2])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput4()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[3])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput5()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[4])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput6()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[5])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput7()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[6])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput8()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[7])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput9()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[8])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput10()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[9])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput11()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[10])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput12()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[11])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput13()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[12])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput14()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[13])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput15()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[14])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput16()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[15])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput17()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[16])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput18()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[17])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput19()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[18])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput20()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[19])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput21()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[20])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput22()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[21])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput23()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[22])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput24()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[23])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput25()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[24])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput26()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[25])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput27()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[26])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput28()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[27])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput29()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[28])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput30()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[29])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput31()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[30])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput32()
        {
            var solver = new BitwiseSolver<char>();
            Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[31])));
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput1()
        {
            var solver = new BitwiseSolver<char>();
            Assert.AreEqual(solver.Solve(unsolvableBoards[0]), null);
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput2()
        {
            var solver = new BitwiseSolver<char>();
            Assert.AreEqual(solver.Solve(unsolvableBoards[1]), null);
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput3()
        {
            var solver = new BitwiseSolver<char>();
            Assert.AreEqual(solver.Solve(unsolvableBoards[2]), null);
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput4()
        {
            var solver = new BitwiseSolver<char>();
            Assert.AreEqual(solver.Solve(unsolvableBoards[3]), null);
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput5()
        {
            var solver = new BitwiseSolver<char>();
            Assert.AreEqual(solver.Solve(unsolvableBoards[4]), null);
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput6()
        {
            var solver = new BitwiseSolver<char>();
            Assert.AreEqual(solver.Solve(unsolvableBoards[5]), null);
        }
    }
}