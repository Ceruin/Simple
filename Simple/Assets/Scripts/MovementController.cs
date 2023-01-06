using UnityEngine;

public class MovementController 
{
    // The distance at which the creature will change direction
    public float changeDirectionDistance = 1.0f;

    // A timer for changing direction
    private float changeDirectionTimer = 0.0f;

    // The current direction the creature is moving in
    private Vector2 direction = Vector2.right;

    public bool isDragged = false;

    public void Move(Creature creature)
    {
        if (!isDragged)
        {
            // Update the change direction timer
            changeDirectionTimer += Time.deltaTime;

            // If the change direction timer has reached the change direction distance, it's time to change direction
            if (changeDirectionTimer >= changeDirectionDistance)
            {
                // Reset the change direction timer
                changeDirectionTimer = 0.0f;

                // Choose a new random direction
                direction = Random.insideUnitCircle.normalized;
            }

            // Move the creature in the current direction
            creature.gameObject.transform.position += (Vector3)direction * creature.stats.speed * Time.deltaTime;
        }
    }

}
