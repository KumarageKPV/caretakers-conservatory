using UnityEngine;

namespace CaretakersConservatory
{
    public class StormController : MonoBehaviour
    {
        [SerializeField] private WorldParams worldParams;
        [Header("Storm Components")] [SerializeField] private ParticleSystem rainParticles;
        [SerializeField] private AudioSource stormAudio;
        [SerializeField] private Light flickerLight;
        [SerializeField] private Vector2 flickerIntensityRange = new Vector2(0.5f, 2f);
        [SerializeField] private float flickerSpeed = 10f;

        private bool _active;

        private void OnEnable()
        {
            if (worldParams != null)
            {
                worldParams.StormActiveChanged += OnStormChanged;
                OnStormChanged(worldParams.StormActive);
            }
        }

        private void OnDisable()
        {
            if (worldParams != null)
            {
                worldParams.StormActiveChanged -= OnStormChanged;
            }
        }

        private void Update()
        {
            if (_active && flickerLight != null)
            {
                float t = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
                flickerLight.intensity = Mathf.Lerp(flickerIntensityRange.x, flickerIntensityRange.y, t);
            }
        }

        private void OnStormChanged(bool active)
        {
            _active = active;
            if (rainParticles != null)
            {
                if (active && !rainParticles.isPlaying) rainParticles.Play();
                else if (!active && rainParticles.isPlaying) rainParticles.Stop();
            }
            if (stormAudio != null)
            {
                if (active && !stormAudio.isPlaying) stormAudio.Play();
                else if (!active && stormAudio.isPlaying) stormAudio.Stop();
            }
            if (flickerLight != null && !active)
            {
                flickerLight.intensity = 0f;
            }
        }
    }
}
