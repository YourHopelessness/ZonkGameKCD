using System;
using System.Collections.Generic;
using Xunit;
using ZonkGameCore.Utils;
using ZonkGame.DB.Enum;

namespace ZonkGame.FSM.Tests;

public class GameModeDefinerTests
{
    [Fact]
    public void ReturnsPvPForTwoRealPlayers()
    {
        var players = new[] { PlayerTypeEnum.RealPlayer, PlayerTypeEnum.RealPlayer };
        var result = GameModeDefiner.GetGameMode(players);
        Assert.Equal(ModesEnum.PvP, result);
    }

    [Fact]
    public void ReturnsPvEForRealAndAi()
    {
        var players = new[] { PlayerTypeEnum.RealPlayer, PlayerTypeEnum.AIAgent };
        var result = GameModeDefiner.GetGameMode(players);
        Assert.Equal(ModesEnum.PvE, result);
    }

    [Fact]
    public void ReturnsEvEForTwoAi()
    {
        var players = new[] { PlayerTypeEnum.AIAgent, PlayerTypeEnum.AIAgent };
        var result = GameModeDefiner.GetGameMode(players);
        Assert.Equal(ModesEnum.EvE, result);
    }

    [Fact]
    public void ThrowsForEmptyCollection()
    {
        Assert.Throws<ArgumentException>(() => GameModeDefiner.GetGameMode([]));
    }

    [Fact]
    public void ThrowsForNullInput()
    {
        Assert.Throws<ArgumentNullException>(() => GameModeDefiner.GetGameMode(null!));
    }
}
