﻿using System;
using System.IO;
using System.Linq;
using Functions;
using Accord.IO;
using Accord.Statistics.Models.Regression.Fitting;
using Accord.Statistics.Models.Regression;
using Accord.Math;


namespace TestAccordLR
{
    class Program
    {
        static void Main(string[] args)
        {
            bool modelFile = false;
            if (args.Length > 3 | args.Length < 1)
            {
                Console.WriteLine ("Requires a previously trained and saved Model File");
                Console.WriteLine("Usage <testfile> <label file> opt<Model File>");
                System.Environment.Exit(-1);
                
            }
            else if (args.Length == 3)
            {
                modelFile = true;
                
            }
            Console.WriteLine("Accord Logisitic Regression Prediction");
            string testFname = args[0];
            string labelsFname = args[1];
            string ModelFname = null;
            if (modelFile)
            {
                ModelFname = args[2];
            }
            
            
            double[,] Rawdata;
            double[,] labeldata;
            // Read in the test data, validate file existence by attempting to open the files first
            try
            {
                FileStream fs = File.Open(testFname, FileMode.Open, FileAccess.Write, FileShare.None);
                fs.Close();
                // Reuse fs for validating labels 
                fs = File.Open(labelsFname, FileMode.Open, FileAccess.Write, FileShare.None);
                fs.Close();

                fs = File.Open(ModelFname, FileMode.Open, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening file{0}", e);
                System.Environment.Exit(-1);
            }
            using (CsvReader reader = new CsvReader(testFname, hasHeaders: false))
            {
                Rawdata = reader.ToMatrix();
            }
            using (CsvReader reader = new CsvReader(labelsFname, hasHeaders: false))
            {
                labeldata = reader.ToMatrix();
            }

            // Convert Raw data to Jagged array
            double[][] testdata = Rawdata.ToJagged();
            int[] output1 = funcs.convetToJaggedArray(labeldata);
            if (modelFile == false)
            {
                ModelFname = "RegressionModel.save";
            }


            // For Accord.net Logistic Regression the input data needs to be in Jagged Arrays         
            // Labels can either be int (1,0) or bools

            // Declare a new regression of type LogisticRegressiona and then trt to Load the modle into memeory
            LogisticRegression regression = new LogisticRegression();
            try
            {
                regression = Serializer.Load<LogisticRegression>(ModelFname);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening model file: {0}", ModelFname);
                Console.WriteLine("Exception {0}", e);
                System.Environment.Exit(-1);
            }
            Console.WriteLine("Successfully loaded model file => {0}", ModelFname);
            double [] foo = regression.Probability(testdata);
            bool[] actual = regression.Decide(testdata);
            int[] predictions = funcs.BoolToInt(actual);
            double subtotal = 0;
            int index = 0;
            foreach (var result in predictions)
            {
                if (result == output1[index])
                {
                    subtotal = subtotal + 1;
                }
                index++;
            }
            double accuracy = subtotal / predictions.Count();
            Console.WriteLine("Predicted accuracy:{0}", Math.Round(accuracy * 100, 2));



        }
    }
}
