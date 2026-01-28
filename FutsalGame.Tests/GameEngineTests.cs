using System.Windows;
using FutsalGame.Engine;
using FutsalGame.Models;

namespace FutsalGame.Tests;

public class GameEngineTests
{
    [Fact]
    public void GameEngine_Initialization_CreatesCorrectNumberOfPlayers()
    {
        // Arrange & Act
        var engine = new GameEngine(1000, 600);

        // Assert
        Assert.Equal(10, engine.Players.Count);
        Assert.Equal(5, engine.Players.Count(p => p.Team == Team.Red));
        Assert.Equal(5, engine.Players.Count(p => p.Team == Team.Blue));
    }

    [Fact]
    public void GameEngine_Initialization_CreatesCorrectPlayerRoles()
    {
        // Arrange & Act
        var engine = new GameEngine(1000, 600);

        // Assert
        // Each team should have 1 goalkeeper, 2 defenders, 1 midfielder, 1 forward
        Assert.Equal(2, engine.Players.Count(p => p.Role == PlayerRole.Goalkeeper));
        Assert.Equal(4, engine.Players.Count(p => p.Role == PlayerRole.Defender));
        Assert.Equal(2, engine.Players.Count(p => p.Role == PlayerRole.Midfielder));
        Assert.Equal(2, engine.Players.Count(p => p.Role == PlayerRole.Forward));
    }

    [Fact]
    public void GameEngine_Update_UpdatesAllPlayers()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);
        var player = engine.Players[0];
        player.Velocity = new Vector(10, 10);
        var initialPosition = player.Position;

        // Act
        engine.Update(1.0);

        // Assert
        Assert.NotEqual(initialPosition, player.Position);
    }

    [Fact]
    public void GameEngine_Update_UpdatesBall()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);
        engine.Ball.Velocity = new Vector(10, 10);
        var initialPosition = engine.Ball.Position;

        // Act
        engine.Update(1.0);

        // Assert
        Assert.NotEqual(initialPosition.X, engine.Ball.Position.X);
        Assert.NotEqual(initialPosition.Y, engine.Ball.Position.Y);
    }

    [Fact]
    public void GameEngine_Update_UpdatesGameState()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);

        // Act
        engine.Update(1.0);

        // Assert
        Assert.Equal(1.0, engine.GameState.GameTime);
    }

    [Fact]
    public void GameEngine_KickBall_WorksWhenPlayerHasBall()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);
        var player = engine.Players[0];
        engine.Ball.SetOwner(player);

        // Act
        engine.KickBall(player, new Vector(1, 0), 10.0);

        // Assert
        Assert.Null(engine.Ball.Owner);
        Assert.False(player.HasBall);
        Assert.True(engine.Ball.Velocity.Length > 0);
    }

    [Fact]
    public void GameEngine_ResetPositions_MovesPlayersToStartingPositions()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);
        var player = engine.Players[0];
        var initialPosition = player.Position;
        player.Position = new Point(999, 999);

        // Act
        engine.ResetPositions();

        // Assert
        Assert.NotEqual(999, player.Position.X);
        Assert.NotEqual(999, player.Position.Y);
    }

    [Fact]
    public void GameEngine_ResetPositions_ResetsBallToCenterAndClearsOwner()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);
        var player = engine.Players[0];
        engine.Ball.SetOwner(player);
        engine.Ball.Velocity = new Vector(10, 10);

        // Act
        engine.ResetPositions();

        // Assert
        Assert.Equal(500, engine.Ball.Position.X);
        Assert.Equal(300, engine.Ball.Position.Y);
        Assert.Null(engine.Ball.Owner);
        Assert.Equal(0, engine.Ball.Velocity.X);
        Assert.Equal(0, engine.Ball.Velocity.Y);
    }

    [Fact]
    public void GameEngine_ResetGame_ResetsGameStateAndPositions()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);
        engine.GameState.AddScore(Team.Red);
        engine.GameState.Update(100.0);

        // Act
        engine.ResetGame();

        // Assert
        Assert.Equal(0, engine.GameState.RedTeamScore);
        Assert.Equal(0, engine.GameState.GameTime);
        Assert.Equal(500, engine.Ball.Position.X);
    }

    [Fact]
    public void GameEngine_GetPlayerNearestToBall_ReturnsCorrectPlayer()
    {
        // Arrange
        var engine = new GameEngine(1000, 600);
        engine.Ball.Position = new Point(100, 100);
        
        // Move one red player very close to the ball
        var nearPlayer = engine.Players.First(p => p.Team == Team.Red);
        nearPlayer.Position = new Point(105, 105);

        // Act
        var nearest = engine.GetPlayerNearestToBall(Team.Red);

        // Assert
        Assert.Equal(nearPlayer.Id, nearest?.Id);
    }
}
