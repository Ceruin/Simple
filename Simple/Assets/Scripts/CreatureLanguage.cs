using System.Collections.Generic;
using UnityEngine;

public class CreatureLanguage
{
    private EmojiController EmojiController;
    public CreatureLanguage(EmojiController emojiController)
    {
        EmojiController = emojiController;
    }

    // An enum for the emotions of the creature
    public enum Emotion
    {
        Hunger,
        Happiness,
        Sadness,
        Anger,
        Flirting,
        Average
    }

    // A dictionary that maps emotions to emojis
    private Dictionary<Emotion, string> emotionEmojiMap = new Dictionary<Emotion, string>()
    {
        { Emotion.Hunger, "🤤" },
        { Emotion.Happiness, "😃" },
        { Emotion.Sadness, "😢" },
        { Emotion.Anger, "😠" },
        { Emotion.Flirting, "😍" },
        { Emotion.Average, "😐" }
    };

    // Get the emoji for the specified emotion
    public string GetEmoji(Emotion emotion)
    {
        return emotionEmojiMap[emotion];
    }

    // Make the creature speak with the specified emotion
    public void Speak(Emotion emotion)
    {
        // Get the appropriate emoji for the emotion
        string emoji = GetEmoji(emotion);

        // Display the emoji above the creature
        EmojiController.ShowEmoji(emoji);
    }
}
