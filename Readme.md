# 🐍 Saanp Seedhi - Snake & Ladder Game 🪜

![Language](https://img.shields.io/badge/Language-C%23-blue.svg)
![Platform](https://img.shields.io/badge/Platform-Console%20%2F%20Terminal-green.svg)
![License](https://img.shields.io/badge/License-MIT-purple.svg)

> A classic, feature-rich Indian Board Game (**Saanp Seedhi**) implemented in C# for the Windows Command Line / Terminal with dynamic ASCII graphics, multi-player support, and AI opponents.

---

## 🌟 Highlights & Features

- 🎮 **2 to 4 Players**: Play with friends or family locally.
- 🤖 **Human vs Computer (AI)**: Option to set any player as an automated Computer opponent.
- 🎲 **Interactive Dice Rolling**: Visual dice animations with standard 1-6 rolling mechanics.
- 🎨 **Rich Terminal Interface**: Dynamic 100-cell ASCII board with colored player tokens, snakes, and ladders.
- 📊 **Post-Game Statistics**: Tracks snake bites, ladders climbed, total rolls, and 6s rolled for each player.
- 🐍 **Classic Rules**:
  - Reach exactly **100** to win.
  - Climb ladders to jump ahead.
  - Watch out for snake heads—they slide you back down!
  - Roll a **6** to get a bonus turn (max 3 consecutive 6s allowed).

---

## 🕹️ Game Rules & Positions

### 🐍 Snakes (Head ➔ Tail)
| Head | Tail | Head | Tail | Head | Tail |
| :---: | :---: | :---: | :---: | :---: | :---: |
| **99** | 12 | **95** | 36 | **92** | 51 |
| **83** | 42 | **73** | 1 | **62** | 18 |
| **54** | 34 | **46** | 5 | **38** | 20 |
| **27** | 7 | **16** | 4 | | |

### 🪜 Ladders (Bottom ➔ Top)
| Bottom | Top | Bottom | Top | Bottom | Top |
| :---: | :---: | :---: | :---: | :---: | :---: |
| **2** | 23 | **8** | 29 | **14** | 77 |
| **21** | 56 | **28** | 44 | **36** | 57 |
| **43** | 76 | **50** | 69 | **61** | 80 |
| **71** | 90 | **78** | 98 | | |

---

## 🛠️ How to Run & Play

### Option 1: Direct Execution (Windows)
If `SnakeLadder.exe` is already compiled, simply run it in your terminal:
```bash
./SnakeLadder.exe
```

### Option 2: Compile & Run via C# Compiler / .NET CLI

Using **csc** (C# Command-line compiler):
```bash
csc SnakeLadder.cs
./SnakeLadder.exe
```

Using **.NET CLI**:
```bash
dotnet run
```

---

## 📜 Game Controls

- **Press `ENTER`**: Roll the dice when prompted.
- Computer players roll automatically with smooth turn pauses.

---

## 📄 License

Distributed under the [MIT License](LICENSE). Feel free to modify and share!
