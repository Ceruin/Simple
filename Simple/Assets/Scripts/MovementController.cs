using UnityEngine;

public class MovementController 
{
    // The distance at which the creature will change direction
    public float changeDirectionDistance = 1.0f;

    // A timer for changing direction
    private float changeDirectionTimer = 0.0f;

    // The current direction the creature is moving in
    private Vector2 direction = Vector2.right;

    // A reference to the creature
    private Creature creature;

    public MovementController(Creature creature)
    {
        this.creature = creature;
    }

    public void Move()
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

        // Get the current position of the creature
        Vector3 currentPosition = creature.gameObject.transform.position;

        // Update the x and y components of the position based on the direction and speed
        currentPosition.x += direction.x * creature.stats.speed * Time.deltaTime;
        currentPosition.y += direction.y * creature.stats.speed * Time.deltaTime;

        // Print the current position to the console for debugging purposes
        //Debug.Log("Current position: " + currentPosition);

        // Set the creature's position to the updated position
        creature.gameObject.transform.position = currentPosition;
    }
}
