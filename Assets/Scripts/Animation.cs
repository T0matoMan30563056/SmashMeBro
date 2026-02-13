using System;
using UnityEngine;

public class Animation : MonoBehaviour 
{
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement PM;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PM = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isJumping", PM.GroundJump);

        


        if (PM.isGrounded && !PM.isInside)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }


        

    }


}
