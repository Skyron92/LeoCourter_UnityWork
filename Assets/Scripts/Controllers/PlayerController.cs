using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private const float Gravity = -9.81f;
    [SerializeField] private InputActionReference moveInput, jumpInput, sprintInput;
    [SerializeField] private Animator animator;
    [SerializeField] [Range(1, 10)] private float speed;
    [SerializeField] [Range(1, 10)] private float maxHeight = 1;
    [SerializeField] [Range(0, 10)] private float jumpForce;
    [SerializeField] [Range(0, 10)] private float gravityScale;
    [SerializeField] private LayerMask groundLayer;
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
        StartCoroutine(ApplyMovement());
    }

    private void Update() {
        if(_yDirection < 0) Land();
    }

    private void OnEnable() {
        MoveAction.Enable();
        JumpAction.Enable();
        JumpAction.started += context => {
            if (IsGrounded) {
                animator.SetTrigger("Jump");
            }
            /*else {
                _yDirection = -1;
                _mayRoll = true;
            }*/
        };
        SprintAction.Enable();
    }

    private void OnDisable() {
        MoveAction.Disable();
        JumpAction.Disable();
        SprintAction.Disable();
    }

    private IEnumerator ApplyMovement() {
        while (true) {
            BindAnimation();
            Vector3 movement = GetMovement();
            if (!Physics.Raycast(transform.position, Vector3.down, maxHeight, groundLayer)) _yDirection = -1;
            movement.y += Mathf.Sqrt(maxHeight * -1.0f * Gravity) * jumpForce;
            movement.y += Gravity * Time.deltaTime * gravityScale;
            _controller.Move(movement * Time.deltaTime); 
            yield return null;
        }
    }

    private Vector3 GetMovement() {
        
        return new Vector3(MoveInputValue.x * speed, _yDirection > .5 ? 1 : Gravity, GetForwardMovement(1) * speed);
    }

    private void BindAnimation() {
        animator.SetFloat("X", MoveInputValue.x);
        animator.SetFloat("Y", GetForwardMovement(.5f));
    }

    private void Land() {
        animator.SetTrigger(_mayRoll ? "Roll" : "Land");
        _mayRoll = false;
        _yDirection = 0;
    }

    /// <summary>
    /// Return <see cref="walkValue"/> if W is pressed, 2 * walkValue if shift + W are pressed
    /// 0 in others cases
    /// </summary>
    /// <param name="walkValue">The value desired as normal walk.</param>
    /// <returns></returns>
    private float GetForwardMovement(float walkValue) {
        return MoveInputValue.y > 0
            ? MoveInputValue.y * walkValue + MoveInputValue.y * SprintValue * walkValue
            : MoveInputValue.y;
    }

    public void Jump(int direction) {
        _yDirection = direction;
    }
}