using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
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

        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //    stateMachine.ChangeState(player.dashState);

        if (player.IsEnemyDetected())
        {
            player.CheckForDashInput();
            if (player.IsEnemyDetected().distance < player.attackDistance)
            {

                stateMachine.ChangeState(player.primaryAttackState);
            }
            else
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
        else if (player.IsEnemyDetectedLeft())
        {
            player.CheckForDashInput();
            if (player.IsEnemyDetectedLeft().distance < player.attackDistance)
            {

                stateMachine.ChangeState(player.primaryAttackState);
            }
            else
            {
                stateMachine.ChangeState(player.moveState);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
            stateMachine.ChangeState(player.blackHoleState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);

        if (Input.GetMouseButtonDown(1) && HasNoSword())
            stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetKeyDown(KeyCode.F))
            stateMachine.ChangeState(player.counterAttackState);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }
    
    
    public override void Exit()
    {
        base.Exit();
    }


    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
