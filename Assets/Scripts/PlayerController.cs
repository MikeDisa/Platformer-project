using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;

    [Header("Horizontal")]
    public float moveSpeed = 3f;
    public float accelerationTime = 0.25f;
    public float decelerationTime = 0.15f;

    [Header("Vertical")]
    public float apexHeight = 3f;
    public float apexTime = 0.5f;

    [Header("Ground Checking")]
    public float groundCheckOffset = 0.65f;
    public Vector2 groundCheckSize = new(0.5f, 0.1f);
    public LayerMask groundCheckMask;

    private float acceleration;
    private float deceleration;
    private Vector2 velocity;


    float gravity;
    float jumpVelocity;
    float terminalVel;


    public enum FacingDirection
    {
        left, right
    }

    FacingDirection facingDirection = FacingDirection.right;

    bool isWalking;
    bool isGrounded;
    bool isJumping;

    void Start()
    {
        acceleration = moveSpeed / accelerationTime;
        deceleration = moveSpeed / decelerationTime;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        gravity = -2 * apexHeight / Mathf.Pow(apexTime, 2);
        jumpVelocity = 2 * apexHeight / apexTime;
    }

    void Update()
    {
        
        Vector2 playerInput = new Vector2();

        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        if (isGrounded && Keyboard.current.wKey.wasPressedThisFrame || isGrounded && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            isJumping = true;
        }
        
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            playerInput.x += -1;
            tempDirection = FacingDirection.left;
        }
        
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            playerInput.x += 1;
            tempDirection = FacingDirection.right;
        }
       

        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        rb.linearVelocity = new Vector2(playerInput.x * moveSpeed,rb.linearVelocity.y);

        isWalking = playerInput.x != 0;

        if (playerInput.y > 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (playerInput.x < 0)
        {
            tempDirection = FacingDirection.left;
        }

        if (playerInput.x > 0)
        {
            tempDirection = FacingDirection.right;
        }

        //falling physiscs
        
        if (rb.linearVelocity.y < (terminalVel * -1))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, terminalVel * -1);
        }
        
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = weight;
        }
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, terminalVel, velocity.y);
        }
        else
        {
            velocity.y = 0;
        }
        
        if (isJumping)
        {
            isJumping = false;
            velocity.y = jumpVelocity;
        } 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.gravityScale = 1;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            rb.gravityScale = 1;
        }
    }

    public bool IsWalking()
    { 

        return playerInput.x =!0;
    }
    public bool IsGrounded()
    {
        if (isGrounded)
        {
            return true;
        }

        return false;
    }

    public FacingDirection GetFacingDirection()
    {

        if (tempDirection == FacingDirection.right)
        {
            return FacingDirection.right;
        }

        return FacingDirection.left;
    }
}
