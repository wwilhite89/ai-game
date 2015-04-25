using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using ArtificialNeuralNetworks.Core;

namespace ArtificialNeuralNetworks.Training
{
    public class Trainer : IDisposable
    {
        private List<InputComponent> components;
        private double[] currentDecisions;
        private bool isActive = false;
        private StreamWriter writer;
        private string lastDataLine = "";
        public string characterName { get; private set; }

        public Trainer(string trainingFilePath, string characterName) 
        {
            this.characterName = characterName;
            this.components = new List<InputComponent>();
            this.writer = new StreamWriter(trainingFilePath, true);
        }

        public void StartTraining()
        {
            this.isActive = true;
        }

        public void StopTraining()
        {
            this.isActive = false;
        }

        public void SetCurrentDecision(double[] outputs)
        {
            this.currentDecisions = outputs;
        }

        public void PrintTraining()
        {
            if (this.isActive && this.currentDecisions != null)
            {
                StringBuilder dataLine = new StringBuilder();

                // Write inputs
                foreach (var com in this.components)
                    dataLine.Append(com.GetCurrentTraining().ToString("F6") + " ");
                
                // Write outputs
                foreach (var output in this.currentDecisions)
                    dataLine.Append(output.ToString("F2") + " ");

                string newData = dataLine.ToString();

                if (lastDataLine != newData)
                {
                    writer.WriteLine(newData);
                    this.lastDataLine = newData;
                }
            }
        }

        public IDictionary<string, double> GetSensorData()
        {
            var values = new Dictionary<string, double>();
            this.components.ForEach(x => values.Add(x.ToString(), x.GetCurrentTraining()));
            return values;
        }

        public void RegisterComponent(InputComponent component)
        {
            this.components.Add(component);
        }

        public void Dispose()
        {
            if (this.writer != null)
            {
                this.writer.Flush();
                this.writer.Close();
            }
        }
    }
}
