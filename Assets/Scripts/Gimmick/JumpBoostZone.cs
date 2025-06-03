using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    [SerializeField] float jumpForce = 400f, speed = 5f, jumpZoneForce = 2f;
    int jumpCount = 1;
    float moveX, moveZ;

    bool isGround = false;
    bool isJumpZone = false;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpCount = 0;
    }

    void Update()
    {
        Movement();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = 1;
        }
        if (col.gameObject.CompareTag("JumpZone"))
        {
            isJumpZone = true;
        }
    }

    void Movement()
    {
        if (isGround)
        {
            if (jumpCount > 0 && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce);
                jumpCount--;
            }

            if (isJumpZone)
            {
                rb.AddForce(Vector3.up * jumpForce * jumpZoneForce);
                isJumpZone = false;
            }
        }

        moveX = Input.GetAxis("Horizontal") * speed;
        moveZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(moveX, rb.velocity.y, moveZ);
        rb.velocity = movement;
    }
}