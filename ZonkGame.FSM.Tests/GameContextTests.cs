using System;
using System.Collections.Generic;
using Xunit;
using ZonkGameCore.Context;
using ZonkGameCore.Model;
using ZonkGameCore.InputParams;
using ZonkGame.DB.Enum;

namespace ZonkGame.FSM.Tests;

public class GameContextTests
{
    private static PlayerState CreatePlayer(string name)
    {
        return new PlayerState(new InputPlayerModel(name, new StubInputHandler(), PlayerTypeEnum.RealPlayer, null));
    }

    [Fact]
    public void NextPlayer_SwitchesCurrentPlayer()
    {
        var p1 = CreatePlayer("p1");
        var p2 = CreatePlayer("p2");
        var context = new GameContext(10000, [], p1, new List<PlayerState> { p1, p2 }, Guid.NewGuid());
        p1.TurnScore = 10;
        p1.SubstructDices(2);
        var next = context.NextPlayer();
        Assert.Equal(p2.PlayerId, next.PlayerId);
        Assert.Equal(p2, context.CurrentPlayer);
        Assert.Equal(6, p1.RemainingDice);
        Assert.Equal(0, p1.TurnScore);
    }

    [Fact]
    public void GetOpponentPlayer_ReturnsOtherPlayer()
    {
        var p1 = CreatePlayer("p1");
        var p2 = CreatePlayer("p2");
        var context = new GameContext(10000, [], p1, new List<PlayerState> { p1, p2 }, Guid.NewGuid());
        var opponent = context.GetOpponentPlayer();
        Assert.Equal(p2.PlayerId, opponent.PlayerId);
    }
}
