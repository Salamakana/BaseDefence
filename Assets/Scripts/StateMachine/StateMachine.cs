public class StateMachine
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;

        currentState.EnterState();
    }

    public void ExecuteState()
    {
        if (currentState != null)
            currentState.ExecuteState();
    }
}

public interface IState
{
    void EnterState();
    void ExecuteState();
    void ExitState();
}

