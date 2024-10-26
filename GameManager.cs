using TMPro;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject startCanvas;
    public GameObject questionCanvas;
    public GameObject gameplayCanvas; // For in-game UI elements like score and difficulty
    public TextMeshProUGUI difficultyText;

    public TMP_Dropdown ageDropdown;
    public TMP_Dropdown gameExperienceDropdown;
    public TMP_Dropdown similarGameExperienceDropdown;

    public int difficultyLevel = 1;

void Awake(){
    Instance = this;
}
    
    void Start()
    {
        ShowStartScreen(); // Show start screen when game starts
    }

    public void OnStartButtonPressed()
    {
        // Hide start screen and show questions
        startCanvas.SetActive(false);
        questionCanvas.SetActive(true);
    }

    public void OnSubmitAnswers()
    {
        // Get dropdown answers
        int age = ageDropdown.value;
        int gameExperience = gameExperienceDropdown.value;
        int similarGameExperience = similarGameExperienceDropdown.value;

        // Adjust difficulty based on answers
        AdjustDifficulty(age, gameExperience, similarGameExperience);

        // Hide question canvas and show gameplay UI
        questionCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
    }

    void AdjustDifficulty(int age, int gameExperience, int similarGameExperience)
    {
        // Example: Use a basic logic based on answers to set difficulty (customize as needed)
        if (age == 2 && gameExperience == 2 && similarGameExperience == 2) // Assuming '2' is the highest option
        {
            difficultyLevel = 3; // High difficulty
        }
        else if (age == 1 || gameExperience == 1 || similarGameExperience == 1) // Assuming '1' is medium
        {
            difficultyLevel = 2; // Medium difficulty
        }
        else
        {
            difficultyLevel = 1; // Low difficulty
        }

        Debug.Log($"Initial Difficulty Set to: {difficultyLevel}");
        UpdateDifficultyText();
    }

    void UpdateDifficultyText()
    {
        if (difficultyText != null)
        {
            difficultyText.text = "Difficulty Level: " + difficultyLevel;
        }
    }

    // Show/hide canvases
    void ShowStartScreen()
    {
        startCanvas.SetActive(true);
        questionCanvas.SetActive(false);
        gameplayCanvas.SetActive(false);
    }

    // This method could be called at the end of the game to analyze reaction time and completion time
    public void AnalyzePerformance(float reactionTime, float completionTime)
    {
        // Example adjustment based on performance (you can expand this as needed)
        if (reactionTime < 1.0f && completionTime < 10.0f)
        {
            difficultyLevel = Mathf.Clamp(difficultyLevel + 1, 1, 3); // Increase difficulty if performance is high
        }
        else if (reactionTime > 3.0f || completionTime > 20.0f)
        {
            difficultyLevel = Mathf.Clamp(difficultyLevel - 1, 1, 3); // Decrease difficulty if performance is low
        }
        UpdateDifficultyText();
    }
}
