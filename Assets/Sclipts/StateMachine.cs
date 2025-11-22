using UnityEngine;

public class StateMachine<T> where T : MonoBehaviour
{
    public IState<T> CurrentState { get; private set; }
    private readonly T owner;
    public StateMachine(T owner, IState<T> state)
    {
        this.owner = owner;
        CurrentState = state;
        CurrentState.OnEnter(owner);
    }

    public void ExcuteUpdate()
    {
        CurrentState.OnUpdate(owner);
    }

    public void ChangeState(IState<T> nextState)
    {
        CurrentState.OnExit(owner, nextState);
        nextState.OnEnter(owner, CurrentState);
        CurrentState = nextState;
    }
}
