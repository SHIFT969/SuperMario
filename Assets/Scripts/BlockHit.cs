using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class BlockHit : MonoBehaviour
{
    public int maxHits = -1;
    public Sprite emptyBlock;
    public GameObject item;

    private bool animating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!animating && maxHits != 0 && collision.transform.DotTest(this.transform, Vector2.up)) {
                Hit();
            }
        }
    }

    private void Hit()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        maxHits--;

        if (maxHits == 0) {
            spriteRenderer.sprite = emptyBlock;
        }

        if (item != null) {
            Instantiate(item, transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        animating = true;

        var restingPosition = transform.localPosition;
        var animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        var elapsed = 0f;
        var duration = 0.125f;

        while (elapsed < duration) {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }
}
