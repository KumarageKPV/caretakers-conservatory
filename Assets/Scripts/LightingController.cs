using UnityEngine;

namespace CaretakersConservatory
{
    [RequireComponent(typeof(Light))]
    public class LightingController : MonoBehaviour
    {
        [SerializeField] private WorldParams worldParams;
        [SerializeField] private float intensityMultiplier = 1f;
        private Light _light;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        private void OnEnable()
        {
            if (worldParams != null)
            {
                worldParams.LightIntensityChanged += OnLightChanged;
                OnLightChanged(worldParams.LightIntensity);
            }
        }

        private void OnDisable()
        {
            if (worldParams != null)
            {
                worldParams.LightIntensityChanged -= OnLightChanged;
            }
        }

        private void OnLightChanged(float intensity)
        {
            if (_light != null)
            {
                _light.intensity = intensity * intensityMultiplier;
            }
        }
    }
}
