using UnityEngine;

namespace CaretakersConservatory
{
    public class FanController : MonoBehaviour
    {
        [SerializeField] private WorldParams worldParams;
        [SerializeField] private Transform fanBlades;
        [SerializeField] private Vector3 rotationAxis = Vector3.forward; // Assume model oriented so Z forward

        private float currentSpeed;

        private void OnEnable()
        {
            if (worldParams != null)
            {
                worldParams.FanSpeedChanged += OnFanSpeedChanged;
                currentSpeed = worldParams.FanSpeed;
            }
        }

        private void OnDisable()
        {
            if (worldParams != null)
            {
                worldParams.FanSpeedChanged -= OnFanSpeedChanged;
            }
        }

        private void Update()
        {
            if (fanBlades != null && currentSpeed > 0f)
            {
                fanBlades.Rotate(rotationAxis, currentSpeed * Time.deltaTime, Space.Self);
            }
        }

        private void OnFanSpeedChanged(float newSpeed)
        {
            currentSpeed = newSpeed;
        }
    }
}
