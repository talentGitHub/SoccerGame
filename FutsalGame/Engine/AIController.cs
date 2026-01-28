using System;
using System.Windows;
using FutsalGame.Models;

namespace FutsalGame.Engine;

public class AIController
{
    private readonly GameEngine _engine;
    private readonly Random _random = new Random();

    public AIController(GameEngine engine)
    {
        _engine = engine;
    }

    public void UpdateAI(Team team)
    {
        foreach (var player in _engine.Players)
        {
            if (player.Team == team)
            {
                UpdatePlayerAI(player);
            }
        }
    }

    public void UpdateAI(Team team, Player? excludePlayer)
    {
        foreach (var player in _engine.Players)
        {
            if (player.Team == team && player.Id != excludePlayer?.Id)
            {
                UpdatePlayerAI(player);
            }
        }
    }

    private void UpdatePlayerAI(Player player)
    {
        if (player.HasBall)
        {
            // Player has the ball - decide to pass or shoot
            HandlePlayerWithBall(player);
        }
        else
        {
            // Player doesn't have ball - position or chase
            HandlePlayerWithoutBall(player);
        }
    }

    private void HandlePlayerWithBall(Player player)
    {
        Point ballPos = player.Position;
        Point goalPos;

        // Determine opponent's goal position
        if (player.Team == Team.Red)
        {
            goalPos = new Point(_engine.FieldWidth - 20, _engine.FieldHeight / 2);
        }
        else
        {
            goalPos = new Point(20, _engine.FieldHeight / 2);
        }

        // Calculate direction to goal
        Vector toGoal = new Vector(goalPos.X - ballPos.X, goalPos.Y - ballPos.Y);
        double distanceToGoal = toGoal.Length;
        
        // If close to goal, shoot
        if (distanceToGoal < 250 && player.Role != PlayerRole.Goalkeeper)
        {
            toGoal.Normalize();
            // Add some randomness to the shot
            double angle = (_random.NextDouble() - 0.5) * 0.3;
            Vector shootDir = RotateVector(toGoal, angle);
            _engine.KickBall(player, shootDir, 15.0);
        }
        else if (_random.NextDouble() < 0.02) // 2% chance to pass per update
        {
            // Look for a teammate to pass to
            Player? bestTeammate = null;
            double bestScore = double.MinValue;

            foreach (var teammate in _engine.Players)
            {
                if (teammate.Team == player.Team && teammate.Id != player.Id)
                {
                    double distance = GetDistance(player.Position, teammate.Position);
                    Vector toTeammate = new Vector(
                        teammate.Position.X - player.Position.X,
                        teammate.Position.Y - player.Position.Y
                    );
                    toTeammate.Normalize();
                    
                    // Prefer teammates closer to goal
                    double teammateDistToGoal = GetDistance(teammate.Position, goalPos);
                    double score = (distanceToGoal - teammateDistToGoal) / distance;

                    if (score > bestScore && distance > 50 && distance < 300)
                    {
                        bestScore = score;
                        bestTeammate = teammate;
                    }
                }
            }

            if (bestTeammate != null)
            {
                Vector passDir = new Vector(
                    bestTeammate.Position.X - player.Position.X,
                    bestTeammate.Position.Y - player.Position.Y
                );
                passDir.Normalize();
                _engine.KickBall(player, passDir, 8.0);
            }
        }
        else
        {
            // Move towards goal
            toGoal.Normalize();
            player.MoveTo(toGoal);
        }
    }

    private void HandlePlayerWithoutBall(Player player)
    {
        Point ballPos = _engine.Ball.Position;
        double distanceToBall = GetDistance(player.Position, ballPos);

        if (player.Role == PlayerRole.Goalkeeper)
        {
            // Goalkeeper stays near goal
            Point goalPos;
            if (player.Team == Team.Red)
            {
                goalPos = new Point(60, _engine.FieldHeight / 2);
            }
            else
            {
                goalPos = new Point(_engine.FieldWidth - 60, _engine.FieldHeight / 2);
            }

            // Only move if ball is nearby
            if (distanceToBall < 200)
            {
                Vector toBall = new Vector(ballPos.X - player.Position.X, ballPos.Y - player.Position.Y);
                toBall.Normalize();
                player.MoveTo(toBall);
            }
            else
            {
                // Return to goal position
                Vector toGoal = new Vector(goalPos.X - player.Position.X, goalPos.Y - player.Position.Y);
                if (toGoal.Length > 10)
                {
                    toGoal.Normalize();
                    player.MoveTo(toGoal);
                }
            }
        }
        else
        {
            // Determine if this player should chase the ball
            var nearestTeammate = _engine.GetPlayerNearestToBall(player.Team);
            
            if (nearestTeammate?.Id == player.Id || distanceToBall < 150)
            {
                // Chase the ball
                Vector toBall = new Vector(ballPos.X - player.Position.X, ballPos.Y - player.Position.Y);
                toBall.Normalize();
                player.MoveTo(toBall);
            }
            else
            {
                // Position according to role
                Point targetPos = GetPositionForRole(player);
                Vector toTarget = new Vector(targetPos.X - player.Position.X, targetPos.Y - player.Position.Y);
                
                if (toTarget.Length > 30)
                {
                    toTarget.Normalize();
                    player.MoveTo(toTarget * 0.5);
                }
            }
        }
    }

    private Point GetPositionForRole(Player player)
    {
        double xPos = 0;
        double yPos = _engine.FieldHeight / 2;

        if (player.Team == Team.Red)
        {
            switch (player.Role)
            {
                case PlayerRole.Defender:
                    xPos = _engine.FieldWidth * 0.25;
                    yPos = player.Id % 2 == 0 ? _engine.FieldHeight * 0.35 : _engine.FieldHeight * 0.65;
                    break;
                case PlayerRole.Midfielder:
                    xPos = _engine.FieldWidth * 0.4;
                    break;
                case PlayerRole.Forward:
                    xPos = _engine.FieldWidth * 0.55;
                    break;
            }
        }
        else
        {
            switch (player.Role)
            {
                case PlayerRole.Defender:
                    xPos = _engine.FieldWidth * 0.75;
                    yPos = player.Id % 2 == 0 ? _engine.FieldHeight * 0.35 : _engine.FieldHeight * 0.65;
                    break;
                case PlayerRole.Midfielder:
                    xPos = _engine.FieldWidth * 0.6;
                    break;
                case PlayerRole.Forward:
                    xPos = _engine.FieldWidth * 0.45;
                    break;
            }
        }

        return new Point(xPos, yPos);
    }

    private double GetDistance(Point p1, Point p2)
    {
        double dx = p2.X - p1.X;
        double dy = p2.Y - p1.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private Vector RotateVector(Vector v, double angle)
    {
        double cos = Math.Cos(angle);
        double sin = Math.Sin(angle);
        return new Vector(
            v.X * cos - v.Y * sin,
            v.X * sin + v.Y * cos
        );
    }
}
