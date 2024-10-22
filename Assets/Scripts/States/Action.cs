public class Action : State {
    public Action(AIController owner) : base(owner) { }

    public override void Enter() {
        
    }

    public override void Do() {
        throw new System.NotImplementedException();
    }

    public override void Exit(State newState) 
    {
        throw new System.NotImplementedException();
    }
}