using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalLookLimit = 80.0f;

    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>(); // Assumes camera is a child

        if (playerCamera == null)
        {
            Debug.LogError("PlayerMovementController: No Camera found as a child of PlayerAvatar. Please assign one.");
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Player Movement (WASD)
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow
        float verticalInput = Input.GetAxis("Vertical");   // W/S or Up/Down arrow

        Vector3 forwardMovement = transform.forward * verticalInput;
        Vector3 rightMovement = transform.right * horizontalInput;

        // Apply movement, making it frame-rate independent
        characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation (around Y axis of PlayerAvatar)
        transform.Rotate(0f, mouseX, 0f);

        // Vertical rotation (around X axis of Camera)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        playerCamera.transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }
}
