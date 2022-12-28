using UnityEngine;

public class EmojiController 
{
    // The text mesh component for displaying the emotion emoji
    public TextMesh emojiTextMesh;

    // The duration for which the emotion emoji is displayed
    public float emojiDuration = 1.0f;

    // A timer for displaying the emotion emoji
    private float emojiTimer = 0.0f;

    public void UpdateEmoji()
    {
        // Update the emoji timer
        emojiTimer += Time.deltaTime;

        // If the emoji timer has reached the emoji duration, hide the emoji
        if (emojiTimer >= emojiDuration)
        {
            HideEmoji();
        }
    }

    // Show the specified emoji above the creature
    public void ShowEmoji(string emoji)
    {
        // Set the text of the text mesh to the specified emoji
        //emojiTextMesh.text = emoji;

        // Reset the emoji timer
        emojiTimer = 0.0f;
    }

    // Hide the emoji
    public void HideEmoji()
    {
        // Set the text of the text mesh to an empty string
        //emojiTextMesh.text = "";
    }
}
