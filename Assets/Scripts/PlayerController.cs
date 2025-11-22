using UnityEngine;
using UnityEngine.InputSystem;

namespace CaretakersConservatory
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintMultiplier = 1.5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Look")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float lookSensitivity = 2f;
        [SerializeField] private float maxLookAngle = 80f;

        [Header("Interaction")]
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactableLayers = ~0;

        private CharacterController _controller;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _sprintInput;
        private float _verticalVelocity;
        private float _cameraPitch;
        private PickupInteractable _heldObject;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main?.transform;
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
        }

        private void HandleMovement()
        {
            // Movement direction relative to player facing
            Vector3 moveDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
            float speed = _sprintInput ? moveSpeed * sprintMultiplier : moveSpeed;
            
            // Apply gravity
            if (_controller.isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = -2f; // small downward force to keep grounded
            }
            else
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }

            Vector3 velocity = moveDirection * speed + Vector3.up * _verticalVelocity;
            _controller.Move(velocity * Time.deltaTime);
        }

        private void HandleLook()
        {
            if (_lookInput.sqrMagnitude < 0.01f) return;

            // Horizontal rotation (yaw)
            transform.Rotate(Vector3.up, _lookInput.x * lookSensitivity);

            // Vertical rotation (pitch)
            _cameraPitch -= _lookInput.y * lookSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -maxLookAngle, maxLookAngle);

            if (cameraTransform != null)
            {
                cameraTransform.localEulerAngles = new Vector3(_cameraPitch, 0f, 0f);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _sprintInput = context.ReadValueAsButton();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (_heldObject != null)
            {
                // Drop/throw held object
                _heldObject.Drop(applyThrow: true);
                _heldObject = null;
            }
            else
            {
                // Raycast to pick up object
                Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayers))
                {
                    PickupInteractable pickup = hit.collider.GetComponent<PickupInteractable>();
                    if (pickup != null)
                    {
                        _heldObject = pickup;
                        // Create a hold position slightly in front of camera
                        Transform holdPoint = cameraTransform;
                        pickup.Pickup(holdPoint);
                    }
                }
            }
        }
    }
}
