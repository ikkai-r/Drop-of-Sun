using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
      float horizontalInput = Input.GetAxis("Horizontal");

      rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);

      //flip
      if(horizontalInput > 0.01f) {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
      } else if (horizontalInput < -0.01f) {
        transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
      }

      //jump
      if(Input.GetKey(KeyCode.Space) && grounded) {
        // rb.velocity = new Vector2(rb.velocity.x, movementSpeed);
        Jump();
      }

      //anim parameters

      anim.SetBool("isWalking", horizontalInput != 0);
      anim.SetBool("grounded", grounded);
    }

    private void Jump() {
      rb.velocity = new Vector2(rb.velocity.x, movementSpeed);
      anim.SetTrigger("jump");
      grounded = false;
    }

     private void OnCollisionEnter2D(Collision2D collision) {
      if(collision.gameObject.tag == "Ground") {
        grounded = true;
      }
    }
}

