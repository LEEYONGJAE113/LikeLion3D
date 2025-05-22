using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState { get; private set; }

    T owner;

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void ChangeState(State<T> targetState)
    {
        CurrentState?.Exit();
        CurrentState = targetState;
        CurrentState.Enter(owner);
    }

    public void Execute()
    {
        CurrentState?.Execute();
    }
}
