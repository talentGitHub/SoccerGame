using System.Windows;

namespace FutsalGame.Models;

public class Ball
{
    public Point Position { get; set; }
    public Vector Velocity { get; set; }
    public double Radius { get; set; } = 10.0;
    public Player? Owner { get; set; }

    public Ball(Point startPosition)
    {
        Position = startPosition;
        Velocity = new Vector(0, 0);
    }

    public void Update(double deltaTime)
    {
        if (Owner != null)
        {
            // Ball follows the owner
            Position = Owner.Position;
        }
        else
        {
            // Ball moves freely
            Position = new Point(
                Position.X + Velocity.X * deltaTime,
                Position.Y + Velocity.Y * deltaTime
            );

            // Apply friction (delta-time adjusted for consistent physics)
            Velocity *= Math.Pow(0.98, deltaTime * 60);

            // Stop if very slow
            if (Velocity.Length < 0.1)
            {
                Velocity = new Vector(0, 0);
            }
        }
    }

    public void Kick(Vector direction, double power)
    {
        Owner = null;
        Velocity = direction * power;
    }

    public void SetOwner(Player player)
    {
        Owner = player;
        player.HasBall = true;
    }

    public void RemoveOwner()
    {
        if (Owner != null)
        {
            Owner.HasBall = false;
            Owner = null;
        }
    }
}
