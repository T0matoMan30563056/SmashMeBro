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


    private PlayerInput playerInput;

    private InputAction moveAction;

    float InputBuffer = 0f;
    bool JumpBuffer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerAttacks = GetComponent<PlayerAttacks>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, ground);

        isInside = Physics2D.OverlapCircle(InsideCheck.position, 0.5f, ground);

        Vector2 MoveValue = moveAction.ReadValue<Vector2>();

        if(Mathf.Abs(MoveValue.x) == 1)
        {
            playerAttacks.Direction = MoveValue.x;
        }

        rb.linearVelocity = new Vector2(Speed * MoveValue.x, rb.linearVelocity.y);


        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {

            JumpBuffer = true;
            InputBuffer = BufferDuration;
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
}
