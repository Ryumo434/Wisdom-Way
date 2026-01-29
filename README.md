# Wisdom Way

![Gameplay Screenshot](Images/WisWayMenu.png)

Wisdom Way is a 2D top-down pixel art game developed in Unity, focusing on modular game systems, enemy AI, and puzzle-based progression.

Your mission is to defeat the wizard at the top of the tower.
The tower consists of five layers, each representing a different area of knowledge.
On your journey to the top, you will face monsters, puzzles, and challenges.

This game was developed as part of the Software Engineering course at HWR Berlin.

![Gameplay Demo](Images/WisWayVideo.gif)

## Setup & Run

1. Install Unity Hub
2. Install Unity **2022.3.33f1 LTS**
3. Clone the repository
```bash
git clone https://github.com/Ryumo434/Wisdom-Way.git
```
4. Open the project via Unity Hub
5. Open the `MainMenu` scene
6. Press Play


## Features

- **Player Movement & Combat**
  - Smooth top-down movement with running and dash mechanics
  - Melee combat system with attack animations and hit detection

- **Enemy Encounters**
  - Multiple enemy types with distinct behaviors
  - State-based enemy logic (idle, patrol, chase, attack)
  - Combat encounters integrated into level progression

- **Collectable Items**
  - Collectable objects that influence gameplay and progression
  - Item-based interaction with puzzles and the environment

- **Puzzle & Quiz Mechanics**
  - Logic-based puzzles and quiz challenges
  - Puzzles are required to unlock new areas and advance floors
  - Integration of NPCs as puzzle and quest triggers

- **Tower Progression**
  - Five unique tower layers, each representing a distinct knowledge domain
  - Increasing difficulty through enemy density and puzzle complexity

- **Save & Exit System**
  - Manual save functionality with persistent game state
  - Player progress, collected items, and completed puzzles are stored
  - Safe exit and resume gameplay from the last saved state


## Used Technologies

- **Unity (2022.3 LTS)** – Game engine, scene management, animation system
- **C#** – Gameplay programming, game logic, AI behavior, save systems
- **Unity Input System** – Player movement, dash, and combat controls
- **Scriptable Objects** – Data-driven design for items and gameplay values
- **Git & GitHub** – Version control, team collaboration, branching workflow
- **Visual Studio Code** – Development environment and debugging


## Contributors

- **Nasser** – Character implementation and enemy behavior
- **Lukas** – Quiz systems and puzzle mechanics
- **Mihoshi** – Save & load system, NPC logic, and puzzle mechanics
- **Sinan** – Level and map design

