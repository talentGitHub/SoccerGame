using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FutsalGame.Models;

namespace FutsalGame.Engine;

public class GameEngine
{
    public List<Player> Players { get; private set; }
    public Ball Ball { get; private set; }
    public GameState GameState { get; private set; }
    public double FieldWidth { get; private set; }
    public double FieldHeight { get; private set; }
    
    private const double PlayerRadius = 15.0;
    private const double BallPickupDistance = 20.0;
    private const double GoalWidth = 120.0;
    private const double GoalDepth = 20.0;

    public GameEngine(double fieldWidth, double fieldHeight)
    {
        FieldWidth = fieldWidth;
        FieldHeight = fieldHeight;
        Players = new List<Player>();
        Ball = new Ball(new Point(fieldWidth / 2, fieldHeight / 2));
        GameState = new GameState();
        
        InitializePlayers();
    }

    private void InitializePlayers()
    {
        // Red Team (Left side)
        Players.Add(new Player(0, "Red GK", Team.Red, PlayerRole.Goalkeeper, 
            new Point(60, FieldHeight / 2)));
        Players.Add(new Player(1, "Red Def1", Team.Red, PlayerRole.Defender, 
            new Point(FieldWidth * 0.25, FieldHeight * 0.3)));
        Players.Add(new Player(2, "Red Def2", Team.Red, PlayerRole.Defender, 
            new Point(FieldWidth * 0.25, FieldHeight * 0.7)));
        Players.Add(new Player(3, "Red Mid", Team.Red, PlayerRole.Midfielder, 
            new Point(FieldWidth * 0.35, FieldHeight / 2)));
        Players.Add(new Player(4, "Red Fwd", Team.Red, PlayerRole.Forward, 
            new Point(FieldWidth * 0.45, FieldHeight / 2)));

        // Blue Team (Right side)
        Players.Add(new Player(5, "Blue GK", Team.Blue, PlayerRole.Goalkeeper, 
            new Point(FieldWidth - 60, FieldHeight / 2)));
        Players.Add(new Player(6, "Blue Def1", Team.Blue, PlayerRole.Defender, 
            new Point(FieldWidth * 0.75, FieldHeight * 0.3)));
        Players.Add(new Player(7, "Blue Def2", Team.Blue, PlayerRole.Defender, 
            new Point(FieldWidth * 0.75, FieldHeight * 0.7)));
        Players.Add(new Player(8, "Blue Mid", Team.Blue, PlayerRole.Midfielder, 
            new Point(FieldWidth * 0.65, FieldHeight / 2)));
        Players.Add(new Player(9, "Blue Fwd", Team.Blue, PlayerRole.Forward, 
            new Point(FieldWidth * 0.55, FieldHeight / 2)));
    }

    public void Update(double deltaTime)
    {
        if (GameState.IsPaused || GameState.IsGameOver)
            return;

        // Update game state
        GameState.Update(deltaTime);

        // Update all players
        foreach (var player in Players)
        {
            player.Update(deltaTime);
            
            // Keep players on field
            player.Position = new Point(
                Math.Clamp(player.Position.X, PlayerRadius, FieldWidth - PlayerRadius),
                Math.Clamp(player.Position.Y, PlayerRadius, FieldHeight - PlayerRadius)
            );
        }

        // Update ball
        Ball.Update(deltaTime);

        // Keep ball on field
        Ball.Position = new Point(
            Math.Clamp(Ball.Position.X, Ball.Radius, FieldWidth - Ball.Radius),
            Math.Clamp(Ball.Position.Y, Ball.Radius, FieldHeight - Ball.Radius)
        );

        // Check for ball pickup
        CheckBallPickup();

        // Check for goals
        CheckGoals();

        // Handle collisions
        HandleCollisions();
    }

