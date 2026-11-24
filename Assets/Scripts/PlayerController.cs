using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed = 3f;

    public float jumpForce = 5f;
    public enum FacingDirection
    {
        left, right
    }

    FacingDirection tempDirection = FacingDirection.right;

    bool isWalking;

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
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
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
        return true;
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
