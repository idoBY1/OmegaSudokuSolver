namespace OmegaSudokuSolver.Tests;

[TestClass]
public class FileInteractionTests
{
    // This is the path to the input files of the tests.
    private static readonly string TEST_INPUTS_FOLDER = "../../../inputFiles/";

    [TestMethod]
    public void Files_SkipWhiteSpace()
    {
        FileInteraction input = new FileInteraction(TEST_INPUTS_FOLDER + "space.txt", "res.txt");

        input.SkipWhiteSpace();

        Assert.AreEqual(input.ReadBoardAuto(), BoardUtils.CreateBoardFromString("0000000000000000"));
    }

    [TestMethod]
    public void Files_IsFinished()
    {
        FileInteraction input = new FileInteraction(TEST_INPUTS_FOLDER + "space.txt", "res.txt");

        input.SkipWhiteSpace();
        input.ReadBoardAuto();

        Assert.IsTrue(input.IsFinished());
    }

    [TestMethod]
    public void Files_ReadBoardAuto_Input1()
    {
        FileInteraction input = new FileInteraction(TEST_INPUTS_FOLDER + "input1.txt", "res.txt");

        Assert.AreEqual(input.ReadBoardAuto(), BoardUtils.CreateBoardFromString("003000002080050000700800049000000100006003000900500078009060014000400200100000500"));

        input.SkipWhiteSpace();

        Assert.AreEqual(input.ReadBoardAuto(), BoardUtils.CreateBoardFromString("005300000800000020070010500400005300010070006003200009060500040000000700000003002"));
        
        input.SkipWhiteSpace();

        Assert.AreEqual(input.ReadBoardAuto(), BoardUtils.CreateBoardFromString("003080000000350000070000600005000000020009407000000001000000080060000030100004000"));
    }

    [TestMethod]
    public void Files_ReadBoardAuto_Input2()
    {
        FileInteraction input = new FileInteraction(TEST_INPUTS_FOLDER + "input2.txt", "res.txt");

        Assert.ThrowsException<ReadBoardFailException>(() => input.ReadBoardAuto());
    }

    [TestMethod]
    public void Files_ReadBoardAuto_Input3()
    {
        FileInteraction input = new FileInteraction(TEST_INPUTS_FOLDER + "input3.txt", "res.txt");

        Assert.AreEqual(input.ReadBoardAuto(), BoardUtils.CreateBoardFromString("0040700000?0000003:050000000000<0000600009000007000000000010000000000=000000000432" +
            "0000;000001800000600000>000000000800015000000000001030<000=000@0000000000700500010>0000?000:0000300000005600200?00;000000:0000200004090;000006000100" +
            "000000@00000=00070004000?0000500000000000000000000000060700"));

        input.SkipWhiteSpace();

        Assert.AreEqual(input.ReadBoardAuto(), BoardUtils.CreateBoardFromString("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"));
    }
}
