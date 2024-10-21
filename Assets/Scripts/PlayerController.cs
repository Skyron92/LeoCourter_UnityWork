using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private const float Gravity = -9.81f;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private Animator animator;
    [SerializeField] [Range(1, 10)] private float speed, maxHeight;
    private CharacterController _controller;
    private bool _isMoving;
    private InputAction MoveAction => moveInput.action;
    private Vector2 InputValue => MoveAction.ReadValue<Vector2>();

    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        BindAnimation();
        _controller.Move(GetMovement() * Time.deltaTime);
    }

    private void OnEnable() {
        MoveAction.Enable();
        MoveAction.started += OnMoveActionStarted;
        MoveAction.canceled += OnMoveActionCanceled;
    }

    private void OnDisable() {
        MoveAction.Disable();
        MoveAction.started -= OnMoveActionStarted;
        MoveAction.canceled -= OnMoveActionCanceled;
    }

    private Vector3 GetMovement() {
        return new Vector3(InputValue.x * speed, Gravity, InputValue.y * speed);
    }

    private void BindAnimation() {
        animator.SetFloat("X", InputValue.x);
        animator.SetFloat("Y", InputValue.y);
    }

    private void OnMoveActionStarted(InputAction.CallbackContext obj) {
        _isMoving = true;
    }

    private void OnMoveActionCanceled(InputAction.CallbackContext obj) {
        _isMoving = false;
    }
}