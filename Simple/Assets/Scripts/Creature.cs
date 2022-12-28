using UnityEngine;
using UnityEngine.InputSystem;

public class Creature : MonoBehaviour
{
    [SerializeField]
    // A reference to the hatching controller component
    public HatchingController hatchingController = new HatchingController();

    // A reference to the movement controller component
    private MovementController movementController;

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

    // A reference to the creature language component
    private CreatureLanguage creatureLanguage;

    private void Start()
    {
        movementController = new MovementController(this);
        creatureLanguage = new CreatureLanguage(emojiController);
        SetLastAttentionTime();
        // Start the hatching process
        hatchingController.Hatch();
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
            movementController.Move();

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
        creatureLanguage.Speak(emotion);
    }
}
