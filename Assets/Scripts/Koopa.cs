using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;

    private bool shelled;
    private bool pushed;
    public float shellSpeed = 12;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(this.transform, Vector2.down)) {
                EnterShell();
            } else {
                collision.gameObject.GetComponent<Player>().Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.gameObject.CompareTag("Player")) {
            if (!pushed) {
                var direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            } else {
                other.GetComponent<Player>().Hit();
            }
        } else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell")) {
            Hit();
        }
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        var movement = GetComponent<EntityMovement>();
        movement.enabled = true;
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void EnterShell()
    {
        shelled = true;

        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;

        Destroy(gameObject, 3f);
    }
}
