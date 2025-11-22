using UnityEngine;

namespace CaretakersConservatory
{
    public class WindController : MonoBehaviour
    {
        [SerializeField] private WorldParams worldParams;
        [SerializeField] private string shaderWindStrengthProperty = "_GlobalWindStrength";
        [SerializeField] private float shaderScale = 1f;

        private void OnEnable()
        {
            if (worldParams != null)
            {
                worldParams.WindStrengthChanged += OnWindChanged;
                OnWindChanged(worldParams.WindStrength);
            }
        }

        private void OnDisable()
        {
            if (worldParams != null)
            {
                worldParams.WindStrengthChanged -= OnWindChanged;
            }
        }

        private void OnWindChanged(float strength)
        {
            Shader.SetGlobalFloat(shaderWindStrengthProperty, strength * shaderScale);
        }
    }
}
