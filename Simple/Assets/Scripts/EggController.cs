using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EggController : MonoBehaviour
{
    public GameObject creaturePrefab; // Reference to the creature prefab

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider that the egg has collided with is the player's collider
        if (collision.gameObject.tag == "Player")
        {
            // Ignore the collision and return early
            return;
        }

        // Destroy the egg when it collides with something
        Destroy(gameObject);

        // Spawn a new instance of the creature prefab at the position of the destroyed egg
        Instantiate(creaturePrefab, transform.position, Quaternion.identity);
    }
}

