using UnityEngine;

public class HatchingController 
{
    // The time it takes for the creature to hatch from an egg
    public float hatchTime = 1.0f;
    // A flag indicating whether the creature is fully hatched
    public bool isHatched = false;
    // A reference to the animator component
    private Animator animator;

    // A timer for hatching
    private float hatchTimer = 0.0f;

    public void Hatch()
    {
        // Update the hatch timer
        hatchTimer += Time.deltaTime;

        // If the hatch timer has reached the hatch time, the creature has hatched
        if (hatchTimer >= hatchTime)
        {
            // Reset the hatch timer
            hatchTimer = 0.0f;

            // Set the animator's "Hatched" parameter to true
            //animator.SetBool("Hatched", true);

            // The creature is now able to move around
            //enabled = false;

            isHatched = true;
        }
    }

    public void HastenHatching()
    {
        // Reduce the hatch time by 0.1 seconds
        hatchTime = Mathf.Max(hatchTime - 0.1f, 0.0f);
    }
}
