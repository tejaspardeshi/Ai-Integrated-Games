using System.Diagnostics;
using UnityEngine;

public class DifficultyPredictor : MonoBehaviour
{
    public int age;
    public int gameExperience;
    public int similarGameExperience;
    public string pythonScriptPath = "D:\Desktop\final\final"; // Update this path

    // Method to predict difficulty using Python model
    public int PredictDifficulty()
    {
        try
        {
            // Start a new process to call the Python script
            Process process = new Process();
            process.StartInfo.FileName = "final"; // Ensure final is in PATH
            process.StartInfo.Arguments = $"{pythonScriptPath} {age} {gameExperience} {similarGameExperience}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            // Start the process
            process.Start();

            // Read the output, which should be the difficulty level
            string output = process.StandardOutput.ReadLine();
            process.WaitForExit();

            // Convert the output to an integer difficulty level
            if (int.TryParse(output, out int predictedDifficulty))
            {
                Debug.Log($"Predicted Difficulty Level: {predictedDifficulty}");
                return predictedDifficulty;
            }
            else
            {
                Debug.LogError("Failed to parse predicted difficulty from Python output.");
                return -1; // Return -1 in case of an error
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error calling Python script: {e.Message}");
            return -1;
        }
    }
}
