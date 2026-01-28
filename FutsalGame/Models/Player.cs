using System.Windows;

namespace FutsalGame.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Point Position { get; set; }
    public Vector Velocity { get; set; }
    public Team Team { get; set; }
    public PlayerRole Role { get; set; }
    public double Speed { get; set; } = 2.0;
    public double MaxSpeed { get; set; } = 4.0;
    public double Stamina { get; set; } = 100.0;
    public bool HasBall { get; set; }
    
    public Player(int id, string name, Team team, PlayerRole role, Point startPosition)
    {
        Id = id;
        Name = name;
        Team = team;
        Role = role;
        Position = startPosition;
        Velocity = new Vector(0, 0);
    }

    public void Update(double deltaTime)
    {
        // Update position based on velocity
        Position = new Point(
            Position.X + Velocity.X * deltaTime,
            Position.Y + Velocity.Y * deltaTime
        );

        // Apply friction
        Velocity *= 0.95;

        // Regenerate stamina
        if (Stamina < 100)
        {
            Stamina += 0.1 * deltaTime;
            if (Stamina > 100) Stamina = 100;
        }
    }

    public void MoveTo(Vector direction)
    {
        if (Stamina > 0)
        {
            Velocity += direction * Speed;
            
            // Limit speed
            double currentSpeed = Velocity.Length;
            if (currentSpeed > MaxSpeed)
            {
                Velocity = Velocity / currentSpeed * MaxSpeed;
            }

            Stamina -= 0.05;
            if (Stamina < 0) Stamina = 0;
        }
    }
}

public enum Team
{
    Red,
    Blue
}

public enum PlayerRole
{
    Goalkeeper,
    Defender,
    Midfielder,
    Forward
}
