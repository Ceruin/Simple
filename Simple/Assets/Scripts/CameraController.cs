using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float movespeedMultiplier = 1.0f; // A value that determines how much the camera's speed should be affected by the player's movespeed

    void Update()
    {
        // Calculate a "t" value based on the player's movespeed
        float t = movespeedMultiplier * Time.deltaTime;

        // Interpolate the camera's position towards the player's position, with a Z position of -10
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, -10), t);
    }
}
