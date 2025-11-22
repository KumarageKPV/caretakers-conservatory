# 🌱 Caretaker’s Conservatory — Interactive Storyworld

**Caretaker’s Conservatory** is a compact, interactive story-world environment.
This MVP demonstrates minimum-viable interaction design, dynamic environmental systems, shader-driven animation, and XR-ready controls—all optimized for desktop play and VR grading using the **XR Device Simulator** (no headset required).

---

## 🚀 **Project Overview**

This repository implements **The Caretaker’s Conservatory**, a small interactive environment featuring:

- A fully modeled conservatory space (terrain + structure)
- A controllable ventilation fan (rigid-body animation)
- A shader-animated hanging plant (wind-responsive vertex offset)
- Material parameter controls (diffuse light, specular strength, wind strength)
- Physics interaction: pickup, throw, and destructible clay pot
- Storm mode (particles + audio + flickering lights)
- World-space status display with live numerical values
- Desktop first-person controls + XR Device Simulator for VR testing

The entire experience is built for Windows Standalone (x86_64), Unity 2022 LTS, and URP.

---

## 🛠 **Tech Stack**

| Category | Tools / Technologies |
|---------|----------------------|
| Engine | Unity **2022 LTS** (URP Template) |
| XR | OpenXR, XR Interaction Toolkit, XR Device Simulator |
| Modeling | Blender 4.x → FBX pipeline |
| Scripting | C# (Unity / VS Code) |
| Shaders | Shader Graph (URP) |
| Audio | Unity Audio Source + SFX |
| Build Target | Windows Desktop (.exe) |

---

## 🎯 **Core MVP Features**

### **Environment**
- Terrain sculpted inside Blender  
- Small conservatory building + static colliders  
- Imported as FBX (Y-up, -Z forward)

### **Interactions**
- **Fan control**: rotation speed connected to UI slider  
- **Light control**: diffuse intensity + specular level  
- **Wind control**: global float used in plant shader  
- **Storm toggle**:  
  - Particle emission boost  
  - Ambient wind SFX  
  - Slight light flicker

### **Physics**
- Pickup system (distance-based)  
- Throw mechanic (velocity-based)  
- Destructible object: pot breaks into fragments prefab  
- Optional particles + SFX on break

### **UI**
- World-space sliders + toggle  
- Status panel showing:  
  - Fan speed  
  - Light intensity  
  - Wind strength

### **Shader Feature**
- Vertex offset using SimpleNoise × windStrength  
- Cheap, fast alternative to Cloth simulation

---

## 🎮 **Controls**

### **Desktop Mode**
| Action | Key |
|--------|-----|
| Move | WASD |
| Look | Mouse |
| Interact / Pick up / Throw | Left Mouse |
| Toggle Storm | UI Toggle |
| Adjust Parameters | On-screen sliders |

### **XR Device Simulator (No Headset Needed)**
Provided via **Window → Analysis → Device Simulator**.

Controls:
- WASD to move  
- Mouse to aim  
- Tab to toggle device window  
- Left/Right click for XR interactions

---

## 📁 **Folder Structure**

```
/Assets
  /Models
  /Materials
  /Scripts
  /Shaders
  /Textures
  /Prefabs
  /Scenes (Main.unity)
  /UI
/Builds
/Docs
/Video
```

---

## 🧩 **Key Scripts (Included in /Assets/Scripts)**

**Core Data Model:**
- `WorldParams.cs` — ScriptableObject-based global environment state with C# events for parameter changes

**Controllers (Event-Driven):**
- `FanController.cs` — Subscribes to FanSpeed events, rotates fan blades transform
- `LightingController.cs` — Subscribes to LightIntensity events, adjusts Light component
- `WindController.cs` — Subscribes to WindStrength events, sets Shader global float for plant animation
- `StormController.cs` — Subscribes to StormActive events, manages particles, audio, light flicker

**Interaction:**
- `PlayerController.cs` — First-person movement (Input System), WASD + mouse look, raycast pickup/throw
- `PickupInteractable.cs` — Attach to objects; handles pickup, hold position, throw with velocity
- `DestructiblePot.cs` — OnCollisionEnter impulse threshold check, spawns fragment prefab, plays SFX

**UI:**
- `UISliderController.cs` — Binds UI sliders/toggles to WorldParams properties, propagates user input
- `StatusDisplay.cs` — Subscribes to WorldParams events, updates Text UI elements with live values

**Testing:**
- `WorldParamsTests.cs` — NUnit EditMode tests validating event firing and parameter synchronization

All scripts use event-driven architecture to minimize polling and improve performance. Shader Graph assets and scene assembly remain manual steps in Unity Editor.

---

## 🧪 **Testing Checklist**

EditMode tests added for `WorldParams` event firing (see `Assets/Tests/EditMode/WorldParamsTests.cs`). PlayMode tests pending once scene and prefabs are added.

- [ ] Player can move using WASD  
- [ ] Fan speed slider updates rotation  
- [ ] Light slider changes intensity  
- [ ] Wind slider affects hanging plant sway  
- [ ] Pickup + throw pot works reliably  
- [ ] Pot breaks correctly on collision  
- [ ] Storm toggle activates particles + audio + flicker  
- [ ] Status panel values update live  
- [ ] Build runs at ≥30 FPS on GTX 1650  
- [ ] Shader Graph plant sway responds to wind parameter  
- [ ] XR Device Simulator interactions map correctly

---

## 🔧 **How to Run**

