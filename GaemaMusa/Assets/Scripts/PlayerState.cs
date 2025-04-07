using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected float xInput;
    private string animBoolName;


    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }


    public virtual void Enter()
    {
        Debug.Log("Enter " + animBoolName);
    }


    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }


    public virtual void Exit()
    {
        Debug.Log("Exit " + animBoolName);
    }
}
