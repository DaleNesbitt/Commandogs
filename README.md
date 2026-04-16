# Commandogs

Commandogs is a two-player cooperative stealth and puzzle game built in Unity. The game uses asymmetric gameplay, where one player operates inside a 3D environment while the other supports them remotely through surveillance systems and environmental controls.

---

## Overview

In Commandogs, players take on two distinct roles:

- Player 1 controls Rua, a dog navigating a hostile 3D environment.
- Player 2 controls Finn, a remote operator who guides Rua using CCTV-style camera feeds and interacts with environmental systems.

The game is built around communication, planning, and coordination. Players must work together during planning phases, then survive periods where communication is restricted during gameplay.

---

## Core Design Goals

- Asymmetric cooperative gameplay between two distinct roles  
- Communication-driven puzzle solving  
- Stealth-focused traversal and avoidance mechanics  
- Structured gameplay loop of planning followed by execution  
- Strong reliance on trust and coordination between players  

---

## Narrative

The game is set in a world controlled by Dr. Vincent Vile, a former animal behaviour scientist who developed neural control technology intended to enforce obedience in animals. His work later evolved into a system capable of controlling humans through neural collars.

Finn Keane, a former military K9 commander, opposes Vile’s ideology and represents a philosophy based on trust and training rather than control. After being injured in the field, Finn can no longer operate physically in the environment. Instead, he supports Rua, his trained German Shepherd, from a remote command station.

Rua is sent into the Vile Vault, a heavily secured facility where Vile’s system is being developed and deployed. Finn guides Rua through surveillance systems to infiltrate the facility, rescue Finn’s captured brother, and destroy the Neuro-Collar Network.

The narrative focuses on the conflict between control and trust, and the relationship between the two protagonists as they attempt to complete their mission under extreme constraints.

---

## Gameplay Structure

### Player 1 – Rua
- Third-person 3D movement-based gameplay  
- Stealth navigation through hostile environments  
- Puzzle interaction and environmental traversal  
- No combat focus, emphasis on avoidance and timing  
- Dependent on instructions from Player 2  

### Player 2 – Finn
- Surveillance-based control interface  
- Multiple camera feeds across the environment  
- Environmental interaction and puzzle solving  
- Ability to switch between cameras to maintain visibility  
- Provides navigation instructions to Player 1  

---

## Communication System

Communication is a core mechanic of the game.

- Players can communicate freely in staging areas
- Once Player 1 leaves the staging area, communication is disabled
- Communication is restored at designated checkpoints

This creates a cycle of planning, execution, and reconnection, requiring players to rely on memory and preparation.

---

## Enemy Design

Enemies use a combination of NavMesh navigation and vision-based detection.

- Patrol behaviour based on predefined paths  
- Detection system using raycasting to simulate a cone of vision  
- Line of sight required for detection  
- Environmental objects can block detection  

When detected, enemies can:
- Enter an alert state  
- Increase patrol intensity  
- Trigger level resets or penalties depending on scenario design  

---

## Puzzle Design

Puzzles are designed around cooperation between both players.

Examples include:
- Remote door activation systems  
- Trap deactivation through surveillance control  
- Timing-based navigation challenges  
- Multi-step environmental manipulation tasks  

The difficulty increases as players progress through levels, with more complex layouts and tighter coordination requirements.

---

## Level Flow

Each level is structured into distinct phases:

1. Staging Area  
   - Players communicate freely  
   - Strategy and planning take place  

2. Traversal Phase  
   - Communication is disabled  
   - Player 1 navigates the environment  
   - Player 2 provides limited environmental support  

3. Checkpoint  
   - Players reconnect  
   - Strategy is reassessed  

---

## Technical Design

The project is built in Unity and uses several core systems:

- NavMesh AI for enemy movement  
- Raycasting system for line-of-sight detection  
- Event-driven architecture for gameplay interactions  
- Multi-camera system for Player 2 surveillance interface  
- Cinemachine planned for camera transitions and control  

The architecture is designed to remain modular to support future expansion and refinement.

---

## Networking (Planned)

The game is intended to support online cooperative play, where players are physically separated but connected through networked gameplay.

If networking is not implemented, the game can also function using a dual-screen local setup.

---

## Development Challenges

Several technical risks have been identified:

- NavMesh stability in complex environments  
- Performance cost of multiple raycasts per enemy  
- Camera management and smooth switching between feeds  
- Synchronisation between player roles in multiplayer scenarios  
- Event timing and state management complexity  
- Performance optimisation due to lighting, AI, and camera systems  

Planned solutions include object pooling, baked lighting, and careful event system design.

---

## Art Direction

The game uses a semi-stylised visual approach. It is not fully realistic and not fully low-poly.

Key assets include:
- Rua: Animated German Shepherd character  
- Finn: First-person surveillance interface (no full character model required)  
- Dr. Vile: Custom antagonist model  
- Environment: Futuristic facility known as the Vile Vault  

Most assets are sourced from the Unity Asset Store, with custom models created where necessary.

---

## Audio Design

Audio is designed to support atmosphere and gameplay clarity.

- Environmental ambient audio  
- Sound effects for interactions and alerts  
- Optional background music  
- Custom sound design where possible using studio resources  

---

## Development Roles

All development work is carried out by a single developer:

- Programming  
- Game design  
- Level design  
- Art and asset integration  
- Quality assurance  
- Production  

---

## Tools and Technology

- Unity Engine  
- Git with Git LFS  
- Visual Studio  
- Blender for modelling  
- Cinemachine (planned integration)  

---

## Project Status

The project is currently in active development. Core systems under construction include:

- Player control systems  
- AI navigation and detection  
- Camera switching system  
- Puzzle framework and event system  

---

## License

This project is intended for educational use. All rights reserved unless otherwise specified
