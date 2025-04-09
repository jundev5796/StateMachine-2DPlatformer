using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }


    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
    }
}
