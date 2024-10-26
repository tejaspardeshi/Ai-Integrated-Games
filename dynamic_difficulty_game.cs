using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDifficultyGame : MonoBehaviour
{
    private int difficultyLevel = 1; // Initial difficulty level (easy)
    private List<int> playerPerformance = new List<int>(); // Track player's performance over time
    private List<int> difficultyHistory = new List<int>(); // Track difficulty changes over time
    private System.Random random = new System.Random();

    // Simulates a game round
    public void PlayRound()
    {
        // Simulate player's score for a round (out of 100)
        int score = random.Next(40, 101) - (difficultyLevel * 10);
        if (score < 0) score = 0;
        playerPerformance.Add(score);

        // Adjust difficulty based on performance
        if (score > 80)
            IncreaseDifficulty();
        else if (score < 50)
            DecreaseDifficulty();

        Debug.Log($"Round score: {score}, Current difficulty: {difficultyLevel}");
    }

    // Increases the difficulty level
    private void IncreaseDifficulty()
    {
        if (difficultyLevel < 5)
        {
            difficultyLevel++;
            Debug.Log("Difficulty increased!");
        }
        difficultyHistory.Add(difficultyLevel);
    }

    // Decreases the difficulty level
    private void DecreaseDifficulty()
    {
        if (difficultyLevel > 1)
        {
            difficultyLevel--;
            Debug.Log("Difficulty decreased!");
        }
        difficultyHistory.Add(difficultyLevel);
    }

    // Runs the game simulation
    public void RunGame(int rounds = 10)
    {
        for (int i = 0; i < rounds; i++)
        {
            PlayRound();
        }

        // If using Unity UI, call a function to display `playerPerformance` and `difficultyHistory`
        // on the game screen here if you need to visualize the data.
        DisplayProgress();
    }

    // For now, we display the performance and difficulty history in the console
    private void DisplayProgress()
    {
        Debug.Log("Player Performance: " + string.Join(", ", playerPerformance));
        Debug.Log("Difficulty History: " + string.Join(", ", difficultyHistory));
    }
}

// Usage in Unity:
// Attach this script to a GameObject in Unity, then call RunGame from another script or UI button to run.
