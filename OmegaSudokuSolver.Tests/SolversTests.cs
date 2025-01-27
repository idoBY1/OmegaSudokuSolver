using OmegaSudokuSolver;
using System.Diagnostics;

namespace OmegaSudokuSolver.Tests
{
    [TestClass]
    public class SolversTests
    {
        private List<SudokuBoard<int>> validBoards;
        private List<SudokuBoard<int>> unsolvableBoards;

        public SolversTests()
        {
            validBoards = GetValidSudokuBoards();
            unsolvableBoards = GetUnsolvableSudokuBoards();
        }

        private List<SudokuBoard<int>> GetValidSudokuBoards()
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
                "040010200000009070010000000000430600800000050000200000705008000000600300900000000",
                "705000002000401000300000000010600400200050000000000090000370000080000600090000080",
                "000000410900300000300050000048007000000000062010000000600200005070000800000090000",
                "705000002000401000300000000010600400200050000000000090000370000090000800080000060",
                "080010000005000030000000400000605070890000200000300000200000109006700000000400000",
                "002030008000008000031020000060050270010000050204060031000080605000000013005310400",
                "800000021001009506000005300070400060000070009002090000308600000105000000000000042",
                "020004800054000600900000001000507068000030100080002000300000000070908052000070000",
                "020000074000006080006040900300000000009800000500019000010500032040200000000400001",
                "000658000004000000120000000000009607000300500002080003001900800306000004000047300",
                "000015060030004070000800059300070006000000210400001000920000000000590700805003000",
                "070600300800000006002000090005067000008001009460080000009040000000000207000208003",
                "807006002000050000400000900000020067001004000006309004700060080009002000030000500",
                "708000004065070000040050090000010052000800003006004900000085020070300005000200100",
                "029000600008700020000000050005900300040012006000407000006009001000008400050000060",
                "002005830700020000000000040001207000000000900080103400090800000600050008030700006",
                "630800010097000000000000030000100006100050000040006000500200001004000907300007480",
                "300060100650290000000000008810070500004300900005002007000430090000007006400000800",
                "100000000004001090060080000003070800000005100700100000000003008005004970406850010",
                "008005000300090000007300000021600080000002003500009402000000600005700810400000007",
                "002009007006000030500000009009012600080040090200000001000003905000785000003000040",
                "000503602030690050050700000709002000010000900080900000000006300001000067500370010",
                "100070000000049000906080030000090007020000500008460120061800000300000008005000002",
                "100600000070309040004070002010700009037000200500000006000005900002030000780010000",
                "100000080070008030000051470500000006040007000210004000000090000006020000080000700",
                "900401000030000000060300701200000006000007100008000450009004800000900012300200600",
                "900300004100020005002000301200609003609050000000000000480900070000000040000810000",
                "150070000000000940080000000000008025600200000300000000000900300004600000070000001",
                "100007590070090000006400000000800000630000040090540302320000004000900080000104007",
                "006030521000700000903000040007000600000004000010502700000000973000005000008010000",
                "000750000000048020300000006006000000001000074050402100070000300900005018020900000",
                "000400026070009000280500000800300007900000080000090064000700100006000002003015000",
                "000080610053000000000000502000400000020058007400709050964000000000020860300000700",
                "040200008001050040200800700008000097000600300095700000000000060704010002000003100",
                "004008000000700624000000000002000900603000050700024000050300070090000008000010205",
                "003002060900301000102050080000000002005003098000400100007000005020060000401900000",
                "069107008002000006080000100000700032090008000006300800070004000000050040500006000",
                "046000090003004000000060000007030040000800010500100200001070809005010007030900000",
                "000580600000020000974000000016000800040900070500000000000060008050400010002090400",
                "080650002090007000010300008700000043000400500500001000002090300900000020004200060",
                "030000000805000401900001060000930010000006020040000076000010002700800000001350000",
                "000800000004300800700000190050000070300240000008009003029000000000001020001002409",
                "000006050300100860000900403400008100070300004090001000005003000908060000000000205",
                "150003000000409030300080000030100706504008000000070000000000002400000051600002090",
                "050090000100000600000308000008040009514000000030000200000000004080006007700150060",
                "000098000000000710201000400000006000800470000607001003100500000030000860004000190",
                "000040870061000000000000000000600031200070000400000000050108000700000200000300000",
                "000300000000000081052009006709000100000400050000806020208050090095204000001000000",
                "104605080806200000000000000000060700600007010009100200050400003000910004008000002",
                "002300000850040000970501000400000890020910003000004006000000509000005010007030008",
                "000014700609000000300000000000601030050800000000000010000930800040000002000000500",
                "000000801700200000000506000000700050010000300080000000500000020030080000600040000",
                "000000801600200000000705000000600020010000300080000000200000070040080000500030000",
                "001300000060000020340090000400900700009460005200100000000603001000008050608000400",
                "100000702030100000090000000000390060508000200000000000200008500000600090000040000",
                "006100000070003560090002000080070020000000050000504300400000790100008400000005006",
                "003000780006009000700000005007000003000020090000004210500300006800002000900500000",
                "000501000090000800060000000401000000000070090000000030800000105000200400000360000",
                "020000000006780100000002000001005004000040000900320605000000008007000030060203957",
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
                "240109380639400051851300290090003502308000400560940130410785903925630040083200010"
            };

