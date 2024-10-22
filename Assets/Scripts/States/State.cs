using UnityEngine;

public abstract class State {
    protected AIController Owner;

    protected State(AIController owner) {
        Owner = owner;
    }

    public abstract void Enter();
    public abstract void Do();
    public abstract void Exit(State newState);
}