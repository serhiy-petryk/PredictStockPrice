using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Transforms;
using MLNETTaxi.Models;

namespace MLNETTaxi
{
    class Program
    {
        static void Main(string[] args)
        {
            var _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");
            var _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");
            var _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model, _testDataPath);
            TestSinglePrediction(mlContext, model);

            Console.ReadLine();
        }

        static ITransformer Train(MLContext mlContext, string dataPath)
        {
            // Loads the data.
            // Extracts and transforms the data.
            // Trains the model.
            // Returns the model.

            // IDataView can load either text files or in real time (for example, SQL database or log files).
            IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(dataPath, hasHeader: true, separatorChar: ',');
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
                .Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
                // Choose a learning algorithm
                .Append(mlContext.Regression.Trainers.FastTree());

            ColumnCopyingEstimator x1 = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount");
            var x2 = x1.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded",
                inputColumnName: "VendorId"));
            var x3 = x2.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded",
                inputColumnName: "RateCode"));
            var x4 = x3.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded",
                inputColumnName: "PaymentType"));
            var x5 = x4.Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded",
                "PassengerCount", "TripDistance", "PaymentTypeEncoded"));
            // Choose a learning algorithm
            var x6 = x5.Append(mlContext.Regression.Trainers.FastTree());

            // Train the model
            var model = pipeline.Fit(dataView);
            return model;
        }

        static void Evaluate(MLContext mlContext, ITransformer model, string testDataPath)
        {
            // Loads the test dataset.
            // Creates the regression evaluator.
            // Evaluates the model and creates metrics.
            // Displays the metrics.

            // Load the test dataset
            IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(testDataPath, hasHeader: true, separatorChar: ',');
            // The Transform() method makes predictions for the test dataset input rows.
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");

            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:0.##}");
        }

        static void TestSinglePrediction(MLContext mlContext, ITransformer model)
        {
            // Creates a single comment of test data.
            // Predicts fare amount based on test data.
            // Combines test data and predictions for reporting.
            // Displays the predicted results.

            var predictionFunction = mlContext.Model.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(model);
            var taxiTripSample = new TaxiTrip()
            {
                VendorId = "VTS",
                RateCode = "1",
                PassengerCount = 1,
                TripTime = 1140,
                TripDistance = 3.75f,
                PaymentType = "CRD",
                FareAmount = 0 // To predict. Actual/Observed = 15.5
            };
            var prediction = predictionFunction.Predict(taxiTripSample);

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted fare: {prediction.FareAmount:0.####}, actual fare: 15.5");
            Console.WriteLine($"**********************************************************************");
        }
    }
}
