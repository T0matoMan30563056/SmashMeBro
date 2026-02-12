using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class PlayerMovement : NetworkBehaviour
{

    [Header("Movement and Components")]
    private Rigidbody2D rb;
    [SerializeField] private float Speed;
    private PlayerAttacks playerAttacks;
    private float LocalDirection;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private Transform SpriteTransform;
    private PlayerHealth playerHealth;
    private Vector2 MoveValue = Vector2.zero;



    [Header("Position Checks")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform InsideCheck;
    [SerializeField] Transform RightCheck;
    [SerializeField] Transform LeftCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask wall;
    bool isGrounded;
    bool isInside;
    int isTouchingWall = 0;



    [Header("Jumping")]
    [SerializeField] private float Jump;
    float InputBuffer = 0f;
    bool JumpBuffer = false;
    [SerializeField] float BufferDuration;
    public bool Jumped = false;
    private int JumpHoldTester = 0;
    public bool AirJump = true;
    [SerializeField] private float WallVerticalVelocity;



    [Header("Dashing")]
    public bool isDashing;
    bool canDash = true;
    [SerializeField] private float DashPower;
    [SerializeField] private float DashingTime;
    [SerializeField] private float DashingCooldown;

    private bool DashStart;


    [Header("Attack Animations and Stunns")]

    private bool isStunned = false;
    public bool AnimationStun = false;
    public AnimationCurve VerticalAnimation;
    public AnimationCurve HorizontalAnimation;
    private float AnimationTime;
    private float AnimationDuration;
    public bool Strafe = false;


    [Header("Momentum")]
    public float ExtraVertcalMomentum;
    private float CurrentVerticalMomentum = 0;
    public float MomentumTime = 1;
    public float MomentumLerpMultiplier = 1;


    //Start Variables
    private Vector3 StartScale;
    private float OriginalGravity;

    public GameObject OwnerObject;


    //Vector2 MoveValue = moveAction.ReadValue<Vector2>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttacks = GetComponent<PlayerAttacks>();
        StartScale = SpriteTransform.localScale;


        if (GetComponent<PlayerHealth>())
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        OriginalGravity = rb.gravityScale;

    }


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            GetComponent<PlayerInput>().enabled = false;
            return;
        }
        else
        {
            OwnerObject = gameObject;
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        MoveValue = context.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        Jumped = context.action.triggered;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        DashStart = context.action.triggered;
    }


    // Update is called once per frame
    void Update()
    {

        if (Jumped)
        {
            JumpHoldTester += 1;
            if (JumpHoldTester == 2)
            {
                Jumped = false;
            } 
        }
        else if (!Jumped)
        {
            JumpHoldTester = 0;
        }

        CurrentVerticalMomentum = Mathf.Lerp(ExtraVertcalMomentum, 0, LerpT(MomentumTime));
        MomentumTime += Time.deltaTime * MomentumLerpMultiplier;


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

        if (Physics2D.OverlapCircle(RightCheck.position, 0.1f, wall))
        {
            isTouchingWall = 1;

        }
        else if (Physics2D.OverlapCircle(LeftCheck.position, 0.1f, wall))
        {
            isTouchingWall = -1;
        }
        else
        {
            isTouchingWall = 0;
        }


        if (isGrounded && !isInside)
        {
            AirJump = true;
        }


        MoveValue.x = Mathf.Round(MoveValue.x);
        MoveValue.y = Mathf.Round(MoveValue.y);

        if (Mathf.Abs(MoveValue.x) == 1f)
        {
            SpriteTransform.localScale = new Vector3(StartScale.x * MoveValue.x, StartScale.y, StartScale.z);
            PlayerAnimator.SetBool("Running", true);
        }
        else
        {
            PlayerAnimator.SetBool("Running", false);
        }

        playerAttacks.Direction = MoveValue.x;
        playerAttacks.VerticalDirection = MoveValue.y;

        playerAttacks.isGrounded = isGrounded;
        playerAttacks.isInside = isInside;

        
        //rb.linearVelocity = new Vector2(Speed * MoveValue.x + CurrentVerticalMomentum, rb.linearVelocity.y);
        //This is a potential fix
        rb.linearVelocity = new Vector2(Mathf.Lerp(CurrentVerticalMomentum, Speed * MoveValue.x, LerpT(MomentumTime)), rb.linearVelocity.y);


        if (Jumped && !Strafe)
        {
            
            if (!isGrounded && AirJump && isTouchingWall == 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Jump);
                AirJump = false;
            }
            else if(!isGrounded && isTouchingWall != 0)
            {
                ExtraVertcalMomentum = WallVerticalVelocity * -isTouchingWall;
                MomentumTime = 0;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x + ExtraVertcalMomentum, Jump);
            }
            else
            {
                JumpBuffer = true;
                InputBuffer = BufferDuration;
            }
            


        }

        if (DashStart && canDash && !Strafe) 
        {
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


    float LerpT(float t)
    {
        return Mathf.Pow(t, 3);
    }

}
