using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private const float Gravity = -9.81f;
    [SerializeField] private InputActionReference moveInput, jumpInput;
    [SerializeField] private Animator animator;
    [SerializeField] [Range(1, 10)] private float speed, maxHeight;
    private CharacterController _controller;
    private InputAction MoveAction => moveInput.action;
    private InputAction JumpAction => jumpInput.action;
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
        JumpAction.Enable();
        JumpAction.started += context => animator.SetTrigger("Jump");
    }

    private void OnDisable() {
        MoveAction.Disable();
        JumpAction.Disable();
    }

    private Vector3 GetMovement() {
        return new Vector3(InputValue.x * speed, Gravity, InputValue.y * speed);
    }

    private void BindAnimation() {
        animator.SetFloat("X", InputValue.x);
        animator.SetFloat("Y", InputValue.y);
    }
}