### Option 1: Play Pre-built Executable (Not Yet Available)
1. Download the Windows build (`Conservatory_win.zip`)  
2. Extract the folder  
3. Run `CaretakersConservatory.exe`  
4. Ensure the _Data_ folder is next to the executable

### Option 2: Open in Unity Editor (Development)

**Requirements:**
- Unity 2022.3.17f1 or later (2022 LTS)
- Windows 10/11 with DirectX 11+ GPU
- 8GB RAM minimum (16GB recommended)

**Setup Steps:**

1. **Clone the repository:**
   ```powershell
   git clone https://github.com/KumarageKPV/caretakers-conservatory.git
   cd caretakers-conservatory
   ```

2. **Open in Unity Hub:**
   - Launch Unity Hub
   - Click "Add" → "Add project from disk"
   - Navigate to the cloned folder and select it
   - Unity will import packages (this may take 5-10 minutes on first load)

3. **Verify Package Installation:**
   - Open `Window → Package Manager`
   - Ensure the following are installed:
     - Universal RP (14.0.10+)
     - XR Plugin Management (4.4.0+)
     - OpenXR Plugin (1.9.1+)
     - XR Interaction Toolkit (2.5.4+)
     - Input System (1.7.0+)
     - Test Framework (1.1.34+)

4. **Create a Scene (Currently Manual):**
   - The project structure is scaffolded but scenes must be manually created
   - Create `Assets/Scenes/Main.unity`
   - Add essential GameObjects:
     - **Player**: CharacterController + PlayerController script + Camera child
     - **WorldParams**: Create ScriptableObject via `Assets → Create → CaretakersConservatory → World Params`
     - **UI Canvas**: World space, add sliders/toggle + UISliderController + StatusDisplay
     - **Environment**: Terrain, conservatory model, lighting
     - **Interactive Objects**: Fan (with FanController), Plant (with shader), Pot (with DestructiblePot)
     - **Controllers**: Empty GameObjects with LightingController, WindController, StormController

5. **Run in Editor:**
   - Press Play in Unity Editor
   - Use WASD to move, mouse to look
   - Interact with UI sliders to control parameters

6. **Test XR Device Simulator:**
   - Open `Window → Analysis → XR Device Simulator`
   - Toggle device mode to test XR interactions without headset

---

## ⭐ **Known Limitations (For Graders)**
- Plant sway is shader-based (no Unity Cloth)  
- No custom HLSL—Shader Graph used for speed  
- Limited asset diversity (time-optimized)  
- Fragment physics is deterministic but not fully network-safe  
- Storm particles optimized for low GPU usage (≤50 particles)

**Current Implementation Status (Nov 2025):**
- ✅ Core C# scripts scaffolded with event-driven architecture
- ✅ Unity project structure and package dependencies configured
- ✅ EditMode tests for `WorldParams` parameter propagation
- ⏳ Scene assembly (Main.unity) requires manual setup in Unity Editor
- ⏳ Shader Graph asset for plant wind animation (must create in Editor)
- ⏳ 3D models (conservatory, fan, pot) and prefabs (fragments, UI) pending import
- ⏳ Audio assets and particle systems require Editor configuration
- ⏳ Build pipeline and performance profiling deferred until scene complete

---

## 📝 **Implementation Notes**

This repository now contains a **fully scaffolded Unity 2022.3 LTS project** ready to open in Unity Editor. The following components are implemented:

**Completed:**
1. **Project Structure**: `Assets/`, `Packages/`, `ProjectSettings/` with manifest, version, quality, physics, input, tag configurations
2. **Assembly Definitions**: Modular compilation for main scripts and tests
3. **Core Scripts** (9 files):
   - Event-driven parameter management (`WorldParams.cs`)
   - Scene controllers subscribing to parameter events (Fan, Lighting, Wind, Storm)
   - Player interaction system (pickup, throw, first-person movement via Input System)
   - UI bindings (sliders → WorldParams, status display ← WorldParams events)
4. **Unit Tests**: NUnit EditMode tests for `WorldParams` event behavior
5. **Documentation**: Expanded README with setup instructions, script descriptions, testing checklist

**Next Manual Steps in Unity Editor:**
1. Create `Assets/Scenes/Main.unity` and assemble GameObject hierarchy
2. Create WorldParams ScriptableObject asset instance
3. Build Shader Graph for plant vertex animation (SimpleNoise × `_GlobalWindStrength`)
4. Import or model conservatory, fan, pot meshes; configure materials
5. Set up UI Canvas with sliders, toggle, and status text elements
6. Configure particle systems for storm effects
7. Add audio clips and AudioSource components
8. Create fragment prefab for destructible pot
9. Test in-editor and profile for ≥30 FPS target
10. Build Windows Standalone executable

**Why This Approach:**
Unity scene files (`.unity`) and assets (`.asset`, `.shadergraph`, `.prefab`) are binary/YAML formats best created directly in Unity Editor to ensure proper GUID references and serialization. The C# scripts, assembly definitions, and project settings provided here establish the **functional code layer**, while the **content layer** (models, materials, scenes) requires visual authoring tools.

---

## 📜 **License**
The repository is currently licensed under MIT (see `LICENSE`). The README previously stated "academic evaluation only"; MIT permits broader use (commercial, modification, distribution). If restriction to academic use is desired, consider adopting a different license (e.g., CC BY-NC-SA) and update this section accordingly. Until changed, MIT terms apply.

---

## 🙌 **Acknowledgements**
- Unity Learn — URP + Shader Graph  
- XR Interaction Toolkit Samples  
- Free textures inspired by PolyHaven  

---
