using FutsalGame.Models;

namespace FutsalGame.Tests;

public class GameStateTests
{
    [Fact]
    public void GameState_Initialization_HasCorrectDefaults()
    {
        // Arrange & Act
        var gameState = new GameState();

        // Assert
        Assert.Equal(0, gameState.RedTeamScore);
        Assert.Equal(0, gameState.BlueTeamScore);
        Assert.Equal(0, gameState.GameTime);
        Assert.Equal(600.0, gameState.MaxGameTime);
        Assert.False(gameState.IsGameOver);
        Assert.False(gameState.IsPaused);
    }

    [Fact]
    public void GameState_AddScore_IncreasesTeamScore()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.AddScore(Team.Red);
        gameState.AddScore(Team.Red);
        gameState.AddScore(Team.Blue);

        // Assert
        Assert.Equal(2, gameState.RedTeamScore);
        Assert.Equal(1, gameState.BlueTeamScore);
    }

    [Fact]
    public void GameState_Update_IncreasesGameTime()
    {
        // Arrange
        var gameState = new GameState();

        // Act
        gameState.Update(1.0);
        gameState.Update(2.5);

        // Assert
        Assert.Equal(3.5, gameState.GameTime);
    }

    [Fact]
    public void GameState_Update_SetsGameOverWhenTimeExpires()
    {
        // Arrange
        var gameState = new GameState();
        gameState.MaxGameTime = 10.0;

        // Act
        gameState.Update(11.0);

        // Assert
        Assert.True(gameState.IsGameOver);
    }

    [Fact]
    public void GameState_Update_DoesNotUpdateWhenPaused()
    {
        // Arrange
        var gameState = new GameState();
        gameState.IsPaused = true;

        // Act
        gameState.Update(5.0);

        // Assert
        Assert.Equal(0, gameState.GameTime);
    }

    [Fact]
    public void GameState_Update_DoesNotUpdateWhenGameOver()
    {
        // Arrange
        var gameState = new GameState();
        gameState.IsGameOver = true;

        // Act
        gameState.Update(5.0);

        // Assert
        Assert.Equal(0, gameState.GameTime);
    }

    [Fact]
    public void GameState_GetTimeString_FormatsCorrectly()
    {
        // Arrange
        var gameState = new GameState();
        gameState.GameTime = 125.7; // 2 minutes, 5 seconds

        // Act
        string timeString = gameState.GetTimeString();

        // Assert
        Assert.Equal("02:05", timeString);
    }

    [Fact]
    public void GameState_Reset_ResetsAllProperties()
    {
        // Arrange
        var gameState = new GameState();
        gameState.AddScore(Team.Red);
        gameState.AddScore(Team.Blue);
        gameState.Update(100.0);
        gameState.IsPaused = true;
        gameState.IsGameOver = true;

        // Act
        gameState.Reset();

        // Assert
        Assert.Equal(0, gameState.RedTeamScore);
        Assert.Equal(0, gameState.BlueTeamScore);
        Assert.Equal(0, gameState.GameTime);
        Assert.False(gameState.IsPaused);
        Assert.False(gameState.IsGameOver);
    }
}