    private void CheckBallPickup()
    {
        if (Ball.Owner != null)
            return;

        Player? closestPlayer = null;
        double closestDistance = double.MaxValue;

        foreach (var player in Players)
        {
            double distance = GetDistance(player.Position, Ball.Position);
            if (distance < BallPickupDistance && distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            Ball.SetOwner(closestPlayer);
        }
    }

    private void CheckGoals()
    {
        double goalTop = (FieldHeight - GoalWidth) / 2;
        double goalBottom = (FieldHeight + GoalWidth) / 2;

        // Check left goal (Red team defends)
        if (Ball.Position.X <= GoalDepth && 
            Ball.Position.Y >= goalTop && 
            Ball.Position.Y <= goalBottom)
        {
            GameState.AddScore(Team.Blue);
            ResetPositions();
        }

        // Check right goal (Blue team defends)
        if (Ball.Position.X >= FieldWidth - GoalDepth && 
            Ball.Position.Y >= goalTop && 
            Ball.Position.Y <= goalBottom)
        {
            GameState.AddScore(Team.Red);
            ResetPositions();
        }
    }

    private void HandleCollisions()
    {
        // Player-Player collisions
        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = i + 1; j < Players.Count; j++)
            {
                var p1 = Players[i];
                var p2 = Players[j];
                
                double distance = GetDistance(p1.Position, p2.Position);
                if (distance < PlayerRadius * 2)
                {
                    // Separate players
                    Vector direction = new Vector(
                        p2.Position.X - p1.Position.X,
                        p2.Position.Y - p1.Position.Y
                    );
                    direction.Normalize();
                    
                    double overlap = PlayerRadius * 2 - distance;
                    p1.Position = new Point(
                        p1.Position.X - direction.X * overlap / 2,
                        p1.Position.Y - direction.Y * overlap / 2
                    );
                    p2.Position = new Point(
                        p2.Position.X + direction.X * overlap / 2,
                        p2.Position.Y + direction.Y * overlap / 2
                    );
                }
            }
        }
    }

    public void KickBall(Player player, Vector direction, double power = 10.0)
    {
        if (player.HasBall && Ball.Owner == player)
        {
            Ball.Kick(direction, power);
            player.HasBall = false;
        }
    }

    public void ResetPositions()
    {
        Ball.Position = new Point(FieldWidth / 2, FieldHeight / 2);
        Ball.RemoveOwner();
        Ball.Velocity = new Vector(0, 0);

        // Reset players to starting positions
        int index = 0;
        foreach (var player in Players)
        {
            player.Velocity = new Vector(0, 0);
            player.HasBall = false;
            
            if (player.Team == Team.Red)
            {
                switch (player.Role)
                {
                    case PlayerRole.Goalkeeper:
                        player.Position = new Point(60, FieldHeight / 2);
                        break;
                    case PlayerRole.Defender:
                        player.Position = new Point(FieldWidth * 0.25, 
                            index % 2 == 0 ? FieldHeight * 0.3 : FieldHeight * 0.7);
                        break;
                    case PlayerRole.Midfielder:
                        player.Position = new Point(FieldWidth * 0.35, FieldHeight / 2);
                        break;
                    case PlayerRole.Forward:
                        player.Position = new Point(FieldWidth * 0.45, FieldHeight / 2);
                        break;
                }
            }
            else
            {
                switch (player.Role)
                {
                    case PlayerRole.Goalkeeper:
                        player.Position = new Point(FieldWidth - 60, FieldHeight / 2);
                        break;
                    case PlayerRole.Defender:
                        player.Position = new Point(FieldWidth * 0.75, 
                            index % 2 == 0 ? FieldHeight * 0.3 : FieldHeight * 0.7);
                        break;
                    case PlayerRole.Midfielder:
                        player.Position = new Point(FieldWidth * 0.65, FieldHeight / 2);
                        break;
                    case PlayerRole.Forward:
                        player.Position = new Point(FieldWidth * 0.55, FieldHeight / 2);
                        break;
                }
            }
            index++;
        }
    }

    public void ResetGame()
    {
        GameState.Reset();
        ResetPositions();
    }

    private double GetDistance(Point p1, Point p2)
    {
        double dx = p2.X - p1.X;
        double dy = p2.Y - p1.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public Player? GetPlayerNearestToBall(Team team)
    {
        return Players
            .Where(p => p.Team == team)
            .OrderBy(p => GetDistance(p.Position, Ball.Position))
            .FirstOrDefault();
    }
}
