using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallSpriteRenderer;
    public PlayerSpriteRenderer largeSpriteRenderer;

    private DeathAnimation deathAnimation;

    public bool big => largeSpriteRenderer.enabled;
    public bool small => smallSpriteRenderer.enabled;
    public bool dead => deathAnimation.enabled;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();

    }

    public void Hit()
    {
        if (big) {
            Shrink();
        } else {
            Death();
        }
    }

    private void Death()
    {
        smallSpriteRenderer.enabled = false;
        largeSpriteRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    private void Shrink()
    {
        throw new NotImplementedException();
    }
}
