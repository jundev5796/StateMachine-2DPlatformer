using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;


    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("CounterSuccess", false);
    }


    public override void Update()
    {
        base.Update();

        //player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        if (player.anim.GetBool("CounterSuccess"))
            player.SetVelocity(player.facingDir * 10, 0);
        else
            player.SetZeroVelocity();

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    
                    player.anim.SetBool("CounterSuccess", true);

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
    
    
    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool("CounterSuccess", false);
    }
}
