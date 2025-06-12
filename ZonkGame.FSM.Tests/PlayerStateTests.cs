using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ZonkGameCore.Context;
using ZonkGameCore.Model;
using ZonkGameCore.InputParams;
using ZonkGame.DB.Enum;

namespace ZonkGame.FSM.Tests;

public class StubInputHandler : IInputAsyncHandler
{
    public Task<IEnumerable<int>?> HandleSelectDiceInputAsync(IEnumerable<int> roll, Guid gameId, Guid playerId) => Task.FromResult<IEnumerable<int>?>(null);
    public Task<bool?> HandleShouldContinueGameInputAsync(Guid gameid, Guid playerId) => Task.FromResult<bool?>(null);
}

public class PlayerStateTests
{
    [Fact]
    public void SubstructDices_DecreasesRemaining()
    {
        var player = new PlayerState(new InputPlayerModel("p1", new StubInputHandler(), PlayerTypeEnum.RealPlayer, null));
        player.SubstructDices(2);
        Assert.Equal(4, player.RemainingDice);
    }

    [Fact]
    public void SubstructDices_ThrowsWhenBelowZero()
    {
        var player = new PlayerState(new InputPlayerModel("p1", new StubInputHandler(), PlayerTypeEnum.RealPlayer, null));
        Assert.Throws<InvalidOperationException>(() => player.SubstructDices(7));
    }

    [Fact]
    public void ResetDices_SetsRemainingToSix()
    {
        var player = new PlayerState(new InputPlayerModel("p1", new StubInputHandler(), PlayerTypeEnum.RealPlayer, null));
        player.SubstructDices(3);
        player.ResetDices();
        Assert.Equal(6, player.RemainingDice);
    }

    [Fact]
    public void AddingTotalScore_AddsTurnScore()
    {
        var player = new PlayerState(new InputPlayerModel("p1", new StubInputHandler(), PlayerTypeEnum.RealPlayer, null));
        player.TurnScore = 150;
        player.AddingTotalScore();
        Assert.Equal(150, player.TotalScore);
    }
}
