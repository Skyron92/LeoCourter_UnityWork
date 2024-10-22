using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AIController : MonoBehaviour
{
    NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;
    public GameObject target;
    [SerializeField] private InputActionReference moveInput;
    private InputAction MoveAction => moveInput.action;
    private bool _isMoving;
    public bool IsMoving => _isMoving;
    public bool HasReachedDestination => _agent.remainingDistance <= _agent.stoppingDistance;
    [HideInInspector] public State currentState;
     public Vector3 Direction => target.transform.position - transform.position;
     private Animator _animator;
     public Animator Animator => _animator;

    void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>(); 
        currentState = new Wait(this);
        MoveAction.Enable();
        MoveAction.started += OnMove;
    }

    private void OnMove(InputAction.CallbackContext obj) {
        _isMoving = true;
    }

    private void Start() {
        currentState.Enter();
    }

    private void Update() {
        currentState.Do();
    }
}
