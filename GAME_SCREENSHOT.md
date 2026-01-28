# Futsal Game - Visual Representation

## Game Window Screenshot

```
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃                                    FUTSAL GAME - 5v5                                        ┃
┣━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┫
┃                                                                                             ┃
┃         RED TEAM  2              05:42              3  BLUE TEAM                            ┃
┃                             Playing - Press SPACE to pause                                  ┃
┃                                                                                             ┃
┣━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┫
┃  ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓  ┃
┃  ┃                           FUTSAL FIELD (Green)                                     ┃  ┃
┃  ┃ ╔═╗                                                                          ╔═╗   ┃  ┃
┃  ┃ ║ ║                                                                          ║ ║   ┃  ┃
┃  ┃ ║ ║              D                                        D                  ║ ║   ┃  ┃
┃  ┃ ║G║                                                                          ║G║   ┃  ┃
┃  ┃ ║K║                      M         ●         M                              ║K║   ┃  ┃
┃  ┃ ║ ║                                 │                                        ║ ║   ┃  ┃
┃  ┃ ║ ║              D       ◉F────────┘         D                              ║ ║   ┃  ┃
┃  ┃ ║ ║                                                                          ║ ║   ┃  ┃
┃  ┃ ╚═╝                                                                          ╚═╝   ┃  ┃
┃  ┃                                    │                                               ┃  ┃
┃  ┃────────────────────────────────────┼───────────────────────────────────────────── ┃  ┃
┃  ┃                                    │                                               ┃  ┃
┃  ┃                                   ( )                                              ┃  ┃
┃  ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛  ┃
┣━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┫
┃              Controls: WASD (Move) | SPACE (Kick/Pause) | R (Reset)                        ┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛

Legend:
  GK = Goalkeeper
  D  = Defender  (Red or Blue)
  M  = Midfielder (Red or Blue)
  ◉F = Forward with ball (Yellow highlight - YOU control this player!)
  ●  = Ball
  ╔═╗ = Goals
  ( ) = Center circle

RED TEAM (Left):     ● Red circles
BLUE TEAM (Right):   ● Blue circles
```

## Game Features Visualization

### 1. High-Quality Graphics
The actual WPF application renders:
- **Gradient-filled players** with 3D effect (radial gradient from white center to team color)
- **Smooth shadows** and highlights on the ball
- **Green field** (#1e7c1e) with white field markings
- **Goal areas** with semi-transparent team colors
- **Yellow highlight** on the player you control
- **Dashed circle** around player with ball possession

### 2. Player Layout (Starting Positions)

**RED TEAM (Attacking Right →)**
```
                   GK
                   |
        D                  D
                   |
                   M
                   |
                   F
```

**BLUE TEAM (Attacking Left ←)**
```
                   F
                   |
                   M
                   |
        D                  D
                   |
                   GK
```

### 3. Gameplay Flow

```
┌────────────────┐
│  Press SPACE   │
│   to Start     │
└────────┬───────┘
         ↓
┌────────────────┐      ┌─────────────┐
│   Playing      │◄────→│   Paused    │
│   (Moving,     │      │  (Frozen)   │
│   Kicking)     │      │             │
└────────┬───────┘      └─────────────┘
         ↓
┌────────────────┐
│  Score/Time    │
│   Updates      │
└────────┬───────┘
         ↓
┌────────────────┐
│   Game Over    │
│  (10 minutes)  │
└────────┬───────┘
         ↓
┌────────────────┐
│  Press R to    │
│    Restart     │
└────────────────┘
```

### 4. Controls Demonstration

```
Movement (WASD):
       W (Up)
        ↑
A (Left)←●→D (Right)
        ↓
      S (Down)

Ball Control:
- Move near ball → Automatic pickup
- SPACE → Kick towards goal
- Hold W/S while kicking → Add vertical angle
```

### 5. Visual Effects

**Player Rendering:**
```
     ╭───╮
    ╱ • • ╲    ← Radial gradient (white → team color → darker)
   │  ▼▼  │   ← Role indicator (GK, D, M, F)
    ╲    ╱
     ╰───╯
  Black border (2px)
  or Yellow border (3px) if selected
```

**Ball Rendering:**
```
    ╱─╲
   │ ● │     ← White with gradient to gray
    ╲─╱
  Black border (2px)
```

**Ball Possession Indicator:**
```
    ╱─ ─ ─╲
   │ ╱─╲  │  ← Dashed white circle
   │ │●│  │  ← Pulsing animation
    ╲╰─╯ ╱
     ─ ─
```

### 6. Score Display

```
╔═══════════════════════════════════════════╗
║  RED TEAM  3      05:42      2  BLUE TEAM ║
║         Playing - Press SPACE to pause    ║
╚═══════════════════════════════════════════╝
```

- **Large numbers** (32px font) for scores
- **Timer** in MM:SS format
- **Status text** updates based on game state

### 7. Field Details

**Dimensions:**
- Width: 1000 pixels
- Height: 600 pixels
- Goal size: 120 × 20 pixels
- Penalty box: 180 × 80 pixels

**Markings:**
- Center line (vertical)
- Center circle (120px diameter)
- Two goals (left & right)
- Two penalty boxes
- All lines: White with 70% opacity

## Technical Implementation

The game uses:
- **WPF Canvas** for rendering
- **DispatcherTimer** at 16ms interval (~60 FPS)
- **Ellipse shapes** for players and ball
- **RadialGradientBrush** for 3D effects
- **Vector math** for physics
- **Delta time** for smooth movement
- **Collision detection** for player-player and player-ball interactions

## How to Experience the Real Thing

Since this is a visual representation, to see the actual game with full graphics:

1. **On Windows:**
   ```bash
   cd FutsalGame
   dotnet run
   ```

2. **Build executable:**
   ```bash
   cd FutsalGame
   dotnet publish -c Release -r win-x64 --self-contained
   ```
   Then run: `FutsalGame/bin/Release/net10.0-windows/win-x64/publish/FutsalGame.exe`

The real application features smooth animations, physics-based movement, and high-quality rendering that can't be captured in ASCII art!
