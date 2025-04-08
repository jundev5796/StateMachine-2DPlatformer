using UnityEngine;

public class PlayerWallJump : PlayerState
{
    public PlayerWallJump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();
    }


    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(player.airState);
    }


    public override void Exit()
    {
        base.Exit();
    }

}
