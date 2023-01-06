using UnityEngine;
using UnityEngine.InputSystem;

public class Creature : MonoBehaviour
{
    [SerializeField]
    // A reference to the hatching controller component
    public HatchingController hatchingController = new HatchingController();

    // A reference to the movement controller component
    [SerializeField]
    public MovementController movementController = new MovementController();

    [SerializeField]
    // A reference to the emoji controller component
    public EmojiController emojiController = new EmojiController();

    [SerializeField]
    // A reference to the creature stats component
    public CreatureStats stats = new CreatureStats();

    // The interval at which the creature checks its feelings, in seconds
    public float feelingCheckInterval = 60.0f;

    // The time at which the creature last received attention, in seconds
    private float lastAttentionTime;
    // The sprites for the egg, intermediate, and final stages
    public Sprite eggSprite;
    public Sprite intermediateSprite;
    public Sprite finalSprite;
    [SerializeField]
    // A reference to the creature language component
    public CreatureLanguage creatureLanguage = new CreatureLanguage();
    // The hatching progress counter
    private int hatchingProgress = 0;

    // The sprite renderer component
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        SetLastAttentionTime();
        // Start the hatching process
        hatchingController.Hatch();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Click()
    {
        hatchingProgress++;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Debug.Log("try update sprite");
        if (hatchingProgress < 50)
        {
            spriteRenderer.sprite = eggSprite;
        }
        else if (hatchingProgress < 100)
        {
            spriteRenderer.sprite = intermediateSprite;
        }
        else
        {
            spriteRenderer.sprite = finalSprite;
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        // If the collider is the boundary collider
        if (collider.gameObject.tag == "Boundary")
        {
            // Get the boundary's bounds
            Bounds boundaryBounds = collider.bounds;

            // Get the creature's position
            Vector3 creaturePosition = transform.position;

            // If the creature's x position is outside the boundary's x bounds
            if (creaturePosition.x < boundaryBounds.min.x || creaturePosition.x > boundaryBounds.max.x)
            {
                // Clamp the creature's x position to the boundary's x bounds
                creaturePosition.x = Mathf.Clamp(creaturePosition.x, boundaryBounds.min.x, boundaryBounds.max.x);
            }

            // If the creature's y position is outside the boundary's y bounds
            if (creaturePosition.y < boundaryBounds.min.y || creaturePosition.y > boundaryBounds.max.y)
            {
                // Clamp the creature's y position to the boundary's y bounds
                creaturePosition.y = Mathf.Clamp(creaturePosition.y, boundaryBounds.min.y, boundaryBounds.max.y);
            }

            // Set the creature's position to the clamped position
            transform.position = creaturePosition;
        }
    }

    public void SetLastAttentionTime()
    {
        // Set the initial time at which the creature received attention
        lastAttentionTime = Time.time;
    }

    private void Update()
    {
        emojiController.UpdateEmoji();
        // If the creature is not fully hatched, hasten the hatching process
        if (!hatchingController.isHatched)
        {
            hatchingController.Hatch();
        }

        // If the creature is fully hatched, update its movement and feelings
        if (hatchingController.isHatched)
        {
            // Update the creature's movement
            movementController.Move(this);

            // Check the creature's feelings every feeling check interval seconds
            if (Time.time - lastAttentionTime >= feelingCheckInterval)
            {
                CheckFeelings();
            }
        }
    }

    // Update the creature's happiness based on the elapsed time since it last received attention
    private void CheckFeelings()
    {
        // Calculate the elapsed time since the creature last received attention
        float elapsedTime = Time.time - lastAttentionTime;

        // If the elapsed time is greater than a certain threshold
        if (elapsedTime > 10.0f * 60.0f)
        {
            // Make the creature feel sad and speak with the sad emotion
            ChangeHappiness(-10);
            Speak(CreatureLanguage.Emotion.Sadness);
        }
        else
        {
            // Make the creature feel happy and speak with the happy emotion
            ChangeHappiness(10);
            Speak(CreatureLanguage.Emotion.Happiness);
        }

        // Reset the time at which the creature received attention
        SetLastAttentionTime();
    }

    // Change the creature's happiness by a certain amount
    public void ChangeHappiness(int amount)
    {
        stats.happiness += amount;
    }

    // Make the creature speak with a certain emotion
    public void Speak(CreatureLanguage.Emotion emotion)
    {
        emojiController.ShowEmoji(creatureLanguage.GetEmoji(emotion).Sprite);
    }
}
