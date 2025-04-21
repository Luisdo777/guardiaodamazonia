using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player :   MonoBehaviour
{
    private Rigidbody2D rbPlayer;
    public float speed;
    private SpriteRenderer sr;
    public float jumpForce;
    public bool inFloor =  true;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rbPlayer = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        Jump();
    }
    void MovePlayer()
    {
        float horizontalMoviment = Input.GetAxisRaw("Horizontal");
        rbPlayer.linearVelocity = new Vector2(horizontalMoviment * speed, rbPlayer.linearVelocity.y);

        if (horizontalMoviment > 0)
        {
            sr.flipX = false;
        }

        else if (horizontalMoviment < 0)
        {
            sr.flipX = false;
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && inFloor)
        {
            rbPlayer.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            inFloor = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "ground"){
            inFloor = true;
        }
    }
}
