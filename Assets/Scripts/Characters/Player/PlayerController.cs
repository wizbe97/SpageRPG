using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    UpdateAnimationState animationState;

    public float movementSpeed = 3.0f;
    [HideInInspector] public Vector2 moveInput = Vector2.zero;
    private Rigidbody2D rb;
    private Vector2 lastMoveDirection = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationState = GetComponent<UpdateAnimationState>();
    }

    void FixedUpdate()
    {
        if (!animationState.stateLock)
        {
            rb.velocity = moveInput * movementSpeed;
            if (moveInput != Vector2.zero)
            {
                lastMoveDirection = moveInput.normalized;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput != Vector2.zero)
        {
            if (movementSpeed <= 3.01f)
            {
                animationState.currentState = UpdateAnimationState.PlayerStates.WALK;
            }
            else
            {
                animationState.currentState = UpdateAnimationState.PlayerStates.RUN;
            }
            PlayerFaceMovementDirection();
        }
        else
        {
            animationState.currentState = UpdateAnimationState.PlayerStates.IDLE;
            PlayerFaceLastMovementDirection();
        }
    }

    void PlayerFaceMovementDirection()
    {
        animationState.animator.SetFloat("xMove", moveInput.x);
        animationState.animator.SetFloat("yMove", moveInput.y);
    }

    void PlayerFaceLastMovementDirection()
    {
        animationState.animator.SetFloat("xMove", lastMoveDirection.x);
        animationState.animator.SetFloat("yMove", lastMoveDirection.y);
    }

    void PlayerFollowMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerToMouse = (mousePosition - (Vector2)transform.position).normalized;
        animationState.animator.SetFloat("mouseX", playerToMouse.x);
        animationState.animator.SetFloat("mouseY", playerToMouse.y);
    }
}

