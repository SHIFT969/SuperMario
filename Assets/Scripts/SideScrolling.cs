using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        var cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(player.position.x, transform.position.x);
        transform.position = cameraPosition;
    }
}
