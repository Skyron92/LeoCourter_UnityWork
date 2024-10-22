using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private const float Gravity = -9.81f;
    [SerializeField] private InputActionReference moveInput, jumpInput, sprintInput;
    [SerializeField] private Animator animator;
    [SerializeField] [Range(1, 10)] private float speed;
    [SerializeField] [Range(1, 10)] private float maxHeight = 1;
    [SerializeField] [Range(0, 2)] private float jumpForce;
    private CharacterController _controller;
    private int _yDirection;
    private bool _mayRoll;
    Vector3 _playerVelocity;
    bool IsGrounded => _controller.isGrounded;
    private InputAction MoveAction => moveInput.action;
    private InputAction JumpAction => jumpInput.action;
    private InputAction SprintAction => sprintInput.action;
    float SprintValue => SprintAction.ReadValue<float>();
    private Vector2 MoveInputValue => MoveAction.ReadValue<Vector2>();
    private float JumpInputValue => JumpAction.ReadValue<float>();

    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        BindAnimation();
        ApplyMovement();
        if(_yDirection < 0) Land();
    }

    private void OnEnable() {
        MoveAction.Enable();
        JumpAction.Enable();
        JumpAction.started += context => {
            if (IsGrounded) {
                _yDirection = 1;
                animator.SetTrigger("Jump");
            }
            else {
                _yDirection = -1;
                _mayRoll = true;
            }
        };
        SprintAction.Enable();
    }

    private void OnDisable() {
        MoveAction.Disable();
        JumpAction.Disable();
        SprintAction.Disable();
    }

    private void ApplyMovement() {
        Vector3 movement = GetMovement();
        
        movement.y += Mathf.Sqrt(maxHeight * -1.0f * Gravity) * JumpInputValue;
        movement.y += Gravity * Time.deltaTime;
        _controller.Move(movement * Time.deltaTime);

    }

    private Vector3 GetMovement() {
        return new Vector3(MoveInputValue.x * speed, _yDirection > .5 ? jumpForce : Gravity, MoveInputValue.y * speed);
    }

    private void BindAnimation() {
        animator.SetFloat("X", MoveInputValue.x);
        animator.SetFloat("Y", MoveInputValue.y > 0 ? MoveInputValue.y * .5f + (MoveInputValue.y * SprintValue) * .5f : MoveInputValue.y);
    }

    private void Land() {
        animator.SetTrigger(_mayRoll ? "Roll" : "Land");
        _mayRoll = false;
        _yDirection = 0;
    }
}