# Futsal Game - Visual Guide

## Game Window Layout

```
┌─────────────────────────────────────────────────────────────────────────┐
│  RED TEAM  3          00:00           2  BLUE TEAM                      │
│                   Press SPACE to Start                                   │
├─────────────────────────────────────────────────────────────────────────┤
│ ┌───────────────────────────────────────────────────────────────────┐  │
│ │                                                                     │  │
│ │ ╔═══╗                                                        ╔═══╗ │  │
│ │ ║   ║                                                        ║   ║ │  │
│ │ ║GK ║        D         M         ●         M         D      ║GK ║ │  │
│ │ ║   ║                    F               F                  ║   ║ │  │
│ │ ║   ║         D                                      D      ║   ║ │  │
│ │ ╚═══╝                                                        ╚═══╝ │  │
│ │                                                                     │  │
│ └───────────────────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────────────────┤
│     Controls: WASD (Move) | SPACE (Kick/Pause) | R (Reset)             │
└─────────────────────────────────────────────────────────────────────────┘
```

## Field Layout

The game features a realistic futsal field with:

### Field Dimensions
- Width: 1000 pixels
- Height: 600 pixels
- Green background (#1e7c1e)

### Field Elements

1. **Center Circle**
   - Diameter: 120 pixels
   - Located at the center of the field
   - Center spot included

2. **Goals**
   - Width: 120 pixels
   - Depth: 20 pixels
   - Left goal (Red team defends): Red tinted
   - Right goal (Blue team defends): Blue tinted

3. **Penalty Areas**
   - Width: 180 pixels
   - Depth: 80 pixels
   - One on each end of the field

4. **Field Markings**
   - All lines in white with 70% opacity
   - Line thickness: 3 pixels
   - Center line dividing the field

## Player Representation

### Visual Design
Each player is rendered as a circle with:
- Radius: 15 pixels
- Radial gradient from white center to team color
- Black border (2px) or yellow border (3px) for controlled player
- Role indicator text (GK, D, M, F)

### Team Colors
- **Red Team**: RGB(220, 50, 50) - Bright red
- **Blue Team**: RGB(50, 50, 220) - Bright blue

### Player Roles
```
GK = Goalkeeper (stays near goal)
D  = Defender (guards defensive zone)
M  = Midfielder (controls center field)
F  = Forward (attacks opponent's goal)
```

### Team Formations

**Red Team (Left Side)**
```
            GK
        
    D            D
    
         M
         
         F
```

**Blue Team (Right Side)**
```
            GK
        
    D            D
    
         M
         
         F
```

## Ball Visualization

- Radius: 10 pixels
- White with gray gradient for 3D effect
- Black border (2 pixels)
- Follows physics (momentum, friction)
- Possession indicated by dashed circle around player

## UI Elements

### Score Display
```
┌─────────────────────────────────────────────┐
│  RED TEAM  3      00:00      2  BLUE TEAM   │
└─────────────────────────────────────────────┘
```

### Status Messages
- "Press SPACE to Start" - Initial state
- "Playing - Press SPACE to pause" - During game
- "PAUSED - Press SPACE to resume" - When paused
- "RED TEAM WINS! Press R to restart" - Game over (Red wins)
- "BLUE TEAM WINS! Press R to restart" - Game over (Blue wins)
- "DRAW! Press R to restart" - Game over (tie)

## Gameplay Flow

```
┌─────────────┐
│ Game Start  │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│ Press SPACE │──────► Game Begins
└──────┬──────┘        Timer Starts
       │
       ▼
┌─────────────┐
│   Playing   │◄────► Press SPACE to Pause
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  Time Up or │
│  Goal Scored│
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  Game Over  │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  Press R to │
│   Restart   │
└─────────────┘
```

## Color Scheme

### Main Colors
- **Background**: #2a2a2a (Dark gray)
- **Field**: #1e7c1e (Green)
- **UI Panels**: #1a1a1a (Very dark gray)
- **Borders**: #333333 (Medium gray)
- **Text**: White and #aaaaaa (Light gray)

### Team Colors
- **Red Team**: #ff4444 (Bright red) / #dc3232 (Player color)
- **Blue Team**: #4444ff (Bright blue) / #3232dc (Player color)

### Highlights
- **Selected Player**: Yellow border (RGB(255, 255, 0))
- **Ball**: White with gray gradient
- **Field Lines**: White with 70% opacity

## Special Effects

1. **Gradient Shading**: All players and ball use radial gradients for 3D effect
2. **Ball Possession**: Dashed circle indicator around player with ball
3. **Player Selection**: Yellow highlight border on controlled player
4. **Goal Zones**: Semi-transparent team-colored rectangles in goals
5. **Smooth Animation**: 60 FPS update rate for fluid motion

## Controls Reference

### Movement
```
     W
     ↑
A ← Player → D
     ↓
     S
```

### Actions
- **SPACE**: Multi-purpose key
  - Start game (when not running)
  - Kick ball (when in possession)
  - Pause/Resume (during gameplay)
- **R**: Reset entire game

### Control Tips
1. Move close to ball to automatically pick it up
2. Hold W or S while kicking to add vertical direction
3. Stamina depletes with movement and regenerates when idle
4. AI teammates will assist and position themselves

## Game Mechanics

### Ball Physics
- Momentum-based movement
- 2% friction per update
- Stops when velocity < 0.1
- Follows owner when possessed

### Player Physics
- 5% friction per update
- Maximum speed: 4.0 units
- Speed: 2.0 units
- Stamina: 0-100 (regenerates at 0.1/frame)

### Collision Detection
- Player-player collisions with separation
- Ball-player pickup at 20 pixel distance
- Field boundaries with clamping

### AI Behavior
- **Goalkeeper**: Stays near goal, chases when ball nearby
- **Defenders**: Guard defensive zone
- **Midfielders**: Control center, support attack/defense
- **Forwards**: Push forward, attempt shots
- **Decision Making**:
  - Shoot when near goal
  - Pass to better-positioned teammates
  - Chase ball if nearest
  - Return to position otherwise

## Performance

- **Frame Rate**: ~60 FPS (16ms update interval)
- **Resolution**: 1200x700 window
- **Field Rendering**: Static (drawn once)
- **Dynamic Elements**: Players and ball (redrawn each frame)
- **Optimization**: Old sprites removed before new rendering

## Building for Distribution

### Development Build
```bash
cd FutsalGame
dotnet run
```

### Release Build
```bash
cd FutsalGame
dotnet build -c Release
```

### Standalone Executable
```bash
cd FutsalGame
dotnet publish -c Release -r win-x64 --self-contained
```

Output: `FutsalGame/bin/Release/net10.0-windows/win-x64/publish/FutsalGame.exe`

### Single-File Executable
```bash
cd FutsalGame
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

This creates a single executable file that includes all dependencies.
