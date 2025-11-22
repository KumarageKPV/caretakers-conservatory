using NUnit.Framework;
using UnityEngine;

namespace CaretakersConservatory.Tests
{
    public class FanControllerTests
    {
        private GameObject fanObject;
        private GameObject bladesObject;
        private FanController controller;
        private WorldParams worldParams;

        [SetUp]
        public void Setup()
        {
            worldParams = ScriptableObject.CreateInstance<WorldParams>();
            
            fanObject = new GameObject("Fan");
            bladesObject = new GameObject("Blades");
            bladesObject.transform.SetParent(fanObject.transform);
            
            controller = fanObject.AddComponent<FanController>();
            
            // Use reflection to set private serialized fields
            var worldParamsField = typeof(FanController).GetField("worldParams", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            worldParamsField?.SetValue(controller, worldParams);
            
            var fanBladesField = typeof(FanController).GetField("fanBlades", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            fanBladesField?.SetValue(controller, bladesObject.transform);
        }

        [Test]
        public void FanController_SubscribesToFanSpeedChanges()
        {
            controller.OnEnable();
            
            Vector3 initialRotation = bladesObject.transform.eulerAngles;
            worldParams.FanSpeed = 100f;
            
            // Simulate one frame of Update
            controller.Update();
            
            // Rotation should have changed (we can't test exact value without Time.deltaTime)
            Assert.AreNotEqual(initialRotation.z, bladesObject.transform.eulerAngles.z, 0.01f);
        }

        [Test]
        public void FanController_StopsWhenSpeedIsZero()
        {
            controller.OnEnable();
            worldParams.FanSpeed = 0f;
            
            Vector3 initialRotation = bladesObject.transform.eulerAngles;
            controller.Update();
            
            Assert.AreEqual(initialRotation.z, bladesObject.transform.eulerAngles.z, 0.001f);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(fanObject);
            Object.DestroyImmediate(worldParams);
        }
    }

    public class DestructiblePotTests
    {
        private GameObject potObject;
        private DestructiblePot destructible;

        [SetUp]
        public void Setup()
        {
            potObject = new GameObject("Pot");
            potObject.AddComponent<Rigidbody>();
            destructible = potObject.AddComponent<DestructiblePot>();
            
            var rbField = typeof(DestructiblePot).GetField("rb", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            rbField?.SetValue(destructible, potObject.GetComponent<Rigidbody>());
        }

        [Test]
        public void DestructiblePot_HasRigidbodyAssigned()
        {
            Assert.IsNotNull(potObject.GetComponent<Rigidbody>());
        }

        [Test]
        public void DestructiblePot_BreakThresholdIsPositive()
        {
            var thresholdField = typeof(DestructiblePot).GetField("breakImpulseThreshold", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            float threshold = (float)thresholdField.GetValue(destructible);
            
            Assert.Greater(threshold, 0f);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(potObject);
        }
    }

    public class PickupInteractableTests
    {
        private GameObject pickupObject;
        private PickupInteractable pickup;
        private GameObject holderObject;

        [SetUp]
        public void Setup()
        {
            pickupObject = new GameObject("PickupObject");
            pickupObject.AddComponent<Rigidbody>();
            pickup = pickupObject.AddComponent<PickupInteractable>();
            
            var rbField = typeof(PickupInteractable).GetField("rb", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            rbField?.SetValue(pickup, pickupObject.GetComponent<Rigidbody>());
            
            holderObject = new GameObject("Holder");
        }

        [Test]
        public void PickupInteractable_PickupMakesKinematic()
        {
            pickup.Pickup(holderObject.transform);
            
            Assert.IsTrue(pickupObject.GetComponent<Rigidbody>().isKinematic);
        }

        [Test]
        public void PickupInteractable_DropRestoresPhysics()
        {
            pickup.Pickup(holderObject.transform);
            pickup.Drop(applyThrow: false);
            
            Assert.IsFalse(pickupObject.GetComponent<Rigidbody>().isKinematic);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(pickupObject);
            Object.DestroyImmediate(holderObject);
        }
    }

    public class LightingControllerTests
    {
        private GameObject lightObject;
        private Light lightComponent;
        private LightingController controller;
        private WorldParams worldParams;

        [SetUp]
        public void Setup()
        {
            worldParams = ScriptableObject.CreateInstance<WorldParams>();
            
            lightObject = new GameObject("Light");
            lightComponent = lightObject.AddComponent<Light>();
            controller = lightObject.AddComponent<LightingController>();
            
            var worldParamsField = typeof(LightingController).GetField("worldParams", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            worldParamsField?.SetValue(controller, worldParams);
            
            controller.Awake();
        }

        [Test]
        public void LightingController_UpdatesIntensityOnChange()
        {
            controller.OnEnable();
            
            worldParams.LightIntensity = 5.0f;
            
            Assert.AreEqual(5.0f, lightComponent.intensity, 0.01f);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(lightObject);
            Object.DestroyImmediate(worldParams);
        }
    }
}
