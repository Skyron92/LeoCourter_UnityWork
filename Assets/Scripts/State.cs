using UnityEngine;

public abstract class State : MonoBehaviour {
    protected abstract void Enter();
    protected abstract void Do();
    protected abstract void Exit();
}