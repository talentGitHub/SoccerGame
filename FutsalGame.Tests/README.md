# Futsal Game Tests

This directory contains unit tests for the Futsal Game application.

## Test Coverage

The test suite covers the following components:

### PlayerTests
- Player initialization with correct properties
- Player movement based on velocity
- Friction application to velocity
- Player movement commands (MoveTo)
- Speed limiting (max speed enforcement)
- Stamina regeneration

### BallTests
- Ball initialization
- Ball movement when free
- Ball following owner (possession)
- Ball kicking mechanics
- Friction application
- Ball stopping when slow enough
- Owner management

### GameStateTests
- Game state initialization
- Score tracking for both teams
- Game time updates
- Game over detection when time expires
- Pause functionality
- Time formatting
- Game reset functionality

### GameEngineTests
- Player and team creation (5v5)
- Player role distribution
- Game update loop
- Ball physics updates
- Game state updates
- Ball kicking mechanics
- Position resets after goals
- Game reset functionality
- Player distance calculations

## Running Tests

### On Windows
```bash
cd FutsalGame.Tests
dotnet test
```

### On Linux/Mac
The tests require the Windows Desktop framework to run since the game is a WPF application. However, the tests can be built and validated for correctness:

```bash
cd FutsalGame.Tests
dotnet build
```

To actually run the tests, you need to be on Windows or use a Windows VM.

## Test Framework

- **xUnit**: Modern, extensible testing framework
- **Coverage**: Core game logic and physics
- **Pattern**: Arrange-Act-Assert

## Adding New Tests

When adding new features to the game:

1. Create a new test file in `FutsalGame.Tests/`
2. Name it `<ComponentName>Tests.cs`
3. Follow the existing test patterns
4. Run tests on Windows to verify functionality

## Notes

- Tests validate game logic independently of the UI
- Physics calculations are tested for correctness
- AI behavior is not extensively tested (deterministic behavior can be complex)
- UI rendering is not unit tested (WPF integration testing would be separate)
