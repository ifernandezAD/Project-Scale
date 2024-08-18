using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class BlackHoleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference verticalMove;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float drag = 1f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float zoomSensitivity = 10f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        move.action.performed += OnMoveInput;
        move.action.canceled += OnMoveInput;

        verticalMove.action.Enable();
        verticalMove.action.performed += OnAscendDescendInput;
        verticalMove.action.canceled += OnAscendDescendInput;
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

    private void OnAscendDescendInput(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<float>();
    }

    private void Move()
    {
        // Movimiento horizontal basado en la dirección de la cámara (ignora la inclinación vertical de la cámara)
        Vector3 forwardMovement = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z) * moveInput.y;
        Vector3 strafeMovement = cameraTransform.right * moveInput.x;
        Vector3 movementDirection = (forwardMovement + strafeMovement).normalized;

        // Aplica la fuerza para movimiento horizontal
        rb.AddForce(movementDirection * moveSpeed, ForceMode.Acceleration);

        // Aplica la fuerza para movimiento vertical solo cuando se presiona Q o E
        if (verticalInput != 0)
        {
            rb.AddForce(Vector3.up * verticalInput * moveSpeed, ForceMode.Acceleration);
        }

        // Aplica la resistencia (drag)
        rb.linearVelocity *= 1f - (drag * Time.fixedDeltaTime);
    }

    private void HandleCameraRotation()
    {
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        // Rotar el objeto (agujero negro) solo en el eje Y (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Inclina la cámara hacia arriba o hacia abajo
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

        verticalMove.action.Disable();
        verticalMove.action.performed -= OnAscendDescendInput;
        verticalMove.action.canceled -= OnAscendDescendInput;
    }
}
