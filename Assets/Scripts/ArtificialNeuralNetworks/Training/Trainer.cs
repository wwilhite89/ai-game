using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace ArtificialNeuralNetworks.Training
{
    public class Trainer : IDisposable
    {
        private IList<TrainingComponent> components;
        private bool isActive = false;
        private StreamWriter writer;

        public Trainer(string trainingFilePath) 
        {
            this.components = new List<TrainingComponent>();

            if (File.Exists(trainingFilePath))
                throw new UnityException("File " + trainingFilePath + " already exists.");
            
            this.writer = new StreamWriter(trainingFilePath);
        }

        public void StartTraining()
        {
            this.isActive = true;
        }

        public void StopTraining()
        {
            this.isActive = false;
        }

        public void PrintTraining()
        {
            if (this.isActive)
            {
                foreach (var com in this.components)
                    writer.Write(com.GetCurrentTraining().ToString("F6") + " ");
                writer.WriteLine();
            }
        }

        public void RegisterComponent(TrainingComponent component)
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
