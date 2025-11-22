using UnityEngine;

namespace CaretakersConservatory
{
    public class DestructiblePot : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float breakImpulseThreshold = 8f;
        [SerializeField] private GameObject fragmentsPrefab;
        [SerializeField] private ParticleSystem breakParticles;
        [SerializeField] private AudioSource breakAudio;
        [SerializeField] private bool destroyOriginalOnBreak = true;

        private bool _broken;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_broken || rb == null) return;
            float impulse = collision.impulse.magnitude;
            if (impulse >= breakImpulseThreshold)
            {
                Break();
            }
        }

        private void Break()
        {
            _broken = true;
            if (fragmentsPrefab != null)
            {
                Instantiate(fragmentsPrefab, transform.position, transform.rotation);
            }
            if (breakParticles != null)
            {
                breakParticles.transform.position = transform.position;
                breakParticles.Play();
            }
            if (breakAudio != null)
            {
                breakAudio.transform.position = transform.position;
                breakAudio.Play();
            }
            if (destroyOriginalOnBreak)
            {
                Destroy(gameObject);
            }
        }
    }
}
