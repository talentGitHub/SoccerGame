using System.Windows;
using FutsalGame.Models;

namespace FutsalGame.Tests;

public class PlayerTests
{
    [Fact]
    public void Player_Initialization_SetsCorrectProperties()
    {
        // Arrange & Act
        var player = new Player(1, "Test Player", Team.Red, PlayerRole.Forward, new Point(100, 100));

        // Assert
        Assert.Equal(1, player.Id);
        Assert.Equal("Test Player", player.Name);
        Assert.Equal(Team.Red, player.Team);
        Assert.Equal(PlayerRole.Forward, player.Role);
        Assert.Equal(100, player.Position.X);
        Assert.Equal(100, player.Position.Y);
        Assert.Equal(100, player.Stamina);
        Assert.False(player.HasBall);
    }

    [Fact]
    public void Player_Update_MovesPlayerBasedOnVelocity()
    {
        // Arrange
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(100, 100));
        player.Velocity = new Vector(10, 5);
        double deltaTime = 1.0;

        // Act
        player.Update(deltaTime);

        // Assert
        Assert.Equal(110, player.Position.X);
        Assert.Equal(105, player.Position.Y);
    }

    [Fact]
    public void Player_Update_AppliesFriction()
    {
        // Arrange
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(100, 100));
        player.Velocity = new Vector(10, 10);

        // Act
        player.Update(1.0);
        double velocityAfterUpdate = player.Velocity.Length;

        // Assert
        // Velocity should be reduced due to friction (multiplied by 0.95)
        Assert.True(velocityAfterUpdate < Math.Sqrt(10 * 10 + 10 * 10));
    }

    [Fact]
    public void Player_MoveTo_IncreasesVelocity()
    {
        // Arrange
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(100, 100));
        var direction = new Vector(1, 0);

        // Act
        player.MoveTo(direction);

        // Assert
        Assert.True(player.Velocity.X > 0);
        Assert.True(player.Stamina < 100);
    }

    [Fact]
    public void Player_MoveTo_LimitsMaxSpeed()
    {
        // Arrange
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(100, 100));
        var direction = new Vector(1, 0);

        // Act - Move many times to exceed max speed
        for (int i = 0; i < 100; i++)
        {
            player.MoveTo(direction);
        }

        // Assert
        Assert.True(player.Velocity.Length <= player.MaxSpeed);
    }

    [Fact]
    public void Player_StaminaRegenerates()
    {
        // Arrange
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(100, 100));
        player.Stamina = 50;

        // Act
        player.Update(1.0);

        // Assert
        Assert.True(player.Stamina > 50);
    }
}
