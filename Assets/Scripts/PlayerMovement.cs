using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    private new Rigidbody2D rigidbody;
    private new Camera camera;

    private float inputAxis;
    private Vector2 velocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    private void Update()
    {
        HorizontalMovement();
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded) {
            GroundedMovement();
        }

        ApplyGravity();
    }

    private void ApplyGravity()
    {
        var falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplyer = falling ? 2f : 1f;
        velocity.y += gravity * Time.deltaTime * multiplyer;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0);
        jumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump")) {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if (rigidbody.Raycast(Vector2.right * velocity.x)) {
            velocity.x = 0;
        }

        if (velocity.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f);
        }
    }

    private void FixedUpdate()
    {
        var position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        var leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        var rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            if (transform.DotTest(collision.transform, Vector2.down)) {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        } else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp")) {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }
}
