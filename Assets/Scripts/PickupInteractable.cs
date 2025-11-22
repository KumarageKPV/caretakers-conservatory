using UnityEngine;

namespace CaretakersConservatory
{
    public class PickupInteractable : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float throwForceMultiplier = 6f;

        private bool _held;
        private Transform _holder;
        private Vector3 _lastVelocity;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_held && rb != null && _holder != null)
            {
                // Simple follow; for XR you would parent to attach transform (hand/controller)
                transform.position = _holder.position;
                transform.rotation = _holder.rotation;
            }
            if (rb != null)
            {
                _lastVelocity = rb.velocity;
            }
        }

        public void Pickup(Transform holder)
        {
            if (rb == null) return;
            _held = true;
            _holder = holder;
            rb.isKinematic = true;
        }

        public void Drop(bool applyThrow = true)
        {
            if (rb == null) return;
            rb.isKinematic = false;
            if (applyThrow)
            {
                rb.velocity = _holder != null ? _holder.forward * throwForceMultiplier : _lastVelocity;
            }
            _held = false;
            _holder = null;
        }
    }
}
