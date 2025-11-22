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

- `BeaconRotator.cs` — Fan rotation controller  
- `WorldParams.cs` — Global environment variable store  
- `LightingController.cs` — Light + specular parameter control  
- `SimplePickup.cs` — Pickup + throw interaction  
- `Destructible.cs` — Fragment spawn on impact  
- `StatusDisplay.cs` — UI updates  

All scripts are clean, commented, and optimized for readability.

---

## 🧪 **Testing Checklist**

- [ ] Player can move using WASD  
- [ ] Fan speed slider updates rotation  
- [ ] Light slider changes intensity  
- [ ] Wind slider affects hanging plant sway  
- [ ] Pickup + throw pot works reliably  
- [ ] Pot breaks correctly on collision  
- [ ] Storm toggle activates particles + audio + flicker  
- [ ] Status panel values update live  
- [ ] Build runs at ≥30 FPS on GTX 1650

---

## 🔧 **How to Run**

1. Download the Windows build (`Conservatory_win.zip`)  
2. Extract the folder  
3. Run `CaretakersConservatory.exe`  
4. Ensure the _Data_ folder is next to the executable  
5. Optional: open Unity project for XR simulation tests

---

## ⭐ **Known Limitations (For Graders)**
- Plant sway is shader-based (no Unity Cloth)  
- No custom HLSL—Shader Graph used for speed  
- Limited asset diversity (time-optimized)  
- Fragment physics is deterministic but not fully network-safe  
- Storm particles optimized for low GPU usage (≤50 particles)

---

## 📜 **License**
This project is for academic evaluation only.  
Feel free to use the code for learning or reference.

---

## 🙌 **Acknowledgements**
- Unity Learn — URP + Shader Graph  
- XR Interaction Toolkit Samples  
- Free textures inspired by PolyHaven  

---
