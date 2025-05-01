using JetBrains.Annotations;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Detail")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;


    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lasttimeAttacked;
    [SerializeField] protected LayerMask whatIsEnemy;


    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }


    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }


    #region States
    public PlayerStateMachine stateMachine { get; private set; }


    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackHoleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }


    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.H))
        {
            skill.crystal.CanUseSkill();
        }
    }


    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }


    public void ClearTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }


    public void ExitBlackHoleAbility()
    {
        stateMachine.ChangeState(airState);
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;


        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();


    public virtual RaycastHit2D IsEnemyDetected() => Physics2D.Raycast(wallCheck.position,
     Vector2.right * facingDir, 50, whatIsEnemy);

    public virtual RaycastHit2D IsEnemyDetectedLeft() => Physics2D.Raycast(wallCheck.position,
        Vector2.right * -facingDir, 50, whatIsEnemy);


    public void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashUsageTimer -= Time.deltaTime;

            if (dashUsageTimer < 0)
            {
                dashUsageTimer = dashCooldown;

                dashDir = Input.GetAxisRaw("Horizontal");

                if (dashDir == 0)
                    dashDir = facingDir;


                stateMachine.ChangeState(dashState);
            }

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }


    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
