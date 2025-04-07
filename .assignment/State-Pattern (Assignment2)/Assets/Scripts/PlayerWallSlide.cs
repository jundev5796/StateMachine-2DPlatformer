using UnityEngine;

public class PlayerWallSlide : PlayerState
{
    public PlayerWallSlide(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.anim.SetBool("wallDetected", true);
    }


    public override void Update()
    {
        base.Update();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.9f);

        if (!player.IsWallDetected() || player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);
    }
    
    
    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool("wallDetected", false);
    }

}
