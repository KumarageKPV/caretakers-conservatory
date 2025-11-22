using UnityEngine;
using UnityEngine.UI;

namespace CaretakersConservatory
{
    public class StatusDisplay : MonoBehaviour
    {
        [SerializeField] private WorldParams worldParams;
        [SerializeField] private Text fanSpeedText;
        [SerializeField] private Text windStrengthText;
        [SerializeField] private Text lightIntensityText;
        [SerializeField] private Text stormActiveText;

        private void OnEnable()
        {
            if (worldParams == null) return;
            worldParams.FanSpeedChanged += OnFanSpeed;
            worldParams.WindStrengthChanged += OnWindStrength;
            worldParams.LightIntensityChanged += OnLightIntensity;
            worldParams.StormActiveChanged += OnStormActive;
            // Initial sync
            OnFanSpeed(worldParams.FanSpeed);
            OnWindStrength(worldParams.WindStrength);
            OnLightIntensity(worldParams.LightIntensity);
            OnStormActive(worldParams.StormActive);
        }

        private void OnDisable()
        {
            if (worldParams == null) return;
            worldParams.FanSpeedChanged -= OnFanSpeed;
            worldParams.WindStrengthChanged -= OnWindStrength;
            worldParams.LightIntensityChanged -= OnLightIntensity;
            worldParams.StormActiveChanged -= OnStormActive;
        }

        private void OnFanSpeed(float v)
        {
            if (fanSpeedText != null) fanSpeedText.text = $"Fan: {v:F0}";
        }
        private void OnWindStrength(float v)
        {
            if (windStrengthText != null) windStrengthText.text = $"Wind: {v:F2}";
        }
        private void OnLightIntensity(float v)
        {
            if (lightIntensityText != null) lightIntensityText.text = $"Light: {v:F2}";
        }
        private void OnStormActive(bool v)
        {
            if (stormActiveText != null) stormActiveText.text = v ? "Storm: ON" : "Storm: OFF";
        }
    }
}
