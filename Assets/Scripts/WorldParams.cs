using System;
using UnityEngine;

namespace CaretakersConservatory
{
    [CreateAssetMenu(fileName = "WorldParams", menuName = "CaretakersConservatory/World Params", order = 0)]
    public class WorldParams : ScriptableObject
    {
        [SerializeField, Range(0f, 1000f)] private float fanSpeed = 250f;
        [SerializeField, Range(0f, 10f)] private float windStrength = 1f;
        [SerializeField, Range(0f, 10f)] private float lightIntensity = 3f;
        [SerializeField] private bool stormActive = false;

        public event Action<float> FanSpeedChanged;
        public event Action<float> WindStrengthChanged;
        public event Action<float> LightIntensityChanged;
        public event Action<bool> StormActiveChanged;

        public float FanSpeed
        {
            get => fanSpeed;
            set
            {
                if (Mathf.Approximately(fanSpeed, value)) return;
                fanSpeed = value;
                FanSpeedChanged?.Invoke(fanSpeed);
            }
        }

        public float WindStrength
        {
            get => windStrength;
            set
            {
                if (Mathf.Approximately(windStrength, value)) return;
                windStrength = value;
                WindStrengthChanged?.Invoke(windStrength);
            }
        }

        public float LightIntensity
        {
            get => lightIntensity;
            set
            {
                if (Mathf.Approximately(lightIntensity, value)) return;
                lightIntensity = value;
                LightIntensityChanged?.Invoke(lightIntensity);
            }
        }

        public bool StormActive
        {
            get => stormActive;
            set
            {
                if (stormActive == value) return;
                stormActive = value;
                StormActiveChanged?.Invoke(stormActive);
            }
        }

        public void ApplyAll(float newFanSpeed, float newWindStrength, float newLightIntensity, bool newStormActive)
        {
            FanSpeed = newFanSpeed;
            WindStrength = newWindStrength;
            LightIntensity = newLightIntensity;
            StormActive = newStormActive;
        }
    }
}
