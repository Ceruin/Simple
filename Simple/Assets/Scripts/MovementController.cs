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

    private Vector3 targetPosition;

    public void MoveToLocation(Vector3 location)
    {
        targetPosition = location;
    }

    void CheckForCollision(Creature creature)
    {
        // Get the collider on the creature
        Collider2D creatureCollider = creature.GetComponent<Collider2D>();

        // Calculate the future position
        Vector3 futurePosition = creature.gameObject.transform.position + (Vector3)direction * creature.stats.Dexterity * Time.deltaTime;

        // Check if the future position will cross the other collider
        Vector2 origin = creature.gameObject.transform.position;
        Vector2 newdirection = futurePosition - creature.gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, newdirection, creature.stats.Dexterity * Time.deltaTime);

        if (hit.collider != null && hit.collider != creatureCollider)
        {
            // If a collision is detected, move the creature in the opposite direction
            MoveAwayFromCollider(creature, hit.collider);
        }
        else
        {
            creature.gameObject.transform.position = futurePosition;
        }
    }


    void MoveAwayFromCollider(Creature creature, Collider2D collider)
    {
        Vector2 awayFromCollider = (new Vector2(creature.transform.position.x, creature.transform.position.y) - new Vector2(collider.transform.position.x, collider.transform.position.y)).normalized;

        //Move the creature in that direction
        creature.transform.Translate(awayFromCollider * Time.deltaTime);
        // or set the velocity, depending on how you move your creature
        //rigidbody.velocity = awayFromCollider;
    }

    public void Move(Creature creature)
    {
        if (!isDragged)
        {
            if (!creature.hatchingController.isHatched)
            {
                // Skip the movement update if the creature is not hatched
                return;
            }

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
            CheckForCollision(creature);
        }
    }

}
