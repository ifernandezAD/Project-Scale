using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using DG.Tweening;

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
    [SerializeField] private float maxFOV = 100f;
    [SerializeField] private float zoomDuration = 0.5f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private float verticalInput;
    private float targetFOV;


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

        BlackHoleBehaviour.onRadiusThreshold += UpdateCameraZ;
    }

    void Start()
    {
        targetFOV = virtualCamera.Lens.FieldOfView;
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
        Vector3 forwardMovement = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z) * moveInput.y;
        Vector3 strafeMovement = cameraTransform.right * moveInput.x;
        Vector3 movementDirection = (forwardMovement + strafeMovement).normalized;

        rb.AddForce(movementDirection * moveSpeed, ForceMode.Acceleration);


        if (verticalInput != 0)
        {
            rb.AddForce(Vector3.up * verticalInput * moveSpeed, ForceMode.Acceleration);
        }

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
            float newFOV = Mathf.Clamp(targetFOV - scrollInput * zoomSensitivity, minFOV, maxFOV);

            if (Mathf.Abs(newFOV - virtualCamera.Lens.FieldOfView) > Mathf.Epsilon)
            {
                DOTween.To(() => virtualCamera.Lens.FieldOfView, x => virtualCamera.Lens.FieldOfView = x, newFOV, zoomDuration);
                targetFOV = newFOV;
            }
        }
    }

    void UpdateCameraZ()
    {
        if (virtualCamera != null)
        {
            Vector3 currentCameraPosition = virtualCamera.transform.position;
            currentCameraPosition.z -= 5f;
            virtualCamera.transform.position = currentCameraPosition;

            //virtualCamera.transform.DOMoveZ(currentCameraPosition.z - 5f, 0.5f); 
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

        BlackHoleBehaviour.onRadiusThreshold -= UpdateCameraZ;
    }
}
