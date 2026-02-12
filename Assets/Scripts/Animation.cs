using UnityEngine;

public class Animation : MonoBehaviour 
{
    private Animator animator;
    private Rigidbody rb;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yValocity", rb.linearVelocity.y);
        animator.SetBool("isJumping", GetComponent<PlayerMovement>().Jumped);
        
    }

}
