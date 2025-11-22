using NUnit.Framework;
using UnityEngine;

namespace CaretakersConservatory.Tests
{
    public class WorldParamsTests
    {
        [Test]
        public void FanSpeed_Event_Fires_On_Change()
        {
            var wp = ScriptableObject.CreateInstance<CaretakersConservatory.WorldParams>();
            int count = 0;
            wp.FanSpeedChanged += _ => count++;
            wp.FanSpeed = wp.FanSpeed; // same value, should NOT fire
            Assert.AreEqual(0, count);
            wp.FanSpeed = wp.FanSpeed + 10f; // change
            Assert.AreEqual(1, count);
        }

        [Test]
        public void StormActive_Event_Fires_On_Toggle()
        {
            var wp = ScriptableObject.CreateInstance<CaretakersConservatory.WorldParams>();
            int count = 0;
            wp.StormActiveChanged += _ => count++;
            wp.StormActive = wp.StormActive; // no fire
            Assert.AreEqual(0, count);
            wp.StormActive = !wp.StormActive; // fire
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ApplyAll_Sets_All_Values()
        {
            var wp = ScriptableObject.CreateInstance<CaretakersConservatory.WorldParams>();
            wp.ApplyAll(123f, 4.2f, 7.7f, true);
            Assert.AreEqual(123f, wp.FanSpeed);
            Assert.AreEqual(4.2f, wp.WindStrength);
            Assert.AreEqual(7.7f, wp.LightIntensity);
            Assert.IsTrue(wp.StormActive);
        }
    }
}
