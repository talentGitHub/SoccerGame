using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using FutsalGame.Engine;
using FutsalGame.Models;

namespace FutsalGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private GameEngine _gameEngine = null!;
    private AIController _aiController = null!;
    private DispatcherTimer _gameTimer = null!;
    private DateTime _lastUpdate;
    
    private Player? _selectedPlayer;
    private bool _isWPressed, _isAPressed, _isSPressed, _isDPressed;
    
    private const double FieldWidth = 1000;
    private const double FieldHeight = 600;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGame();
    }

    private void InitializeGame()
    {
        _gameEngine = new GameEngine(FieldWidth, FieldHeight);
        _aiController = new AIController(_gameEngine);
        
        // Select first red team player as controlled player
        _selectedPlayer = _gameEngine.Players[4]; // Red Forward

        // Setup game timer
        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };
        _gameTimer.Tick += GameLoop;
        _lastUpdate = DateTime.Now;

        DrawField();
        UpdateUI();
    }

    private void DrawField()
    {
        GameCanvas.Children.Clear();

        // Field background (already green from Canvas.Background)
        
        // Draw field lines
        var lineBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)) { Opacity = 0.7 };
        double lineThickness = 3;

        // Outer boundary
        var boundary = new Rectangle
        {
            Width = FieldWidth,
            Height = FieldHeight,
            Stroke = lineBrush,
            StrokeThickness = lineThickness,
            Fill = Brushes.Transparent
        };
        Canvas.SetLeft(boundary, 0);
        Canvas.SetTop(boundary, 0);
        GameCanvas.Children.Add(boundary);

        // Center line
        var centerLine = new Line
        {
            X1 = FieldWidth / 2,
            Y1 = 0,
            X2 = FieldWidth / 2,
            Y2 = FieldHeight,
            Stroke = lineBrush,
            StrokeThickness = lineThickness
        };
        GameCanvas.Children.Add(centerLine);

        // Center circle
        var centerCircle = new Ellipse
        {
            Width = 120,
            Height = 120,
            Stroke = lineBrush,
            StrokeThickness = lineThickness,
            Fill = Brushes.Transparent
        };
        Canvas.SetLeft(centerCircle, FieldWidth / 2 - 60);
        Canvas.SetTop(centerCircle, FieldHeight / 2 - 60);
        GameCanvas.Children.Add(centerCircle);

        // Center spot
        var centerSpot = new Ellipse
        {
            Width = 8,
            Height = 8,
            Fill = lineBrush
        };
        Canvas.SetLeft(centerSpot, FieldWidth / 2 - 4);
        Canvas.SetTop(centerSpot, FieldHeight / 2 - 4);
        GameCanvas.Children.Add(centerSpot);

        // Goals
        double goalWidth = 120;
        double goalDepth = 20;
        double goalTop = (FieldHeight - goalWidth) / 2;

        // Left goal (Red defends)
        var leftGoal = new Rectangle
        {
            Width = goalDepth,
            Height = goalWidth,
            Stroke = lineBrush,
            StrokeThickness = lineThickness,
            Fill = new SolidColorBrush(Color.FromRgb(255, 100, 100)) { Opacity = 0.3 }
        };
        Canvas.SetLeft(leftGoal, 0);
        Canvas.SetTop(leftGoal, goalTop);
        GameCanvas.Children.Add(leftGoal);

        // Right goal (Blue defends)
        var rightGoal = new Rectangle
        {
            Width = goalDepth,
            Height = goalWidth,
            Stroke = lineBrush,
            StrokeThickness = lineThickness,
            Fill = new SolidColorBrush(Color.FromRgb(100, 100, 255)) { Opacity = 0.3 }
        };
        Canvas.SetLeft(rightGoal, FieldWidth - goalDepth);
        Canvas.SetTop(rightGoal, goalTop);
        GameCanvas.Children.Add(rightGoal);

        // Penalty areas
        double penaltyWidth = 180;
        double penaltyDepth = 80;
        double penaltyTop = (FieldHeight - penaltyWidth) / 2;

        // Left penalty area
        var leftPenalty = new Rectangle
        {
            Width = penaltyDepth,
            Height = penaltyWidth,
            Stroke = lineBrush,
            StrokeThickness = lineThickness,
            Fill = Brushes.Transparent
        };
        Canvas.SetLeft(leftPenalty, 0);
        Canvas.SetTop(leftPenalty, penaltyTop);
        GameCanvas.Children.Add(leftPenalty);

        // Right penalty area
        var rightPenalty = new Rectangle
        {
            Width = penaltyDepth,
            Height = penaltyWidth,
            Stroke = lineBrush,
            StrokeThickness = lineThickness,
            Fill = Brushes.Transparent
        };
        Canvas.SetLeft(rightPenalty, FieldWidth - penaltyDepth);
        Canvas.SetTop(rightPenalty, penaltyTop);
        GameCanvas.Children.Add(rightPenalty);
    }

    private void GameLoop(object? sender, EventArgs e)
    {
        var now = DateTime.Now;
        double deltaTime = (now - _lastUpdate).TotalSeconds;
        _lastUpdate = now;

        // Handle player input
        if (_selectedPlayer != null)
        {
            Vector movement = new Vector(0, 0);
            if (_isWPressed) movement.Y -= 1;
            if (_isSPressed) movement.Y += 1;
            if (_isAPressed) movement.X -= 1;
            if (_isDPressed) movement.X += 1;

            if (movement.Length > 0)
            {
                movement.Normalize();
                _selectedPlayer.MoveTo(movement);
            }
        }

        // Update AI for blue team
        _aiController.UpdateAI(Team.Blue);
        
        // Update AI for other red team players (excluding the player-controlled one)
        _aiController.UpdateAI(Team.Red, _selectedPlayer);

        // Update game engine
        _gameEngine.Update(deltaTime);

        // Render
        Render();
        UpdateUI();
    }

    private void Render()
    {
        // Remove old sprites (keep field elements)
        var elementsToRemove = GameCanvas.Children
            .OfType<UIElement>()
            .Where(e => e.GetValue(TagProperty) as string == "Sprite")
            .ToList();
        
        foreach (var element in elementsToRemove)
        {
            GameCanvas.Children.Remove(element);
        }

        // Draw players
        foreach (var player in _gameEngine.Players)
        {
            DrawPlayer(player);
        }

        // Draw ball
        DrawBall();
    }

    private void DrawPlayer(Player player)
    {
        double radius = 15;
        
        // Player circle
        var playerCircle = new Ellipse
        {
            Width = radius * 2,
            Height = radius * 2,
            Tag = "Sprite"
        };

        // Set color based on team
        Color playerColor;
        if (player.Team == Team.Red)
        {
            playerColor = Color.FromRgb(220, 50, 50);
        }
        else
        {
            playerColor = Color.FromRgb(50, 50, 220);
        }

        // Highlight selected player
        if (player == _selectedPlayer)
        {
            playerCircle.Stroke = Brushes.Yellow;
            playerCircle.StrokeThickness = 3;
        }
        else
        {
            playerCircle.Stroke = Brushes.Black;
            playerCircle.StrokeThickness = 2;
        }

        // Add gradient for better visuals
        var gradient = new RadialGradientBrush();
        gradient.GradientStops.Add(new GradientStop(Colors.White, 0.0));
        gradient.GradientStops.Add(new GradientStop(playerColor, 0.4));
        gradient.GradientStops.Add(new GradientStop(Color.FromRgb(
            (byte)(playerColor.R * 0.7),
            (byte)(playerColor.G * 0.7),
            (byte)(playerColor.B * 0.7)), 1.0));
        
        playerCircle.Fill = gradient;

        Canvas.SetLeft(playerCircle, player.Position.X - radius);
        Canvas.SetTop(playerCircle, player.Position.Y - radius);
        GameCanvas.Children.Add(playerCircle);

        // Ball indicator
        if (player.HasBall)
        {
            var ballIndicator = new Ellipse
            {
                Width = radius * 2.5,
                Height = radius * 2.5,
                Stroke = Brushes.White,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection { 2, 2 },
                Fill = Brushes.Transparent,
                Tag = "Sprite"
            };
            Canvas.SetLeft(ballIndicator, player.Position.X - radius * 1.25);
            Canvas.SetTop(ballIndicator, player.Position.Y - radius * 1.25);
            GameCanvas.Children.Add(ballIndicator);
        }

        // Player number/role indicator
        var roleText = new TextBlock
        {
            Text = player.Role switch
            {
                PlayerRole.Goalkeeper => "GK",
                PlayerRole.Defender => "D",
                PlayerRole.Midfielder => "M",
                PlayerRole.Forward => "F",
                _ => "?"
            },
            FontSize = 10,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.White,
            Tag = "Sprite"
        };
        Canvas.SetLeft(roleText, player.Position.X - 8);
        Canvas.SetTop(roleText, player.Position.Y - 6);
        GameCanvas.Children.Add(roleText);
    }

    private void DrawBall()
    {
        double radius = 10;
        
        var ballCircle = new Ellipse
        {
            Width = radius * 2,
            Height = radius * 2,
            Tag = "Sprite"
        };

        // Ball gradient
        var gradient = new RadialGradientBrush();
        gradient.GradientStops.Add(new GradientStop(Colors.White, 0.0));
        gradient.GradientStops.Add(new GradientStop(Color.FromRgb(230, 230, 230), 0.7));
        gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 180, 180), 1.0));
        
        ballCircle.Fill = gradient;
        ballCircle.Stroke = Brushes.Black;
        ballCircle.StrokeThickness = 2;

        Canvas.SetLeft(ballCircle, _gameEngine.Ball.Position.X - radius);
        Canvas.SetTop(ballCircle, _gameEngine.Ball.Position.Y - radius);
        GameCanvas.Children.Add(ballCircle);
    }

    private void UpdateUI()
    {
        RedScoreText.Text = _gameEngine.GameState.RedTeamScore.ToString();
        BlueScoreText.Text = _gameEngine.GameState.BlueTeamScore.ToString();
        TimerText.Text = _gameEngine.GameState.GetTimeString();

        if (_gameEngine.GameState.IsGameOver)
        {
            GameStatusText.Text = _gameEngine.GameState.RedTeamScore > _gameEngine.GameState.BlueTeamScore
                ? "RED TEAM WINS! Press R to restart"
                : _gameEngine.GameState.BlueTeamScore > _gameEngine.GameState.RedTeamScore
                    ? "BLUE TEAM WINS! Press R to restart"
                    : "DRAW! Press R to restart";
        }
        else if (_gameEngine.GameState.IsPaused)
        {
            GameStatusText.Text = "PAUSED - Press SPACE to resume";
        }
        else if (_gameTimer.IsEnabled)
        {
            GameStatusText.Text = "Playing - Press SPACE to pause";
        }
        else
        {
            GameStatusText.Text = "Press SPACE to start";
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _isWPressed = true;
                break;
            case Key.A:
                _isAPressed = true;
                break;
            case Key.S:
                _isSPressed = true;
                break;
            case Key.D:
                _isDPressed = true;
                break;
            case Key.Space:
                HandleSpaceKey();
                e.Handled = true;
                break;
            case Key.R:
                ResetGame();
                e.Handled = true;
                break;
        }
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _isWPressed = false;
                break;
            case Key.A:
                _isAPressed = false;
                break;
            case Key.S:
                _isSPressed = false;
                break;
            case Key.D:
                _isDPressed = false;
                break;
        }
    }

    private void HandleSpaceKey()
    {
        if (_gameEngine.GameState.IsGameOver)
        {
            return;
        }

        if (!_gameTimer.IsEnabled)
        {
            // Start game
            _gameTimer.Start();
            _lastUpdate = DateTime.Now;
        }
        else if (_selectedPlayer != null && _selectedPlayer.HasBall)
        {
            // Kick ball
            Vector kickDirection = new Vector(1, 0);
            if (_selectedPlayer.Team == Team.Red)
            {
                kickDirection = new Vector(1, 0); // Kick right
            }
            else
            {
                kickDirection = new Vector(-1, 0); // Kick left
            }

            // Add vertical component based on movement
            if (_isWPressed) kickDirection.Y -= 0.5;
            if (_isSPressed) kickDirection.Y += 0.5;

            kickDirection.Normalize();
            _gameEngine.KickBall(_selectedPlayer, kickDirection, 12.0);
        }
        else
        {
            // Toggle pause
            _gameEngine.GameState.IsPaused = !_gameEngine.GameState.IsPaused;
        }
    }

    private void ResetGame()
    {
        _gameTimer.Stop();
        _gameEngine.ResetGame();
        _lastUpdate = DateTime.Now;
        UpdateUI();
        Render();
    }
}