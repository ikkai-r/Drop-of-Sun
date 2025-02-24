using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed;
    [SerializeField] public float jumpingSpeed;
    [SerializeField] public float maxJumpPressure;

    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;
    private bool isJumping;
    private bool isWalking;
    private bool isCharging;
    private float jumpPressure;
    private float minJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        grounded = true;
        isJumping = false;
        isWalking = false;
        isCharging = false;
        jumpPressure = 0f;
        minJump = jumpingSpeed;
    }

    void Update()
    {
      float horizontalInput = Input.GetAxis("Horizontal");
       Debug.Log(isJumping);

      //move

        if(horizontalInput > 0.01f || horizontalInput < -0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        // if (grounded && !isJumping && !isCharging)
        if (!isCharging)
        {
            rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
        }

      //flip
      if(grounded && !isJumping)
        {
            if (horizontalInput > 0.01f)
            {
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else if (horizontalInput < -0.01f)
            {
                transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            }
        }

        //jump
        if (grounded)
        {
            //holding jump button
            if (Input.GetKey(KeyCode.Space))
            {
                isCharging = true;
                if (jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime * 10f;
                }
                else
                {
                    jumpPressure = maxJumpPressure;
                }
            }
            else
            {
                isCharging = false;

                if (jumpPressure > 0f)
                {
                    isJumping = true;
                    grounded = false;
                    jumpPressure = jumpPressure + minJump;

                    rb.velocity = new Vector2(rb.velocity.x, jumpPressure);

                    jumpPressure = 0f;
                }
            }
        }

      //anim parameters
      anim.SetBool("isWalking", horizontalInput != 0);
      anim.SetBool("grounded", grounded);
      anim.SetBool("isJumping", isJumping);
      anim.SetBool("isCharging", isCharging);
      anim.SetBool("isWalking", isWalking);
    }

     private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector2 contactPoint = collision.GetContact(0).point;

            //below player
            if (contactPoint.y < transform.position.y)
            {
                grounded = true;
                isJumping = false;
                isCharging = false;
                isWalking = false;
            }
        }

    }
}

