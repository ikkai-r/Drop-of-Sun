using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed;
    [SerializeField] public float jumpingSpeed;
    [SerializeField] public float maxJumpPressure;
    [SerializeField] public AudioClip walkingClip;

    [SerializeField] public InputActionReference moveActionToUse;

    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;
    private bool isJumping;
    private bool isWalking;
    private bool isCharging;
    private float jumpPressure;
    private float minJump;
    private AudioSource audioSource;

    private Vector2 moveDirection; // Stores movement input

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        grounded = true;
        isJumping = false;
        isWalking = false;
        isCharging = false;
        jumpPressure = 0f;
        minJump = jumpingSpeed;
    }

    private void Update()
    {
        // Read joystick input
        moveDirection = moveActionToUse.action.ReadValue<Vector2>();

        // Check if moving
        isWalking = moveDirection.x != 0;

        // Play walking sound when moving on ground
        if (isWalking && grounded && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkingClip);
        }

        // Flip character based on movement
        if (grounded && !isJumping)
        {
            if (moveDirection.x > 0.01f)
            {
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else if (moveDirection.x < -0.01f)
            {
                transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            }
        }

        // Jumping logic (Hold joystick down to charge)
        if (grounded)
        {
            if (moveDirection.y < -0.5f) // Joystick pushed down
            {
                isCharging = true;
                jumpPressure = Mathf.Min(jumpPressure + Time.deltaTime * 10f, maxJumpPressure);
            }
            else if (isCharging && moveDirection.y >= 0) // Release joystick to jump
            {
                Jump();
            }
        }

        // Update animator parameters
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("grounded", grounded);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isCharging", isCharging);
    }

    private void FixedUpdate()
    {
        // Apply movement in FixedUpdate
        rb.velocity = new Vector2(moveDirection.x * movementSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        isJumping = true;
        grounded = false;
        isCharging = false;

        float jumpForce = jumpPressure + minJump; // Use accumulated jumpPressure
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        jumpPressure = 0f; // Reset jumpPressure after applying force
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            if (contactPoint.y < transform.position.y)
            {
                grounded = true;
                isJumping = false;
                isCharging = false;
            }
        }
    }
}
