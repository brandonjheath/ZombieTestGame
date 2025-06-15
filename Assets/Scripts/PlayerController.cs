// PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private Vector2 _moveInput;
    private bool _jumpPressed;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Send Messages callbacks
    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
            _jumpPressed = true;
    }

    void Update()
    {
        // --- vertical (gravity + jump) ---
        if (_controller.isGrounded && _velocity.y < 0f)
            _velocity.y = -0.1f;

        if (_jumpPressed && _controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _jumpPressed = false;
        }

        _velocity.y += gravity * Time.deltaTime;

        // --- horizontal movement ---
        Vector3 raw = new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 horizontal = transform.TransformDirection(raw) * moveSpeed;

        // --- apply move ---
        Vector3 motion = horizontal + Vector3.up * _velocity.y;
        _controller.Move(motion * Time.deltaTime);
    }
}
