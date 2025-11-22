using UnityEngine;
using UnityEngine.UI;

namespace CaretakersConservatory
{
    public class UISliderController : MonoBehaviour
    {
        [SerializeField] private WorldParams worldParams;
        
        [Header("UI Elements")]
        [SerializeField] private Slider fanSpeedSlider;
        [SerializeField] private Slider windStrengthSlider;
        [SerializeField] private Slider lightIntensitySlider;
        [SerializeField] private Toggle stormToggle;

        private void Start()
        {
            if (worldParams == null)
            {
                Debug.LogError("WorldParams not assigned to UISliderController!");
                return;
            }

            // Initialize UI elements to current parameter values
            if (fanSpeedSlider != null)
            {
                fanSpeedSlider.minValue = 0f;
                fanSpeedSlider.maxValue = 1000f;
                fanSpeedSlider.value = worldParams.FanSpeed;
                fanSpeedSlider.onValueChanged.AddListener(OnFanSpeedChanged);
            }

            if (windStrengthSlider != null)
            {
                windStrengthSlider.minValue = 0f;
                windStrengthSlider.maxValue = 10f;
                windStrengthSlider.value = worldParams.WindStrength;
                windStrengthSlider.onValueChanged.AddListener(OnWindStrengthChanged);
            }

            if (lightIntensitySlider != null)
            {
                lightIntensitySlider.minValue = 0f;
                lightIntensitySlider.maxValue = 10f;
                lightIntensitySlider.value = worldParams.LightIntensity;
                lightIntensitySlider.onValueChanged.AddListener(OnLightIntensityChanged);
            }

            if (stormToggle != null)
            {
                stormToggle.isOn = worldParams.StormActive;
                stormToggle.onValueChanged.AddListener(OnStormToggleChanged);
            }
        }

        private void OnDestroy()
        {
            if (fanSpeedSlider != null) fanSpeedSlider.onValueChanged.RemoveListener(OnFanSpeedChanged);
            if (windStrengthSlider != null) windStrengthSlider.onValueChanged.RemoveListener(OnWindStrengthChanged);
            if (lightIntensitySlider != null) lightIntensitySlider.onValueChanged.RemoveListener(OnLightIntensityChanged);
            if (stormToggle != null) stormToggle.onValueChanged.RemoveListener(OnStormToggleChanged);
        }

        private void OnFanSpeedChanged(float value)
        {
            if (worldParams != null) worldParams.FanSpeed = value;
        }

        private void OnWindStrengthChanged(float value)
        {
            if (worldParams != null) worldParams.WindStrength = value;
        }

        private void OnLightIntensityChanged(float value)
        {
            if (worldParams != null) worldParams.LightIntensity = value;
        }

        private void OnStormToggleChanged(bool value)
        {
            if (worldParams != null) worldParams.StormActive = value;
        }
    }
}
