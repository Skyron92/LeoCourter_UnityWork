using System;

public class Move : State {
    public Move(AIController owner) : base(owner) { }

    public override void Enter() {
        Owner.Animator.SetFloat("X", Owner.Direction.x);
        Owner.Animator.SetFloat("Y", Owner.Direction.z);
    }

    public override void Do() {
        Owner.Agent.SetDestination(Owner.target.transform.position);
        if(Owner.HasReachedDestination) Exit(new Wait(Owner));
    }

    public override void Exit(State newState) {
        Owner.currentState = newState;
        Owner.currentState.Enter();

    }
}