# Plant Wind Shader - Creation Guide

This document describes how to create the plant wind animation Shader Graph in Unity Editor.

## Overview
The plant shader uses vertex displacement driven by noise and a global wind strength parameter to simulate organic swaying motion.

## Steps to Create in Unity Editor

### 1. Create New Shader Graph
1. In Unity Editor, navigate to `Assets/Shaders/`
2. Right-click → `Create → Shader Graph → URP → Lit Shader Graph`
3. Name it `PlantWindShader`

### 2. Add Graph Properties
Add these properties in the Shader Graph Blackboard:

- `_BaseColor` (Color, default: green #4CAF50)
- `_Smoothness` (Float, Range 0-1, default: 0.3)
- `_WindSpeed` (Float, default: 1.0)
- `_WindScale` (Float, default: 0.5)
- `_WindAmount` (Float, default: 0.2)

### 3. Create Global Wind Strength Reference
1. Add a new property: `_GlobalWindStrength` (Float, default: 1.0)
2. Mark it as **Global** in the property settings
3. This will be set by `WindController.cs` at runtime via `Shader.SetGlobalFloat()`

### 4. Build Vertex Displacement Logic

**Node Chain:**
```
Position (Object Space)
  ↓
Simple Noise Node
  - UV: Position.xz (swizzle node)
  - Scale: multiply Position.xz by _WindScale property
  ↓
Multiply
  - A: Simple Noise output
  - B: _WindAmount property
  ↓
Multiply
  - A: Previous result
  - B: _GlobalWindStrength (global property)
  ↓
Multiply
  - A: Previous result
  - B: Time node (scaled by _WindSpeed)
  ↓
Add to Position.y (only affect vertical axis)
  ↓
Set to Vertex Position (in Vertex shader stage)
```

### 5. Configure Surface Properties
In the Master Stack (Fragment shader):

- **Base Color**: Connect `_BaseColor` property
- **Smoothness**: Connect `_Smoothness` property
- **Alpha**: 1.0 (opaque)
- **Normal**: Default (or normal map if available)

### 6. Shader Settings
- **Surface Type**: Opaque
- **Render Face**: Front (or Both if two-sided leaves)
- **Blend Mode**: N/A (opaque)

### 7. Save and Apply
1. Save the Shader Graph asset
2. Create a Material using this shader
3. Apply the material to the plant mesh
4. Adjust properties in material inspector to tune wind effect

## Runtime Integration

The `WindController.cs` script sets the global wind strength:

```csharp
Shader.SetGlobalFloat("_GlobalWindStrength", windStrength);
```

This allows all plant materials to respond uniformly to the WorldParams wind strength parameter.

## Performance Notes
- Simple Noise is GPU-friendly (no texture lookups)
- Vertex displacement happens in vertex shader (cheaper than fragment)
- Target: <50 vertices per plant mesh for optimal performance on GTX 1650

## Alternative Approach (Advanced)
For more complex wind patterns, replace Simple Noise with:
- **Gradient Noise** (smoother but slightly more expensive)
- **Voronoi Noise** (for clustered movement)
- **Curl Noise** (for vortex-like patterns, requires custom function)

Current implementation uses Simple Noise for speed and clarity.
