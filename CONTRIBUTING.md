# Contributing to Caretaker's Conservatory

Thank you for your interest in contributing! This document provides guidelines for contributing to the project.

## 📋 Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Code Style](#code-style)
- [Testing](#testing)
- [Pull Request Process](#pull-request-process)
- [Project Structure](#project-structure)

---

## 🤝 Code of Conduct

- Be respectful and inclusive
- Provide constructive feedback
- Focus on what's best for the project and community
- Show empathy towards other contributors

---

## 🚀 Getting Started

### Prerequisites

- **Unity Hub** (latest version)
- **Unity 2022.3.17f1** or later (2022 LTS)
- **Git** (2.30+)
- **Visual Studio Code** or **Visual Studio 2022** (recommended)
- **.NET SDK 6.0+** (for code analysis)

### Initial Setup

1. **Fork the repository** on GitHub

2. **Clone your fork:**
   ```powershell
   git clone https://github.com/YOUR_USERNAME/caretakers-conservatory.git
   cd caretakers-conservatory
   ```

3. **Add upstream remote:**
   ```powershell
   git remote add upstream https://github.com/KumarageKPV/caretakers-conservatory.git
   ```

4. **Open in Unity Hub:**
   - Launch Unity Hub
   - Click "Add" → Select the cloned folder
   - Unity will import packages (5-10 minutes on first load)

5. **Verify package installation:**
   - Open `Window → Package Manager`
   - Ensure all packages from `Packages/manifest.json` are installed

---

## 🔄 Development Workflow

### Branch Strategy

- `main` — Stable releases only
- `dev` — Active development (target for PRs)
- `feature/your-feature` — Feature branches
- `bugfix/issue-number` — Bug fix branches

### Creating a Feature Branch

```powershell
git checkout dev
git pull upstream dev
git checkout -b feature/your-feature-name
```

### Staying Up to Date

```powershell
git fetch upstream
git rebase upstream/dev
```

---

## 🎨 Code Style

### C# Conventions

We follow **Microsoft C# Coding Conventions** with Unity-specific adjustments:

**Naming:**
- `PascalCase` for classes, methods, properties, events
- `camelCase` for private fields (with `_` prefix optional)
- `PascalCase` for public fields (Unity serialized fields)
- `IPascalCase` for interfaces (prefix with `I`)

**Example:**
```csharp
public class FanController : MonoBehaviour
{
    [SerializeField] private WorldParams worldParams;
    private float currentSpeed;
    
    public event Action<float> SpeedChanged;
    
    public void UpdateSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
        SpeedChanged?.Invoke(currentSpeed);
    }
}
```

### EditorConfig

This project uses `.editorconfig` for consistent formatting:
- 4 spaces for C# indentation
- CRLF line endings (Windows)
- UTF-8 encoding
- Trim trailing whitespace

**VS Code:** Install "EditorConfig for VS Code" extension  
**Visual Studio:** Built-in support

### Code Organization

- **One class per file** (except nested classes)
- **Namespace:** All scripts in `CaretakersConservatory` namespace
- **File structure:**
  ```
  Assets/
    Scripts/
      Controllers/     # Scene behavior controllers
      Interaction/     # Player interaction scripts
      UI/              # UI-specific scripts
      Data/            # ScriptableObjects
  ```

### Comments and Documentation

- Add XML documentation for public APIs:
  ```csharp
  /// <summary>
  /// Controls fan rotation based on WorldParams events.
  /// </summary>
  public class FanController : MonoBehaviour
  {
      /// <summary>
      /// Subscribes to FanSpeedChanged event and initializes rotation.
      /// </summary>
      private void OnEnable() { }
  }
  ```

- Use inline comments sparingly (code should be self-documenting)
- Document **why**, not **what** (unless complex algorithm)

---

## 🧪 Testing

### Running Tests

**In Unity Editor:**
1. Open `Window → General → Test Runner`
2. Select `EditMode` tab
3. Click "Run All"

**Via Command Line:**
```powershell
# Requires Unity installed and activated
Unity.exe -runTests -batchmode -projectPath . -testResults results.xml -testPlatform EditMode
```

### Writing Tests

- **Location:** `Assets/Tests/EditMode/`
- **Framework:** NUnit 3.x
- **Naming:** `ClassNameTests.cs`

**Example:**
```csharp
using NUnit.Framework;
using UnityEngine;

namespace CaretakersConservatory.Tests
{
    public class WorldParamsTests
    {
        [Test]
        public void FanSpeed_Event_Fires_On_Change()
        {
            var wp = ScriptableObject.CreateInstance<WorldParams>();
            int count = 0;
            wp.FanSpeedChanged += _ => count++;
            
            wp.FanSpeed = 100f;
            Assert.AreEqual(1, count);
        }
    }
}
```

### Test Coverage Goals

- **Core logic:** 80%+ coverage
- **MonoBehaviours:** Test event subscriptions and public methods
- **ScriptableObjects:** Test property setters and event firing

---

## 📬 Pull Request Process

### Before Submitting

1. **Run all tests:**
   ```powershell
   # In Unity Test Runner
   Run All → Ensure all pass
   ```

2. **Check code formatting:**
   ```powershell
   # Format C# files (if dotnet format available)
   dotnet format
   ```

3. **Update documentation:**
   - Update `README.md` if adding features
   - Add/update XML comments for new public APIs
   - Update `IMPLEMENTATION_SUMMARY.md` if needed

4. **Test in Unity Editor:**
   - Verify no console errors
   - Test feature manually in Play mode
   - Check performance impact (Profiler)

### Creating a Pull Request

1. **Push to your fork:**
   ```powershell
   git push origin feature/your-feature-name
   ```

2. **Open PR on GitHub:**
   - Target branch: `dev`
   - Title: Clear, concise description
   - Description: Use PR template (see `.github/PULL_REQUEST_TEMPLATE.md`)

3. **PR Checklist:**
   - [ ] All tests pass
   - [ ] No merge conflicts with `dev`
   - [ ] Code follows style guide
   - [ ] Documentation updated
   - [ ] Commits are clean (squash if necessary)

### PR Review Process

- Maintainers will review within **2-3 business days**
- Address feedback by pushing new commits
- Once approved, maintainer will merge

### Commit Messages

Follow **Conventional Commits** format:

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation only
- `style:` Code formatting (no logic change)
- `refactor:` Code restructuring
- `test:` Adding/updating tests
- `chore:` Build process, dependencies

**Examples:**
```
feat(player): add sprint mechanic with shift key

Add sprint multiplier to PlayerController and wire up Input System binding.

Closes #42
```

```
fix(destructible): correct impulse threshold calculation

The previous threshold was too sensitive, causing pots to break on light contact.
Increased threshold from 5 to 8 based on playtesting.

Fixes #38
```

---

## 📂 Project Structure

```
caretakers-conservatory/
├── .github/
│   ├── workflows/          # CI/CD pipelines
│   └── ISSUE_TEMPLATE/     # Issue templates
├── Assets/
│   ├── Scenes/             # Unity scenes + setup guide
│   ├── Scripts/            # C# gameplay scripts
│   │   ├── *.cs            # MonoBehaviours and ScriptableObjects
│   │   └── *.asmdef        # Assembly definition
│   ├── Shaders/            # Shader Graph guides
│   ├── Tests/              # Unit and integration tests
│   │   └── EditMode/       # NUnit EditMode tests
│   └── PlayerInputActions  # Input System bindings
├── Packages/
│   └── manifest.json       # Package dependencies
├── ProjectSettings/        # Unity project configuration
├── .editorconfig           # Code formatting rules
├── .gitignore              # Git ignore patterns
├── README.md               # Project overview
├── CONTRIBUTING.md         # This file
└── LICENSE                 # MIT License

```

---

## 🐛 Reporting Bugs

Use the **Bug Report** template when creating an issue:

1. Navigate to [Issues](https://github.com/KumarageKPV/caretakers-conservatory/issues)
2. Click "New Issue" → Select "Bug Report"
3. Provide:
   - Unity version
   - Platform (Windows/Linux/Mac)
   - Steps to reproduce
   - Expected vs actual behavior
   - Screenshots/logs if applicable

---

## 💡 Requesting Features

Use the **Feature Request** template:

1. Click "New Issue" → Select "Feature Request"
2. Describe:
   - Use case and motivation
   - Proposed implementation (optional)
   - Alternatives considered

---

## ❓ Questions?

- **Discord:** [Join our community](https://discord.gg/placeholder) (if available)
- **Discussions:** Use [GitHub Discussions](https://github.com/KumarageKPV/caretakers-conservatory/discussions)
- **Email:** [maintainer email if public]

---

## 🙏 Thank You!

Your contributions make this project better. We appreciate your time and effort!
