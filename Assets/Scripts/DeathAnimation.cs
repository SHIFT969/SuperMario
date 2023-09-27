using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite deathSprite;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;

        if (deathSprite != null) {
            spriteRenderer.sprite = deathSprite;
        }
    }

    private void DisablePhysics()
    {
        var colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        GetComponent<Rigidbody2D>().isKinematic = true;

        if (TryGetComponent<PlayerMovement>(out var playerMovement)) {
            playerMovement.enabled = false;
        }

        if (TryGetComponent<EntityMovement>(out var entityMovement)) {
            entityMovement.enabled = false;
        }
    }

    private IEnumerator Animate()
    {
        var elapsed = 0f;
        var duration = 3f;

        var jumpVelocity = 10f;
        var gravity = -36f;

        var velocity = Vector3.up * jumpVelocity;

        while (elapsed < duration) {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
