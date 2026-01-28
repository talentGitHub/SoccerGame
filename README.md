# Futsal Game - 5v5 Windows Application

A Windows desktop application for playing futsal (indoor soccer) with 5 players versus 5 players. Built with C# and WPF, featuring high-quality graphics, physics-based gameplay, and AI-controlled opponents.

## Features

- **5v5 Gameplay**: Play as the Red Team forward against an AI-controlled Blue Team
- **Realistic Physics**: Ball physics with momentum, friction, and collision detection
- **AI Opponents**: Smart AI that positions, passes, and shoots strategically
- **Beautiful Graphics**: 
  - Detailed futsal field with proper markings
  - Gradient-based player and ball rendering
  - Team-colored players (Red vs Blue)
  - Visual indicators for ball possession
- **Game Features**:
  - Score tracking for both teams
  - Game timer (10-minute matches)
  - Pause/Resume functionality
  - Game reset option
  - Role-based player positioning (Goalkeeper, Defender, Midfielder, Forward)

## System Requirements

- **Operating System**: Windows 10 or later
- **.NET SDK**: .NET 10.0 or later
- **Graphics**: Any modern graphics card with DirectX support

## Installation

### Prerequisites
1. Install [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

### Building the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/talentGitHub/SoccerGame.git
   cd SoccerGame
   ```

2. Navigate to the project directory:
   ```bash
   cd FutsalGame
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

### Creating a Standalone Executable

To create a standalone Windows executable:

```bash
cd FutsalGame
dotnet publish -c Release -r win-x64 --self-contained
```

The executable will be created in: `FutsalGame/bin/Release/net10.0-windows/win-x64/publish/FutsalGame.exe`

## Controls

### Keyboard Controls
- **W, A, S, D**: Move your player (Red Team Forward)
- **SPACE**: Start the game / Pause/Resume
- **R**: Reset the game

### Button Controls
- **PASS Button**: Pass the ball to the nearest teammate
- **KICK Button**: Shoot the ball towards the goal (full power)

## Gameplay

1. **Starting the Game**: Press SPACE to start the match
2. **Movement**: Use WASD keys to move your player around the field
3. **Ball Control**: Move close to the ball to pick it up automatically
4. **Passing**: Click the green PASS button or use it to pass to your nearest teammate
5. **Shooting**: Click the orange KICK button to shoot at goal with full power
4. **Kicking**: Press SPACE while holding the ball to kick towards the opponent's goal
   - Add vertical direction by holding W or S while kicking
   - Or use the PASS button to pass to teammates
   - Or use the KICK button for a powerful shot at goal
5. **Scoring**: Get the ball into the opponent's goal (blue goal on the right)
6. **Winning**: The team with the most goals when time expires wins!

## Game Architecture

### Project Structure
```
FutsalGame/
├── Models/
│   ├── Player.cs       - Player entity with physics and state
│   ├── Ball.cs         - Ball entity with physics
│   └── GameState.cs    - Game state management
├── Engine/
│   ├── GameEngine.cs   - Core game logic and physics
│   └── AIController.cs - AI behavior for computer players
├── MainWindow.xaml     - UI layout
└── MainWindow.xaml.cs  - Game rendering and input handling
```

### Key Components

- **GameEngine**: Manages game state, player positions, ball physics, collision detection, and scoring
- **AIController**: Controls AI players with role-based positioning and decision-making
- **Player**: Represents each player with position, velocity, stamina, and role
- **Ball**: Handles ball physics, ownership, and movement

## Graphics Features

- **High-Quality Field Rendering**: Realistic futsal field with:
  - Center circle and line
  - Penalty areas
  - Goal boxes with team colors
  - White field markings
  
- **Player Visualization**:
  - Radial gradient shading for 3D effect
  - Team colors (Red vs Blue)
  - Role indicators (GK, D, M, F)
  - Ball possession indicator
  - Yellow highlight for controlled player

- **Ball Rendering**:
  - 3D gradient effect
  - Realistic shadow and highlights

## Development

### Technologies Used
- **Language**: C# 12
- **Framework**: .NET 10.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Graphics**: WPF Canvas with Shape rendering

### Extending the Game

You can extend the game by:
- Adding more teams or tournaments
- Implementing multiplayer support
- Adding different difficulty levels for AI
- Including power-ups or special abilities
- Adding sound effects and music
- Implementing replays
- Adding player statistics tracking

## License

This project is open source and available for educational purposes.

## Credits

Developed as a demonstration of game development with C# and WPF.