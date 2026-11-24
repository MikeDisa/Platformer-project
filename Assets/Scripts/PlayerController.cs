using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed = 3f;

    public float jumpForce = 5f;

    public float terminalVel = 5f;

    public float weight = 1f;
    public enum FacingDirection
    {
        left, right
    }

    FacingDirection tempDirection = FacingDirection.right;

    bool isWalking;

    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        Vector2 playerInput = new Vector2();

        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        if (isGrounded && Keyboard.current.wKey.wasPressedThisFrame || isGrounded && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            playerInput.y = 1;
        }
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            playerInput.x = -1;
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            playerInput.x = 1;
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
        if (isWalking)
        {
            return true;
        }

        return false;
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
