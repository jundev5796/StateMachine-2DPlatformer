using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;

    private float xInput;

    private int facingDir = 1;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        CheckInput();
        Movement();

        FlipController();
        AnimatorControllers();
    }


    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }


    private void Movement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }


    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }


    private void FlipController()
    {
        if (rb.linearVelocityX > 0 && !facingRight || rb.linearVelocityX < 0 && facingRight)
            Flip();
    }


    private void AnimatorControllers()
    {
        //bool isMoving = rb.linearVelocityX != 0;

        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetBool("isMoving", rb.linearVelocityX != 0);
    }
}
