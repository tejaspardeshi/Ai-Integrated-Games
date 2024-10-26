using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;

public class PlayerData
{
    [LoadColumn(0)]
    public float Age;

    [LoadColumn(1)]
    public float GameExperience;

    [LoadColumn(2)]
    public float SimilarGameExperience;

    [LoadColumn(3)]
    public float Difficulty; // Target label
}

public class DifficultyPrediction
{
    [ColumnName("PredictedLabel")]
    public float PredictedDifficulty;
}

public class DifficultyModel
{
    static readonly string ModelPath = "difficulty_model.zip";
    static readonly MLContext mlContext = new MLContext(seed: 42);

    public void TrainAndSaveModel()
    {
        // Sample data
        var data = new List<PlayerData>
        {
            new PlayerData { Age = 16, GameExperience = 1, SimilarGameExperience = 0, Difficulty = 1 },
            new PlayerData { Age = 24, GameExperience = 3, SimilarGameExperience = 1, Difficulty = 2 },
            new PlayerData { Age = 30, GameExperience = 5, SimilarGameExperience = 3, Difficulty = 3 },
            new PlayerData { Age = 45, GameExperience = 7, SimilarGameExperience = 5, Difficulty = 2 }
        };

        // Load data into IDataView
        IDataView dataView = mlContext.Data.LoadFromEnumerable(data);

        // Define data processing and training pipeline
        var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", "Age", "GameExperience", "SimilarGameExperience")
            .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(PlayerData.Difficulty)))
            .Append(mlContext.Transforms.NormalizeMinMax("Features"));

        var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.FastTree())
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
        
        var trainingPipeline = dataProcessPipeline.Append(trainer);

        // Train/Test split
        var trainTestSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

        // Train the model
        var trainedModel = trainingPipeline.Fit(trainTestSplit.TrainSet);

        // Evaluate the model
        var predictions = trainedModel.Transform(trainTestSplit.TestSet);
        var metrics = mlContext.MulticlassClassification.Evaluate(predictions);
        Console.WriteLine($"Model accuracy: {metrics.MacroAccuracy:F2}");

        // Save the trained model
        mlContext.Model.Save(trainedModel, dataView.Schema, ModelPath);
        Console.WriteLine("Model saved to " + ModelPath);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var difficultyModel = new DifficultyModel();
        difficultyModel.TrainAndSaveModel();
    }
}
