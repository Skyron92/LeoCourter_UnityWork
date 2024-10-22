using System;
using UnityEngine;

public class Wait : State {
    public Wait(AIController owner) : base(owner) { }

    public override void Enter() {
        Owner.Animator.SetFloat("X", 0);
        Owner.Animator.SetFloat("Y", 0);
    }

    public override void Do() {
        if (Owner.IsMoving) {
            Exit(new Move(Owner));
        }
    }

    public override void Exit(State newState) {
        Owner.currentState = newState;
        Owner.currentState.Enter();
    }
}