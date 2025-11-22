# GitHub Actions CI/CD Setup Guide

This guide explains how to configure Unity license secrets for the GitHub Actions CI/CD pipeline.

## Overview

The CI/CD workflow (`.github/workflows/ci.yml`) requires Unity license configuration to run automated tests and builds. Without proper secrets, the workflow will skip Unity-related jobs and only run code linting.

## Current Workflow Behavior

**Without Unity Secrets:**
- ✓ Lint job runs (file structure validation, EditorConfig check)
- ⏭️ Test job skipped
- ⏭️ Build job skipped

**With Unity Secrets:**
- ✓ Lint job runs
- ✓ Test job runs (EditMode NUnit tests)
- ✓ Build job runs (Windows x64 build on `main`/`dev` branches)

---

## Option 1: Personal/Free Unity License (Recommended for Individual Developers)

### Step 1: Activate Unity License Locally

1. Open Unity Hub
2. Sign in with your Unity account
3. Open the project in Unity Editor
4. Unity will activate automatically (or manually via `Help → Manage License`)

### Step 2: Generate License File

Run this command in a terminal from your Unity project root:

```powershell
# Windows PowerShell
$env:UNITY_PATH = "C:\Program Files\Unity\Hub\Editor\2022.3.17f1\Editor\Unity.exe"
& $env:UNITY_PATH -batchmode -nographics -quit -logFile - -createManualActivationFile

# This creates Unity_v2022.x.alf file
```

### Step 3: Manually Activate License

1. Go to https://license.unity3d.com/manual
2. Upload the `.alf` file
3. Download the `.ulf` license file

### Step 4: Convert License to Base64

```powershell
# Windows PowerShell
$licenseContent = Get-Content -Path "Unity_v2022.x.ulf" -Raw
$bytes = [System.Text.Encoding]::UTF8.GetBytes($licenseContent)
$base64 = [Convert]::ToBase64String($bytes)
$base64 | Set-Content -Path "unity_license_base64.txt"

# Copy the contents of unity_license_base64.txt
```

### Step 5: Add GitHub Secret

1. Go to your GitHub repository
2. Navigate to `Settings → Secrets and variables → Actions`
3. Click `New repository secret`
4. Name: `UNITY_LICENSE`
5. Value: Paste the base64-encoded license content
6. Click `Add secret`

### Step 6: Add Email and Password (Optional but Recommended)

Add two more secrets:

- `UNITY_EMAIL`: Your Unity account email
- `UNITY_PASSWORD`: Your Unity account password

**Note:** These are used for Unity's authentication but are optional if you use `UNITY_LICENSE`.

---

## Option 2: Unity Plus/Pro Serial Number

If you have a Unity Plus or Pro subscription:

### Add GitHub Secret

1. Go to `Settings → Secrets and variables → Actions`
2. Add `UNITY_SERIAL` secret with your serial number
3. Add `UNITY_EMAIL` and `UNITY_PASSWORD` secrets

---

## Option 3: Disable Unity CI/CD (Use Local Testing Only)

If you don't want to set up Unity CI/CD, the workflow is already configured to gracefully skip Unity jobs. Only the lint job will run, validating:

- EditorConfig presence
- Required script files
- Project structure

**To run tests locally:**
1. Open Unity Editor
2. `Window → General → Test Runner`
3. Select `EditMode` tab
4. Click `Run All`

---

## Verifying Setup

After adding secrets, push a commit to trigger the workflow:

```powershell
git add .
git commit -m "test: verify CI/CD setup"
git push origin dev
```

Check the Actions tab on GitHub to see if jobs run successfully.

---

## Troubleshooting

### Error: "Missing Unity License File"

**Solution:** Add `UNITY_LICENSE` secret (Option 1) or `UNITY_SERIAL` + `UNITY_EMAIL` + `UNITY_PASSWORD` (Option 2).

### Error: "License activation failed"

**Solution:** Ensure your Unity account has an active license and the base64 encoding is correct.

### Workflow Skips All Jobs

**Solution:** Check the `if:` conditions in `.github/workflows/ci.yml` and ensure secrets are named exactly:
- `UNITY_LICENSE` or `UNITY_SERIAL`
- `UNITY_EMAIL`
- `UNITY_PASSWORD`

### License Expired

**Solution:** Re-activate locally and regenerate the `.ulf` file, then update the `UNITY_LICENSE` secret.

---

## Security Notes

- **Never commit** `.ulf` or `.alf` files to the repository
- **Never commit** base64-encoded license strings
- GitHub Secrets are encrypted and only exposed to workflow runs
- Secrets are not accessible in forked repositories (by design)

---

## Alternative: Use GameCI's License Activation Action

For advanced setups, consider using GameCI's activation action:

```yaml
- name: Request manual activation file
  uses: game-ci/unity-request-activation-file@v2
```

See: https://game.ci/docs/github/activation

---

## Current Workflow Status

✓ **Workflow is production-ready** but requires Unity secrets to run tests/builds  
✓ **Lint job always runs** (no secrets needed)  
✓ **Fork-friendly** (skips Unity jobs if secrets unavailable)

---

**For more information:**
- [Unity Manual Activation](https://docs.unity3d.com/Manual/ManualActivationGuide.html)
- [GameCI Documentation](https://game.ci/docs)
- [GitHub Actions Secrets](https://docs.github.com/en/actions/security-guides/encrypted-secrets)
