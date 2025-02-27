using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;
    public float jumpForce = 7f;

    [Header("Gravedad y Suelo")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    [Header("Cámara")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isGrounded;
    private float xRotation = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor
    }

    void Update()
    {
        CheckGround();
        LookAround();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    // ?? Verifica si el personaje está en el suelo
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    // ????? Movimiento del jugador
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    /*private void MovePlayer()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * speed;
        Vector3 velocity = rb.velocity;
        rb.velocity = new Vector3(move.x, velocity.y, move.z);
    }           */
    private void MovePlayer()
    {
        // Tomamos la dirección en base a la rotación del jugador
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Aplicamos el movimiento en esa dirección
        Vector3 velocity = rb.velocity;
        rb.velocity = new Vector3(moveDirection.x * speed, velocity.y, moveDirection.z * speed);
    }

    // ?? Manejo del salto
    private void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    // ?? Movimiento de la cámara con el mouse
    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void LookAround()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // Rotación horizontal del jugador
        transform.Rotate(Vector3.up * mouseX);

        // Rotación vertical de la cámara (limitada)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
