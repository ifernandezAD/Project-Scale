using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class BlackHoleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float drag = 1f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float zoomSensitivity = 10f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;

    [Header("References")]
    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        move.action.performed += OnMoveInput;
        move.action.canceled += OnMoveInput;
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleCameraZoom();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {

        Vector3 forwardMovement = cameraTransform.forward * moveInput.y;
        Vector3 strafeMovement = cameraTransform.right * moveInput.x;


        Vector3 movementDirection = (forwardMovement + strafeMovement).normalized;


        rb.AddForce(movementDirection * moveSpeed, ForceMode.Acceleration);


        rb.linearVelocity *= 1f - (drag * Time.fixedDeltaTime);
    }

    private void HandleCameraRotation()
    {
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;


        transform.Rotate(Vector3.up * mouseX);

        cameraTransform.Rotate(Vector3.left * mouseY);
    }
    private void HandleCameraZoom()
    {
        float scrollInput = Mouse.current.scroll.ReadValue().y;

        if (virtualCamera != null)
        {
            float newFOV = virtualCamera.Lens.FieldOfView - scrollInput * zoomSensitivity;
            virtualCamera.Lens.FieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
        }
    }

    private void OnDisable()
    {
        move.action.Disable();
        move.action.performed -= OnMoveInput;
        move.action.canceled -= OnMoveInput;
    }

}
