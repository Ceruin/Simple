using System;
using UnityEngine;

// stats based on dark souls, int dex, strength
[Serializable]
public class CreatureStats
{
    public float Intelligence;
    public float Strength;
    public float Dexterity;
    public float Hunger = 0f;
    public float Age;
    public float Happiness = 0f;
    public Evolution EvolutionState;
    public Emotion EmotionState;

    public enum Emotion
    {
        Content,
        Angry,
        Hungry,
        Sad,
        Happy,
        Flirtatious
    }

    public enum Evolution
    {
        Baby,
        Juvenile,
        Adult
    }

    public void IncreaseHunger(float hunger)
    {
        Hunger = Mathf.Min(Hunger + hunger, 100f);
    }

    public void DecreaseHunger(float hunger)
    {
        Hunger = Mathf.Max(Hunger - hunger, 0f);
    }

    public void IncreaseHappiness(float happiness)
    {
        Happiness = Mathf.Min(Happiness + happiness, 100f);
    }

    public void DecreaseHappiness(float happiness)
    {
        Happiness = Mathf.Max(Happiness - happiness, 0f);
    }

    public void IncreaseAge(float age)
    {
        Age += age;
    }

    public void Evolve()
    {
        switch (EvolutionState)
        {
            case Evolution.Baby:
                EvolutionState = Evolution.Juvenile;
                break;

            case Evolution.Juvenile:
                EvolutionState = Evolution.Adult;
                break;

            case Evolution.Adult:
                // Creature can't evolve any further
                break;
        }
        Age = 0;
    }

    // Other fields and methods
    public NeuralNetwork emotionNetwork;

    public float learningRate = 0.1f;

    public void UpdateEmotion()
    {
        float[] inputs = new float[] { Hunger, Happiness, Age };
        float[] output = emotionNetwork.FeedForward(inputs);
        int maxIndex = 0;
        for (int i = 1; i < output.Length; i++)
        {
            if (output[i] > output[maxIndex])
            {
                maxIndex = i;
            }
        }
        EmotionState = (Emotion)maxIndex;

        // Train the network
        float[] target = new float[Enum.GetNames(typeof(Emotion)).Length];
        target[(int)EmotionState] = 1;
        emotionNetwork.BackPropagation(inputs, target, learningRate);
    }
}