# Scene Setup Guide - Main.unity

This guide walks through assembling the complete scene in Unity Editor after opening the project.

## Prerequisites
- Unity 2022.3.17f1 (or later 2022 LTS) installed
- Project opened in Unity Hub with all packages imported
- Familiarity with Unity Editor basics (GameObject hierarchy, Inspector, Scene view)

---

## Step 1: Create Main Scene

1. In Unity Editor, navigate to `Assets/Scenes/`
2. Right-click → `Create → Scene`
3. Name it `Main.unity`
4. Double-click to open the scene
5. Delete default Main Camera and Directional Light (we'll create custom ones)

---

## Step 2: Create WorldParams ScriptableObject

This is the central data hub for all environment parameters.

1. In Project window, navigate to `Assets/`
2. Right-click → `Create → CaretakersConservatory → World Params`
3. Name it `WorldParamsInstance`
4. In Inspector, set initial values:
   - **Fan Speed**: 250
   - **Wind Strength**: 1.0
   - **Light Intensity**: 3.0
   - **Storm Active**: false

**Note:** This asset will be referenced by all controllers and UI scripts.

---

## Step 3: Set Up Player GameObject

### 3.1 Create Player Root
1. Hierarchy → Right-click → `Create Empty` → Name it `Player`
2. Add Component: `Character Controller`
   - **Height**: 1.8
   - **Radius**: 0.3
   - **Center**: (0, 0.9, 0)
3. Add Component: `Player Controller` (custom script)
   - **Move Speed**: 5
   - **Sprint Multiplier**: 1.5
   - **Look Sensitivity**: 2
   - **Interact Distance**: 3
   - **Interactable Layers**: Set to "Interactable" layer (layer 10)

### 3.2 Add Camera Child
1. Select `Player` in Hierarchy
2. Right-click → `3D Object → Camera`
3. Rename to `PlayerCamera`
4. Set Transform:
   - **Position**: (0, 0.6, 0) — eye level
   - **Rotation**: (0, 0, 0)
5. In Player Controller component, drag `PlayerCamera` into **Camera Transform** field

### 3.3 Set Up Input Actions
1. In Player Inspector, find `Player Input` component (add if missing)
2. Set **Actions**: Drag `Assets/PlayerInputActions.inputactions` asset
3. Set **Default Map**: `Player`
4. Set **Behavior**: `Invoke Unity Events`
5. Wire up events:
   - `Move` → `PlayerController.OnMove`
   - `Look` → `PlayerController.OnLook`
   - `Sprint` → `PlayerController.OnSprint`
   - `Interact` → `PlayerController.OnInteract`

### 3.4 Position Player
Set Player Transform Position to a reasonable spawn point, e.g., (0, 1, -5)

---

## Step 4: Environment Setup

### 4.1 Directional Light (Sun)
1. Hierarchy → Right-click → `Light → Directional Light`
2. Rename to `Sun`
3. Transform Rotation: (50, -30, 0) — angled sunlight
4. Light Component:
   - **Color**: Warm white (#FFF4E0)
   - **Intensity**: 1.2
   - **Mode**: Realtime
5. Add Component: `Lighting Controller`
   - **World Params**: Drag `WorldParamsInstance` asset
   - **Intensity Multiplier**: 1.0

### 4.2 Terrain / Ground Plane
**Option A: Simple Plane (Quick)**
1. Hierarchy → `3D Object → Plane`
2. Scale: (10, 1, 10)
3. Create material with grass texture or solid color
4. Set Layer: `Ground` (layer 8)

**Option B: Terrain (Detailed)**
1. Hierarchy → `3D Object → Terrain`
2. Use Terrain Tools to sculpt conservatory floor
3. Add grass/dirt texture
4. Set Layer: `Ground`

### 4.3 Conservatory Structure
**Placeholder until 3D model available:**
1. Create primitives (cubes) to represent walls, roof, frame
2. Group under empty GameObject named `ConservatoryStructure`
3. Add colliders for interaction boundaries
4. Assign materials (glass, metal textures)

---

## Step 5: Interactive Objects

### 5.1 Fan GameObject
1. Hierarchy → `Create Empty` → Name: `Fan`
2. Child object: `3D Object → Cylinder` (or import fan model) → Name: `FanBlades`
   - Rotate to align spin axis (e.g., Z-forward for horizontal fan)
3. Add Component to `Fan` root: `Fan Controller`
   - **World Params**: `WorldParamsInstance`
   - **Fan Blades**: Drag `FanBlades` transform
   - **Rotation Axis**: (0, 0, 1) — or adjust to model orientation

### 5.2 Plant with Wind Shader
1. Hierarchy → `3D Object → Quad` (placeholder) or import plant mesh
2. Name: `HangingPlant`
3. Create Material:
   - Shader: `PlantWindShader` (create Shader Graph per guide)
   - Assign to plant mesh
4. Position above player eye level (e.g., hanging from ceiling)

**Shader setup:** Follow `Assets/Shaders/PlantWindShader_Guide.md`

### 5.3 Destructible Pot
1. Hierarchy → `3D Object → Cylinder` (or pot mesh) → Name: `Pot`
2. Add Components:
   - `Rigidbody` (Mass: 2, Drag: 0.5)
   - `Box Collider` or `Mesh Collider`
   - `Destructible Pot` (custom script)
     - **Break Impulse Threshold**: 8
     - **Fragments Prefab**: Create prefab with broken pieces (see below)
3. Add Component: `Pickup Interactable`
   - **Throw Force Multiplier**: 6
4. Set Layer: `Interactable` (layer 10)
5. Add Tag: `Destructible`

**Create Fragment Prefab:**
1. Create several small primitive cubes/spheres
2. Each has Rigidbody + Collider
3. Group under empty GameObject → Save as Prefab: `PotFragments`
4. Assign to `Destructible Pot` → `Fragments Prefab` field

---

## Step 6: Storm Effects

### 6.1 Storm Controller GameObject
1. Hierarchy → `Create Empty` → Name: `StormSystem`
2. Add Component: `Storm Controller`
   - **World Params**: `WorldParamsInstance`

### 6.2 Rain Particle System
1. Child of `StormSystem`: Right-click → `Effects → Particle System` → Name: `RainParticles`
2. Configure:
   - **Start Lifetime**: 2
   - **Start Speed**: 10
   - **Emission Rate**: 50
   - **Shape**: Box (cover scene area)
   - **Gravity Modifier**: 1
   - **Play on Awake**: OFF (controlled by StormController)
3. Drag into `Storm Controller` → `Rain Particles` field

### 6.3 Storm Audio
1. Hierarchy → `Create Empty` → Name: `StormAudio` (child of StormSystem)
2. Add Component: `Audio Source`
   - **Clip**: Import thunder/rain audio loop
   - **Loop**: ON
   - **Play on Awake**: OFF
   - **Spatial Blend**: 0 (2D ambient sound)
3. Drag into `Storm Controller` → `Storm Audio` field

### 6.4 Flicker Light
1. Child of `StormSystem`: `Light → Point Light` → Name: `FlickerLight`
2. Light Component:
   - **Color**: White
   - **Intensity**: 0 (starts off)
   - **Range**: 20
3. Drag into `Storm Controller` → `Flicker Light` field
4. Set **Flicker Intensity Range**: (0.5, 2)
5. Set **Flicker Speed**: 10

---

## Step 7: UI Canvas Setup

### 7.1 Create World-Space Canvas
1. Hierarchy → `UI → Canvas`
2. Canvas Component:
   - **Render Mode**: World Space
   - **Event Camera**: Drag `PlayerCamera`
3. Rect Transform:
   - **Position**: (0, 2, 3) — in front of player spawn
   - **Width**: 400, **Height**: 600
   - **Scale**: (0.01, 0.01, 0.01) — world units

### 7.2 Add Control Sliders
Create three sliders for parameters:

**Fan Speed Slider:**
1. Canvas → Right-click → `UI → Slider` → Name: `FanSpeedSlider`
2. Slider Component:
   - **Min Value**: 0, **Max Value**: 1000
   - **Value**: 250
3. Position in canvas layout

**Wind Strength Slider:**
1. Slider → Name: `WindStrengthSlider`
2. **Min**: 0, **Max**: 10, **Value**: 1

**Light Intensity Slider:**
1. Slider → Name: `LightIntensitySlider`
2. **Min**: 0, **Max**: 10, **Value**: 3

### 7.3 Add Storm Toggle
1. Canvas → `UI → Toggle` → Name: `StormToggle`
2. Toggle Component:
   - **Is On**: false
3. Customize label: "Storm Mode"

### 7.4 Add Status Display Panel
1. Canvas → `UI → Panel` → Name: `StatusPanel`
2. Inside Panel, add four `UI → Text` elements:
   - `FanSpeedText` (initial text: "Fan: 250")
   - `WindStrengthText` ("Wind: 1.00")
   - `LightIntensityText` ("Light: 3.00")
   - `StormActiveText` ("Storm: OFF")
3. Position as vertical list

### 7.5 Wire Up UI Controller
1. Canvas root → Add Component: `UI Slider Controller`
   - **World Params**: `WorldParamsInstance`
   - **Fan Speed Slider**: Drag `FanSpeedSlider`
   - **Wind Strength Slider**: Drag `WindStrengthSlider`
   - **Light Intensity Slider**: Drag `LightIntensitySlider`
   - **Storm Toggle**: Drag `StormToggle`

2. StatusPanel → Add Component: `Status Display`
   - **World Params**: `WorldParamsInstance`
   - **Fan Speed Text**: Drag `FanSpeedText`
   - **Wind Strength Text**: Drag `WindStrengthText`
   - **Light Intensity Text**: Drag `LightIntensityText`
   - **Storm Active Text**: Drag `StormActiveText`

---

## Step 8: Wind Controller (Global Shader Parameter)

1. Hierarchy → `Create Empty` → Name: `WindSystem`
2. Add Component: `Wind Controller`
   - **World Params**: `WorldParamsInstance`
   - **Shader Wind Strength Property**: `_GlobalWindStrength` (default)
   - **Shader Scale**: 1.0

This sets the global shader float used by plant materials.

---

## Step 9: Testing Checklist

**Press Play** and verify:

- [ ] Player moves with WASD, looks with mouse
- [ ] Camera follows player rotation
- [ ] Fan speed slider adjusts fan blade rotation
- [ ] Wind strength slider makes plant sway (if shader applied)
- [ ] Light intensity slider changes scene brightness
- [ ] Storm toggle activates rain particles, audio, flicker
- [ ] Status panel updates live values
- [ ] Left-click picks up pot
- [ ] Throw pot (release while moving mouse)
- [ ] Pot breaks into fragments on high-impact collision
- [ ] Frame rate stays ≥30 FPS (check Stats window)

---

## Step 10: Performance Profiling

1. Open `Window → Analysis → Profiler`
2. Run scene in Play mode
3. Check:
   - **CPU**: Should be <33ms per frame (30 FPS target)
   - **Rendering**: Draw calls <100
   - **Physics**: Rigidbody count managed
4. Adjust quality settings if needed (`Edit → Project Settings → Quality`)

---

## Step 11: XR Device Simulator Test

1. `Window → Analysis → XR Device Simulator`
2. In Game view, device controls appear
3. Test XR interaction mappings (hand controllers simulate mouse input)
4. Verify pickup/throw works in simulated VR mode

---

## Step 12: Save and Build

### Save Scene
1. `File → Save` (Ctrl+S)
2. Ensure `Main.unity` is saved in `Assets/Scenes/`

### Build Settings
1. `File → Build Settings`
2. Add `Main.unity` to Scenes in Build
3. Platform: **Windows Standalone**
4. Architecture: **x86_64**
5. Target: **Windows Desktop**
6. Click **Build** → Choose output folder (e.g., `Builds/Windows/`)

---

## Troubleshooting

**Player falls through ground:**
- Ensure ground has Collider component
- Check Player CharacterController is enabled

**Sliders don't update parameters:**
- Verify WorldParamsInstance is assigned in all controllers
- Check UISliderController OnValueChanged events are wired

**Fan doesn't rotate:**
- Confirm FanBlades transform is assigned in FanController
- Check rotation axis matches model orientation

**Plant doesn't sway:**
- Ensure PlantWindShader is created and applied
- Verify WindController sets `_GlobalWindStrength` property
- Check shader uses correct global property name

**No audio in storm mode:**
- Import audio clip and assign to StormAudio AudioSource
- Ensure AudioListener is on PlayerCamera

**Performance below 30 FPS:**
- Reduce particle emission rates
- Lower shadow quality in Quality Settings
- Simplify plant mesh geometry
- Disable post-processing if enabled

---

## Next Steps

- Import polished 3D models (conservatory, fan, pot)
- Add textures and materials (PBR workflow)
- Implement advanced shaders (normal maps, specular highlights)
- Add ambient audio (birds, wind rustling)
- Create fragment variations for destructible objects
- Expand testing to PlayMode automated tests
- Set up CI/CD pipeline for automated builds
- Profile and optimize for VR headset deployment

---

**Scene setup complete!** The project is now runnable with all core features integrated.
