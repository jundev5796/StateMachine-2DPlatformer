using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }


    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        spriteRenderer = GetComponent<SpriteRenderer>();

        idleState = new PlayerIdleState(this, stateMachine, "ON");
        moveState = new PlayerMoveState(this, stateMachine, "OFF");
    }


    private void Start()
    {
        stateMachine.Initialize(idleState);
    }


    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
