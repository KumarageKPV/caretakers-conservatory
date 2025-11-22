#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CaretakersConservatory.Editor
{
    public class PerformanceBenchmark
    {
        [MenuItem("Tools/Caretakers Conservatory/Run Performance Benchmark")]
        public static void RunBenchmark()
        {
            Debug.Log("=== Caretaker's Conservatory Performance Benchmark ===");
            Debug.Log($"Unity Version: {Application.unityVersion}");
            Debug.Log($"Platform: {Application.platform}");
            Debug.Log($"System: {SystemInfo.processorType} | {SystemInfo.systemMemorySize}MB RAM");
            Debug.Log($"GPU: {SystemInfo.graphicsDeviceName}");
            Debug.Log("");

            BenchmarkWorldParamsEvents();
            BenchmarkScriptableObjectCreation();
            BenchmarkEventSubscriptions();

            Debug.Log("=== Benchmark Complete ===");
        }

        private static void BenchmarkWorldParamsEvents()
        {
            Debug.Log("--- WorldParams Event Performance ---");

            var worldParams = ScriptableObject.CreateInstance<WorldParams>();
            var sw = Stopwatch.StartNew();

            // Test event firing performance
            int eventCount = 0;
            worldParams.FanSpeedChanged += _ => eventCount++;
            worldParams.WindStrengthChanged += _ => eventCount++;
            worldParams.LightIntensityChanged += _ => eventCount++;
            worldParams.StormActiveChanged += _ => eventCount++;

            const int iterations = 10000;
            for (int i = 0; i < iterations; i++)
            {
                worldParams.FanSpeed = Random.Range(0f, 1000f);
                worldParams.WindStrength = Random.Range(0f, 10f);
                worldParams.LightIntensity = Random.Range(0f, 10f);
                worldParams.StormActive = i % 2 == 0;
            }

            sw.Stop();
            Debug.Log($"  {iterations * 4} parameter updates: {sw.ElapsedMilliseconds}ms");
            Debug.Log($"  Average per update: {(sw.ElapsedMilliseconds / (float)(iterations * 4)):F4}ms");
            Debug.Log($"  Events fired: {eventCount}");
            Debug.Log($"  Updates/sec (estimated): {(iterations * 4) / (sw.ElapsedMilliseconds / 1000f):F0}");

            Object.DestroyImmediate(worldParams);
        }

        private static void BenchmarkScriptableObjectCreation()
        {
            Debug.Log("\n--- ScriptableObject Creation Performance ---");

            var sw = Stopwatch.StartNew();
            const int createCount = 1000;

            for (int i = 0; i < createCount; i++)
            {
                var wp = ScriptableObject.CreateInstance<WorldParams>();
                Object.DestroyImmediate(wp);
            }

            sw.Stop();
            Debug.Log($"  {createCount} create/destroy cycles: {sw.ElapsedMilliseconds}ms");
            Debug.Log($"  Average per cycle: {(sw.ElapsedMilliseconds / (float)createCount):F4}ms");
        }

        private static void BenchmarkEventSubscriptions()
        {
            Debug.Log("\n--- Event Subscription Performance ---");

            var worldParams = ScriptableObject.CreateInstance<WorldParams>();
            var sw = Stopwatch.StartNew();

            const int subscriptionCount = 100;
            System.Action<float>[] handlers = new System.Action<float>[subscriptionCount];

            // Subscribe multiple handlers
            for (int i = 0; i < subscriptionCount; i++)
            {
                int index = i;
                handlers[i] = _ => { /* Simulate work */ };
                worldParams.FanSpeedChanged += handlers[i];
            }

            sw.Stop();
            long subscribeTime = sw.ElapsedMilliseconds;

            // Test event propagation with many subscribers
            sw.Restart();
            const int propagationIterations = 1000;
            for (int i = 0; i < propagationIterations; i++)
            {
                worldParams.FanSpeed = Random.Range(0f, 1000f);
            }
            sw.Stop();

            Debug.Log($"  {subscriptionCount} subscriptions: {subscribeTime}ms");
            Debug.Log($"  {propagationIterations} event propagations ({subscriptionCount} handlers): {sw.ElapsedMilliseconds}ms");
            Debug.Log($"  Average per propagation: {(sw.ElapsedMilliseconds / (float)propagationIterations):F4}ms");

            // Unsubscribe
            sw.Restart();
            for (int i = 0; i < subscriptionCount; i++)
            {
                worldParams.FanSpeedChanged -= handlers[i];
            }
            sw.Stop();
            Debug.Log($"  {subscriptionCount} unsubscriptions: {sw.ElapsedMilliseconds}ms");

            Object.DestroyImmediate(worldParams);
        }

        [MenuItem("Tools/Caretakers Conservatory/Validate Project Setup")]
        public static void ValidateProjectSetup()
        {
            Debug.Log("=== Project Setup Validation ===");

            int issues = 0;

            // Check assembly definitions
            if (!AssetExists("Assets/Scripts/CaretakersConservatory.asmdef"))
            {
                Debug.LogWarning("Missing: CaretakersConservatory.asmdef");
                issues++;
            }

            if (!AssetExists("Assets/Tests/EditMode/CaretakersConservatory.Tests.asmdef"))
            {
                Debug.LogWarning("Missing: CaretakersConservatory.Tests.asmdef");
                issues++;
            }

            // Check core scripts
            string[] requiredScripts = new[]
            {
                "Assets/Scripts/WorldParams.cs",
                "Assets/Scripts/FanController.cs",
                "Assets/Scripts/LightingController.cs",
                "Assets/Scripts/WindController.cs",
                "Assets/Scripts/StormController.cs",
                "Assets/Scripts/PlayerController.cs",
                "Assets/Scripts/PickupInteractable.cs",
                "Assets/Scripts/DestructiblePot.cs",
                "Assets/Scripts/UISliderController.cs",
                "Assets/Scripts/StatusDisplay.cs"
            };

            foreach (var script in requiredScripts)
            {
                if (!AssetExists(script))
                {
                    Debug.LogWarning($"Missing: {script}");
                    issues++;
                }
            }

            // Check Input Actions
            if (!AssetExists("Assets/PlayerInputActions.inputactions"))
            {
                Debug.LogWarning("Missing: PlayerInputActions.inputactions");
                issues++;
            }

            // Check package dependencies
            if (!IsPackageInstalled("com.unity.render-pipelines.universal"))
            {
                Debug.LogError("Missing Package: Universal RP");
                issues++;
            }

            if (!IsPackageInstalled("com.unity.inputsystem"))
            {
                Debug.LogError("Missing Package: Input System");
                issues++;
            }

            if (!IsPackageInstalled("com.unity.xr.interaction.toolkit"))
            {
                Debug.LogWarning("Missing Package: XR Interaction Toolkit (optional for XR support)");
            }

            if (issues == 0)
            {
                Debug.Log("<color=green>✓ All core project files present!</color>");
            }
            else
            {
                Debug.LogWarning($"Found {issues} missing files or packages.");
            }

            Debug.Log("=== Validation Complete ===");
        }

        private static bool AssetExists(string path)
        {
            return !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path));
        }

        private static bool IsPackageInstalled(string packageName)
        {
            var request = UnityEditor.PackageManager.Client.List(true, false);
            while (!request.IsCompleted) { }

            if (request.Status == UnityEditor.PackageManager.StatusCode.Success)
            {
                foreach (var package in request.Result)
                {
                    if (package.name == packageName)
                        return true;
                }
            }

            return false;
        }
    }
}
#endif
