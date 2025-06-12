using System.Collections.Generic;
using Xunit;
using ZonkGameCore.FSM;

namespace ZonkGame.FSM.Tests;

public class DiceCombinationTests
{
    [Fact]
    public void CalculateScore_FullStreet()
    {
        var score = DicesCombinationsExtension.CalculateScore([1,2,3,4,5,6]);
        Assert.Equal(1500, score);
    }

    [Fact]
    public void CalculateScore_SemiStreetWithoutSix()
    {
        var score = DicesCombinationsExtension.CalculateScore([1,2,3,4,5]);
        Assert.Equal(500, score);
    }

    [Fact]
    public void CalculateScore_SemiStreetWithoutOne()
    {
        var score = DicesCombinationsExtension.CalculateScore([2,3,4,5,6]);
        Assert.Equal(750, score);
    }

    [Fact]
    public void CalculateScore_TripleOnes()
    {
        var score = DicesCombinationsExtension.CalculateScore([1,1,1]);
        Assert.Equal(1000, score);
    }

    [Fact]
    public void CalculateScore_FourTwos()
    {
        var score = DicesCombinationsExtension.CalculateScore([2,2,2,2]);
        Assert.Equal(400, score);
    }

    [Fact]
    public void CalculateScore_SingleOneAndFive()
    {
        var score = DicesCombinationsExtension.CalculateScore([1,5]);
        Assert.Equal(150, score);
    }
}
