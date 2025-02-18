using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed;
    private Rigidbody2D rb;
    private Animator anim;

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
      if(Input.GetKey(KeyCode.Space)) {
        rb.velocity = new Vector2(rb.velocity.x, movementSpeed);
      }

      //anim parameters

      anim.SetBool("isWalking", horizontalInput != 0);
    }

}