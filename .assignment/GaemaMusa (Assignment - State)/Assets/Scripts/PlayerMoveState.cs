using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();
        player.spriteRenderer.color = Color.red;
    }


    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.N))
            player.stateMachine.ChangeState(player.idleState);
    }


    public override void Exit()
    {
        base.Exit();
    }
}
