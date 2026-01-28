using System.Windows;
using FutsalGame.Models;

namespace FutsalGame.Tests;

public class BallTests
{
    [Fact]
    public void Ball_Initialization_SetsCorrectProperties()
    {
        // Arrange & Act
        var ball = new Ball(new Point(100, 100));

        // Assert
        Assert.Equal(100, ball.Position.X);
        Assert.Equal(100, ball.Position.Y);
        Assert.Null(ball.Owner);
        Assert.Equal(10.0, ball.Radius);
    }

    [Fact]
    public void Ball_Update_MovesWhenNoOwner()
    {
        // Arrange
        var ball = new Ball(new Point(100, 100));
        ball.Velocity = new Vector(10, 5);

        // Act
        ball.Update(1.0);

        // Assert
        Assert.Equal(110, ball.Position.X);
        Assert.Equal(105, ball.Position.Y);
    }

    [Fact]
    public void Ball_Update_FollowsOwnerWhenAssigned()
    {
        // Arrange
        var ball = new Ball(new Point(100, 100));
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(200, 200));
        ball.SetOwner(player);

        // Act
        ball.Update(1.0);

        // Assert
        Assert.Equal(200, ball.Position.X);
        Assert.Equal(200, ball.Position.Y);
        Assert.True(player.HasBall);
    }

    [Fact]
    public void Ball_Kick_RemovesOwnerAndSetsVelocity()
    {
        // Arrange
        var ball = new Ball(new Point(100, 100));
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(100, 100));
        ball.SetOwner(player);

        // Act
        ball.Kick(new Vector(1, 0), 10.0);

        // Assert
        Assert.Null(ball.Owner);
        Assert.Equal(10.0, ball.Velocity.X);
    }

    [Fact]
    public void Ball_Update_AppliesFriction()
    {
        // Arrange
        var ball = new Ball(new Point(100, 100));
        ball.Velocity = new Vector(10, 10);

        // Act
        ball.Update(1.0);
        double velocityAfterUpdate = ball.Velocity.Length;

        // Assert
        // Velocity should be reduced due to friction (multiplied by 0.98)
        Assert.True(velocityAfterUpdate < Math.Sqrt(10 * 10 + 10 * 10));
    }

    [Fact]
    public void Ball_Update_StopsWhenSlowEnough()
    {
        // Arrange
        var ball = new Ball(new Point(100, 100));
        ball.Velocity = new Vector(0.05, 0.05);

        // Act
        ball.Update(1.0);

        // Assert
        Assert.Equal(0, ball.Velocity.X);
        Assert.Equal(0, ball.Velocity.Y);
    }

    [Fact]
    public void Ball_RemoveOwner_ClearsOwnershipAndPlayerState()
    {
        // Arrange
        var ball = new Ball(new Point(100, 100));
        var player = new Player(1, "Test", Team.Red, PlayerRole.Forward, new Point(100, 100));
        ball.SetOwner(player);

        // Act
        ball.RemoveOwner();

        // Assert
        Assert.Null(ball.Owner);
        Assert.False(player.HasBall);
    }
}
