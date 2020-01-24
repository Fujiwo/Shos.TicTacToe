// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML;
using System;

namespace Shos_TicTacToeML.Model
{
    public class ConsumeModel
    {
        static PredictionEngine<ModelInput, ModelOutput> predEngine;

        static ConsumeModel()
        {
            // Create new MLContext
            var mlContext = new MLContext();
            // Load model & create prediction engine
            var modelPath = AppDomain.CurrentDomain.BaseDirectory + "MLModel.zip";
            var mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);
            predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        // For more info on consuming ML.NET models, visit https://aka.ms/model-builder-consume
        // Method for consuming model in your app
        public static ModelOutput Predict(ModelInput input) => predEngine.Predict(input);
    }
}
