# 🏎️ iDrive — Racing Simulator

A fully functional **3D racing game** built in **Unity (C#)** featuring physics-based car control, an AI opponent, a lap-based checkpoint system, multiple camera views, and a real-time HUD — developed as a personal project to learn game development fundamentals.

---

## 🎮 Gameplay

- Race on a custom-built 3D track
- Compete against an **AI opponent** that follows the track via waypoint navigation
- Complete 5 laps as fast as possible
- Track your **current lap time** and **best lap time** live on screen

---

## ✨ Features

### 🚗 Physics-Based Car Controller
- **Rigidbody physics** — realistic acceleration, braking, and momentum
- Separate forward and reverse acceleration values
- Dual-raycast **ground detection** — car aligns to slope angle in real time
- Speed cap with velocity clamping
- Visual **front wheel rotation** matching steering input

### 🤖 AI Opponent
- Waypoint-based navigation through all checkpoints
- **Randomised path variance** — AI takes slightly different lines each lap
- Configurable acceleration and turn speed

### 🏁 Checkpoint & Lap System
- Sequential checkpoint validation — skipping checkpoints is not allowed
- Automatic **lap counter** (default: 5 laps)
- Lap time resets on each completed lap
- Best lap time tracked across the full race

### 📷 Multi-Camera System
- **Follow camera** — dynamically zooms out at higher speeds
- **Nose camera** — front-facing first-person-style view
- Press the assigned **Camera button** to cycle between views in-game

### 🔊 Audio System
- **Engine sound** with dynamic pitch scaling based on current speed
- **Tire squeal** audio triggered on sharp turns, fading out when straightened
- **Collision sound** on impacts with barriers or objects — with randomised pitch for variety

### 💨 Visual Effects
- **Dust trail particles** at all 4 wheels — triggered by turning or low-speed sliding

### 🖥️ HUD
- Live current lap time (`00m00.000` format)
- Best lap time across the session
- Lap counter (`current / total`)

---

## 🛠️ Tech Stack

| Technology | Usage |
|---|---|
| **Unity** | Game engine and scene editor |
| **C#** | All game logic and scripting |
| **Rigidbody physics** | Vehicle movement and collision |
| **Unity Particle System** | Dust trail effects |
| **Unity Audio** | Engine, tire, and collision sounds |
| **TextMeshPro** | HUD text rendering |

---

## 📁 Project Structure

```
iDrive/
└── Assets/
    ├── Scripts/
    │   ├── CarControl.cs       # Physics car controller + AI logic + lap timing
    │   ├── RaceManager.cs      # Singleton — manages checkpoints and race config
    │   ├── UIManager.cs        # Singleton — controls HUD text elements
    │   ├── Checkpoint.cs       # Checkpoint data (number tag)
    │   ├── CpChecker.cs        # Trigger detector — notifies car of checkpoint hits
    │   ├── CameraControl.cs    # Follow camera with speed-based distance
    │   ├── CameraSwitch.cs     # Cycles between multiple camera views
    │   ├── NoseCam.cs          # Front-facing nose camera
    │   └── PlaySoundOnHit.cs   # Collision audio with randomised pitch
    ├── Materials/              # Car materials (5 colour variants), grass, smoke
    ├── Prefabs/                # PlayerCar, CameraSwitch, Canvas prefabs
    └── Scenes/
        ├── Track1              # Main race track
        ├── SampleScene         # Development/test scene
        └── Test                # Secondary test scene
```

---

## 🚀 How to Run

**Requirements:** Unity 2020.3 or later (LTS recommended)

1. Clone the repository:
```bash
git clone https://github.com/DLaszlo2003/iDrive.git
```
2. Open Unity Hub → **Add project from disk** → select the `iDrive/` folder
3. Open the `Track1` scene from `Assets/Scenes/`
4. Press **Play** in the Unity Editor

### Controls

| Action | Key |
|---|---|
| Accelerate | `W` / `↑` |
| Reverse / Brake | `S` / `↓` |
| Steer Left | `A` / `←` |
| Steer Right | `D` / `→` |
| Switch Camera | Assigned Camera button |

---

## 🧠 Architecture

The project uses the **Singleton pattern** for the two manager classes (`RaceManager`, `UIManager`), giving any script in the scene direct access to race state and HUD without requiring direct object references.

Car control and AI are handled by a single `CarControl` component with an `isAI` toggle — making it easy to configure human-controlled and AI-controlled cars from the same codebase.

---

## 👤 Author

**Dezső László** — [LinkedIn](https://www.linkedin.com/in/lászló-dezső-39a676255/) · [GitHub](https://github.com/dezsolaszlo)
