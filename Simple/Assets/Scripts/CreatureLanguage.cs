using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class CreatureLanguage
{
    // An enum for the emotions of the creature
    public enum Emotion
    {
        Hunger,
        Happiness,
        Sadness,
        Anger,
        Flirting,
        Content
    }

    [Serializable]
    public struct Emoji
    {
        public Emotion Emotion;
        public Sprite Sprite;
    }

    public Emoji[] Emotions;

    // Get the emoji for the specified emotion
    public Emoji GetEmoji(Emotion emotion)
    {
        return Emotions.Where(p => p.Emotion.Equals(emotion)).First();
    }
}