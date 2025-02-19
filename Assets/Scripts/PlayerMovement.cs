using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed;
    [SerializeField] public float jumpingSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;
    private bool isJumping;
    private bool isCharging;
    private float jumpPressure;
    private float minJump;
    private float maxJumpPressure;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        grounded = true;
        isJumping = false;
        isCharging = false;
        jumpPressure = 0f;
        minJump = jumpingSpeed;
        maxJumpPressure = 7f;
    }

    void Update()
    {
      float horizontalInput = Input.GetAxis("Horizontal");
       Debug.Log(isJumping);

      //move
      if(grounded && !isJumping)
        {
            rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
        }

      //flip
      if(horizontalInput > 0.01f) {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
      } else if (horizontalInput < -0.01f) {
        transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
      }

      //jump
      Jump();

      //anim parameters
      anim.SetBool("isWalking", horizontalInput != 0);
      anim.SetBool("grounded", grounded);
      anim.SetBool("isJumping", isJumping);
      anim.SetBool("isCharging", isCharging);
    }

    private void Jump() {

        if(grounded)
        {
            //holding jump button
            if (Input.GetKey(KeyCode.Space))
            {
                isCharging = true;
                if (jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime * 10;
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
                    rb.velocity = new Vector2(jumpPressure / 10f, jumpPressure);
                    jumpPressure = 0f;
                }
            }


            Debug.Log(jumpPressure);
        } 

    }

     private void OnCollisionEnter2D(Collision2D collision) {
      if(collision.gameObject.tag == "Ground") {
        grounded = true;
            isJumping = false;
            isCharging = false;
        }
    }
}

