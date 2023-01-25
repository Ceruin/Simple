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

    // A reference to the animator component
    private Animator animator;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Get a reference to the animator component
        animator = GetComponent<Animator>();
    }

    private void CheckForItem()
    {
        // Check if there is an item within the pick up radius
        Collider[] itemsInRadius = Physics.OverlapSphere(transform.position, pickUpRadius, LayerMask.GetMask("Item"));
        if (itemsInRadius.Length > 0)
        {
            // Sort the items based on priority
            // ...

            // Set the currentItem variable to the highest priority item
            currentItem = itemsInRadius[0].GetComponent<Item>();

            // Make the creature path to the item

            //agent.SetDestination(currentItem.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            // Make the creature pick up the item
            currentItem = other.GetComponent<Item>();
            currentItem.PickUp();
            currentItem.transform.parent = transform;
            currentItem.transform.localPosition = new Vector3(0, 0, 1);
        }
    }

    public void Click()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Debug.Log("try update sprite");
        if (hatchingController.percent < 50)
        {
            spriteRenderer.sprite = eggSprite;
            GetComponent<PolygonCollider2D>().TryUpdateShapeToAttachedSprite();
        }
        else if (hatchingController.percent < 100)
        {
            spriteRenderer.sprite = intermediateSprite;
            GetComponent<PolygonCollider2D>().TryUpdateShapeToAttachedSprite();
        }
        else
        {
            spriteRenderer.sprite = finalSprite;
            GetComponent<PolygonCollider2D>().TryUpdateShapeToAttachedSprite();
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        // If the collider is the boundary collider
        if (collider.gameObject.tag == "Boundary")
        {
            HandleBoundaryCollision(collider);
        }
    }

    private void HandleBoundaryCollision(Collider2D collider)
    {
        Bounds boundaryBounds = collider.bounds;
        Vector3 creaturePosition = transform.position;

        if (creaturePosition.x < boundaryBounds.min.x || creaturePosition.x > boundaryBounds.max.x)
        {
            creaturePosition.x = Mathf.Clamp(creaturePosition.x, boundaryBounds.min.x, boundaryBounds.max.x);
        }

        if (creaturePosition.y < boundaryBounds.min.y || creaturePosition.y > boundaryBounds.max.y)
        {
            creaturePosition.y = Mathf.Clamp(creaturePosition.y, boundaryBounds.min.y, boundaryBounds.max.y);
        }

        transform.position = creaturePosition;
    }

    public void SetLastAttentionTime()
    {
        // Set the initial time at which the creature received attention
        lastAttentionTime = Time.time;
    }

    public void CheckForEvolution()
    {
        if (stats.Age > 500f)
        {
            stats.Evolve();
        }
    }

    public void UpdateHunger()
    {
        stats.IncreaseHunger(Mathf.Clamp(stats.Hunger + Time.deltaTime, 0f, 100f));
    }
    public void UpdateAge()
    {
        stats.IncreaseAge(Time.deltaTime);
    }
    public float pickUpRadius = 2.0f;

    private Item currentItem;
    private void Update()
    {
        CheckForEvolution();
        UpdateHunger();
        UpdateAge();
        DisplayEmotion();
        UpdateUnhatched();
        UpdateHatched();
        CheckForItem();
    }

    private void UpdateHatched()
    {
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

    private void UpdateUnhatched()
    {
        // If the creature is not fully hatched, hasten the hatching process
        if (!hatchingController.isHatched)
        {
            hatchingController.Hatch();
            Debug.Log("Time: " + hatchingController.hatchTimer + " Hatched?: " + hatchingController.isHatched);
            // If the creature is in its egg stage
            if (hatchingController.isHatched)
            {
                // Play the hatching animation
                //animator.SetTrigger("Hatch");
            }
            UpdateSprite();
        }
    }

    private void DisplayEmotion()
    {
        emojiController.UpdateEmoji();
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
        if (amount > 0)
            stats.IncreaseHappiness(amount);
        else 
            stats.DecreaseHappiness(amount);
    }

    // Make the creature speak with a certain emotion
    public void Speak(CreatureLanguage.Emotion emotion)
    {
        emojiController.ShowEmoji(creatureLanguage.GetEmoji(emotion).Sprite);
    }
}
