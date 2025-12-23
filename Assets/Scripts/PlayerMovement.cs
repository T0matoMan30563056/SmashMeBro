using System.Collections;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    public bool AirJump = true;

    public bool AnimationStun = false;

    public AnimationCurve VerticalAnimation;
    public AnimationCurve HorizontalAnimation;

    private float AnimationTime;
    private float AnimationDuration;

    private float OriginalGravity;

    private float LocalDirection;

    public bool Strafe = false;

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

        OriginalGravity = rb.gravityScale;

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

        if (AnimationStun)
        {
            AnimationTime += Time.deltaTime;

            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(HorizontalAnimation.Evaluate(AnimationTime) * LocalDirection, VerticalAnimation.Evaluate(AnimationTime));
            Keyframe lastframe = HorizontalAnimation[HorizontalAnimation.length - 1];
            if (AnimationTime >= lastframe.time)
            {
                AnimationStun = false;
                rb.gravityScale = OriginalGravity;
                Strafe = false;
            }

            InputBuffer = 0f;

            if (!Strafe)
            {
                return;
            }
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

        if (isGrounded && !isInside)
        {
            AirJump = true;
        }

        Vector2 MoveValue = moveAction.ReadValue<Vector2>();

        MoveValue.x = Mathf.Round(MoveValue.x);
        MoveValue.y = Mathf.Round(MoveValue.y);


        playerAttacks.Direction = MoveValue.x;
        playerAttacks.VerticalDirection = MoveValue.y;

        playerAttacks.isGrounded = isGrounded;
        playerAttacks.isInside = isInside;

        
        rb.linearVelocity = new Vector2(Speed * MoveValue.x, rb.linearVelocity.y);


        if (Keyboard.current[Key.Space].wasPressedThisFrame && !Strafe)
        {
            if (!isGrounded && AirJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Jump);
                AirJump = false;
            }
            else
            {
                JumpBuffer = true;
                InputBuffer = BufferDuration;
            }
        }

        if (Keyboard.current[Key.LeftShift].wasPressedThisFrame && canDash && !Strafe) 
        {
            Debug.Log("ts is working :thumbs_up:");
            StartCoroutine(Dash());
        }

        if (isGrounded && !isInside && JumpBuffer && !Strafe)
        {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Jump);
            InputBuffer = 0f;
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
        yield return new WaitForSeconds(DashingTime);
        rb.gravityScale = OriginalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashingCooldown);
        canDash = true;

    }


    public void AnimationMovement(float Direction)
    {
        LocalDirection = Direction;

        AnimationTime = 0f;

        AnimationStun = true;
        
    }

}
