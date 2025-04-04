using UnityEngine;

public class Enemy_Skeleton : Entity
{
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;


    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
    }
}
