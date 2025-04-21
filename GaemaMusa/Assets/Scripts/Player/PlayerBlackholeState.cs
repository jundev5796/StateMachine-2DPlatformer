using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flytime = 0.4f;
    private bool skillUsed;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {

    }


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }


    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        stateTimer = flytime;
        rb.gravityScale = 0;
    }


    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 15);

        if (stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                Debug.Log("Cast Blackhole");
                skillUsed = true;
            }
        }
    }


    public override void Exit()
    {
        base.Exit();
    }
}