            var boards = new List<SudokuBoard<int>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        private List<SudokuBoard<int>> GetUnsolvableSudokuBoards()
        {
            string[] boardStrings = {
                "000005080000601043000000000010500000000106000300000005530000061000000004000000000",
                "000030000060000400007050800000406000000900000050010300400000020000300000000000000",
                "009028700806004005003000004600000000020713450000000002300000500900400807001250300",
                "090300001000080046000000800405060030003275600060010904001000000580020000200007060",
                "000041000060000020002000000320600000000050041700000002000000230048000000501002000",
                "900100004014030800003000090000708001800003000000000030021000070009040500500016003"
            };

            var boards = new List<SudokuBoard<int>>();

            for (int i = 0; i < boardStrings.Length; i++)
            {
                boards.Add(BoardUtils.CreateBoardFromString(boardStrings[i]));
            }

            return boards;
        }

        // BasicBacktrackingSolver

        // Too slow!

        //[TestMethod]
        //public void BasicBacktrackingSolver_Solve_ValidInput()
        //{
        //    var checker = new SetChecker<int>();
        //    var solver = new BasicBacktrackingSolver<int>();

        //    for (int i = 0; i < validBoards.Count; i++)
        //    {
        //        Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[i])));
        //    }
        //}

        //[TestMethod]
        //public void BasicBacktrackingSolver_Solve_UnsolvableInput()
        //{
        //    var solver = new BasicBacktrackingSolver<int>();

        //    for (int i = 0; i < unsolvableBoards.Count; i++)
        //    {
        //        Assert.Equals(solver.Solve(unsolvableBoards[i]), null);
        //    }
        //}

        //[TestMethod]
        //public void BasicBacktrackingSolver_Solve_ValidInput_MeasureTime()
        //{
        //    var checker = new SetChecker<int>();
        //    var solver = new BasicBacktrackingSolver<int>();

        //    Stopwatch sw = Stopwatch.StartNew();

        //    for (int i = 0; i < validBoards.Count; i++)
        //    {
        //        solver.Solve(validBoards[i]);
        //    }

        //    sw.Stop();

        //    Assert.IsTrue(sw.ElapsedMilliseconds / validBoards.Count < 1000);
        //}

        //[TestMethod]
        //public void BasicBacktrackingSolver_Solve_UnsolvableInput_MeasureTime()
        //{
        //    var solver = new BasicBacktrackingSolver<int>();

        //    Stopwatch sw = Stopwatch.StartNew();

        //    for (int i = 0; i < unsolvableBoards.Count; i++)
        //    {
        //        solver.Solve(unsolvableBoards[i]);
        //    }

        //    sw.Stop();

        //    Assert.IsTrue(sw.ElapsedMilliseconds / unsolvableBoards.Count < 1000);
        //}

        // RuleBasedSolver

        // Also too slow!

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput()
        //{
        //    var checker = new SetChecker<int>();
        //    var solver = new RuleBasedSolver<int>();

        //    for (int i = 0; i < validBoards.Count; i++)
        //    {
        //        Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[i])));
        //    }
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_UnsolvableInput()
        //{
        //    var solver = new RuleBasedSolver<int>();

        //    for (int i = 0; i < unsolvableBoards.Count; i++)
        //    {
        //        Assert.AreEqual(solver.Solve(unsolvableBoards[i]), null);
        //    }
        //}

        //[TestMethod]
        //public void RuleBasedSolver_Solve_ValidInput_MeasureTime()
        //{
        //    var checker = new SetChecker<int>();
        //    var solver = new RuleBasedSolver<int>();

        //    Stopwatch sw = Stopwatch.StartNew();

        //    for (int i = 0; i < validBoards.Count; i++)
        //    {                
        //        solver.Solve(validBoards[i]);   
        //    }

        //    sw.Stop();

        //    Assert.IsTrue(sw.ElapsedMilliseconds / validBoards.Count < 500);
        //}

        [TestMethod]
        public void RuleBasedSolver_Solve_UnsolvableInput_MeasureTime()
        {
            var solver = new RuleBasedSolver<int>();

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < unsolvableBoards.Count; i++)
            {
                solver.Solve(unsolvableBoards[i]);
            }

            sw.Stop();

            Assert.IsTrue(sw.ElapsedMilliseconds / unsolvableBoards.Count < 500);
        }

        // BitwiseSolver

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput()
        {
            var checker = new SetChecker<int>();
            var solver = new BitwiseSolver<int>();

            for (int i = 0; i < validBoards.Count; i++)
            {
                Assert.IsTrue(checker.IsSolved(solver.Solve(validBoards[i])));
            }
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput()
        {
            var solver = new BitwiseSolver<int>();

            for (int i = 0; i < unsolvableBoards.Count; i++)
            {
                Assert.AreEqual(solver.Solve(unsolvableBoards[i]), null);
            }
        }

        [TestMethod]
        public void BitwiseSolver_Solve_ValidInput_MeasureTime()
        {
            var checker = new SetChecker<int>();
            var solver = new BitwiseSolver<int>();

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < validBoards.Count; i++)
            {
                solver.Solve(validBoards[i]);
            }

            sw.Stop();

            Assert.IsTrue(sw.ElapsedMilliseconds / validBoards.Count < 500);
        }

        [TestMethod]
        public void BitwiseSolver_Solve_UnsolvableInput_MeasureTime()
        {
            var solver = new BitwiseSolver<int>();

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < unsolvableBoards.Count; i++)
            {
                solver.Solve(unsolvableBoards[i]);
            }

            sw.Stop();

            Assert.IsTrue(sw.ElapsedMilliseconds / unsolvableBoards.Count < 500);
        }
    }
}