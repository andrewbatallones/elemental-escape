using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb;
    private Animator anim;

    public float speed = 10f;
    public float jumpForce;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float moveDirection;
    private bool isGrounded;
    private bool wasOnGround;
    private PlayerManager playerManager;


    private void Awake()
    {
        playerController = new PlayerController();

        SetControls();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();

        moveDirection = 0f;
        isGrounded = false;
        wasOnGround = false;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        CheckGrounded();

        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

        if (moveDirection > 0)
            Flip(180);
        else if (moveDirection < 0)
            Flip(0);
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
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (!wasOnGround && isGrounded)
        {
            anim.SetTrigger("landed");
        }

        wasOnGround = isGrounded;
    }

    private void Flip(int number)
    {
        // This works if no animation
        //facingRight = !facingRight;
        //Vector3 scale = transform.localScale;
        //scale.x *= -1;
        //transform.localScale = scale;

        transform.eulerAngles = new Vector3(0, number, 0);
    }

    private void Jump()
    {
        // Simple Iteration.... too smooth
        //rb.velocity = Vector2.up * jumpForce;

        if (isGrounded)
        {
            anim.SetTrigger("takeOff");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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

        playerController.Gameplay.Shoot.performed += _ => playerManager.Shoot();
    }
}
