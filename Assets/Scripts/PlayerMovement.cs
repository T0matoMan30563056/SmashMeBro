using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] private float Speed;
    [SerializeField] private float Jump;
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform InsideCheck;
    [SerializeField] float BufferDuration;


    private PlayerAttacks playerAttacks;
    
    bool isGrounded;
    bool isInside;
    
    // Dashing
    public bool isDashing;
    bool canDash = true;
    [SerializeField] private float DashPower;
    [SerializeField] private float DashingTime;
    [SerializeField] private float DashingCooldown;

    [SerializeField] private TrailRenderer tr;

    private PlayerInput playerInput;
    private InputAction moveAction;

    float InputBuffer = 0f;
    bool JumpBuffer = false;

    private bool isStunned = false;
    private PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttacks = GetComponent<PlayerAttacks>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];


        if (GetComponent<PlayerHealth>())
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

    }

    // Update is called once per frame
    void Update()
    {   
        if (playerHealth != null)
        {
            isStunned = playerHealth.Stunned;
        }
        if (isStunned)
        {
            return;
        }
        if (isDashing)
        {

            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(playerAttacks.Direction * DashPower, 0f);

            InputBuffer = 0f;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, ground);
        isInside = Physics2D.OverlapCircle(InsideCheck.position, 0.5f, ground);
        Vector2 MoveValue = moveAction.ReadValue<Vector2>();

        MoveValue.x = Mathf.Round(MoveValue.x);
        MoveValue.y = Mathf.Round(MoveValue.y);


        if (Mathf.Abs(MoveValue.x) == 1)
        {
            playerAttacks.Direction = MoveValue.x;
        }
        playerAttacks.VerticalDirection = MoveValue.y;

        playerAttacks.isGrounded = isGrounded;
        playerAttacks.isInside = isInside;


        rb.linearVelocity = new Vector2(Speed * MoveValue.x, rb.linearVelocity.y);


        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {

            JumpBuffer = true;
            InputBuffer = BufferDuration;
        }

        if (Keyboard.current[Key.LeftShift].wasPressedThisFrame && canDash) 
        {
            Debug.Log("ts is working :thumbs_up:");
            StartCoroutine(Dash());
        }

        if (isGrounded && !isInside && JumpBuffer)
        {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Jump);
        }


        InputBuffer -= Time.deltaTime;
        InputBuffer = Mathf.Clamp(InputBuffer, 0f, BufferDuration);

        if (InputBuffer == 0)
        {
            JumpBuffer = false;
        }


    }

    private IEnumerator Dash()
    {
        canDash= false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        
        //tr.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashingCooldown);
        canDash = true;

    }
}
