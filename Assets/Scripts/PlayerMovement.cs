using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb;

    public float speed = 10f;
    public float jumpForce;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float moveDirection;
    private bool facingRight;
    private bool isGrounded;


    private void Awake()
    {
        playerController = new PlayerController();

        SetControls();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = 0f;
        facingRight = false;
        isGrounded = false;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

        if ((!facingRight && moveDirection > 0) || (facingRight && moveDirection < 0))
            Flip();
    }

    private void OnEnable()
    {
        playerController.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerController.Gameplay.Disable();
    }


    // Custom Functions
    // Public


    // Private
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Jump()
    {
        // Simple Iteration.... too smooth
        //rb.velocity = Vector2.up * jumpForce;

        if (isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CancelJump()
    {
        if (!isGrounded && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    private void SetControls()
    {
        playerController.Gameplay.Move.performed += _ => moveDirection = _.ReadValue<float>();
        playerController.Gameplay.Move.canceled += _ => moveDirection = _.ReadValue<float>();

        playerController.Gameplay.Jump.performed += _ => Jump();
        playerController.Gameplay.Jump.canceled += _ => CancelJump();
    }
}
