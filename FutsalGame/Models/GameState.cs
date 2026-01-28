namespace FutsalGame.Models;

public class GameState
{
    public int RedTeamScore { get; set; }
    public int BlueTeamScore { get; set; }
    public double GameTime { get; set; }
    public double MaxGameTime { get; set; } = 600.0; // 10 minutes
    public bool IsGameOver { get; set; }
    public bool IsPaused { get; set; }
    
    public void AddScore(Team team)
    {
        if (team == Team.Red)
            RedTeamScore++;
        else
            BlueTeamScore++;
    }

    public void Update(double deltaTime)
    {
        if (!IsPaused && !IsGameOver)
        {
            GameTime += deltaTime;
            if (GameTime >= MaxGameTime)
            {
                IsGameOver = true;
            }
        }
    }

    public string GetTimeString()
    {
        int minutes = (int)(GameTime / 60);
        int seconds = (int)(GameTime % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }

    public void Reset()
    {
        RedTeamScore = 0;
        BlueTeamScore = 0;
        GameTime = 0;
        IsGameOver = false;
        IsPaused = false;
    }
}
