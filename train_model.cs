using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

public class GameData
{
    [LoadColumn(0)]
    public float Score;

    [LoadColumn(1)]
    public float TimeTaken;

    [LoadColumn(2)]
    public float Accuracy;

    [LoadColumn(3)]
    public float NormalizedScore;  // Target
}

public class GameDifficultyPrediction
{
    [ColumnName("Score")]
    public float PredictedScore;
}

public class DifficultyModel
{
    static readonly string ModelPath = "dda_model.zip";
    static readonly int Epochs = 1000;

    public void TrainModel(string dataPath)
    {
        var mlContext = new MLContext();

        // Load data from file
        IDataView data = mlContext.Data.LoadFromTextFile<GameData>(dataPath, separatorChar: ',', hasHeader: true);

        // Data Preparation: Normalizing score between 0 and 1
        var dataProcessPipeline = mlContext.Transforms.CopyColumns("NormalizedScore", "Score")
            .Append(mlContext.Transforms.Concatenate("Features", "Score", "TimeTaken", "Accuracy"))
            .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"));

        // Model Definition
        var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "NormalizedScore", featureColumnName: "Features");
        var trainingPipeline = dataProcessPipeline.Append(trainer);

        // Training the Model with Epochs Simulation
        ITransformer trainedModel = null;
        float loss = 0f;
        for (int epoch = 1; epoch <= Epochs; epoch++)
        {
            var modelWithEpoch = trainingPipeline.Fit(data);
            var predictions = modelWithEpoch.Transform(data);
            var metrics = mlContext.Regression.Evaluate(predictions, "NormalizedScore", "Score");

            loss = metrics.MeanSquaredError;

            if (epoch % 100 == 0)
            {
                Console.WriteLine($"Epoch [{epoch}/{Epochs}], Loss: {loss:F4}");
            }

            trainedModel = modelWithEpoch;
        }

        // Save the final model
        mlContext.Model.Save(trainedModel, data.Schema, ModelPath);
        Console.WriteLine("Model training completed and saved at " + ModelPath);
    }
}
