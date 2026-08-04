using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState
    {
        get;
        private set;
    }

    public void Initialize(EnemyState startState)
    {
        CurrentState = startState;

        CurrentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState?.Exit();

        CurrentState = newState;

        Debug.Log("Enemy State : " +
                  CurrentState.GetType().Name);

        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}