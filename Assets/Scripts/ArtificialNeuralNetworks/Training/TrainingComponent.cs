using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameDB;

namespace ArtificialNeuralNetworks.Training
{
    public abstract class TrainingComponent : MonoBehaviour
    {
        private double currentValue;
        private int cycleTime;
        private int currentCycle;

        protected CharacterController player;
        protected double newValue;

        public TrainingComponent(CharacterController player, int cycleTime)
        {
            this.cycleTime = cycleTime;
            this.player = player;
        }

        void Update() {
            if (++this.currentCycle % this.cycleTime == 0)
            {
                this.currentValue = this.newValue;
                this.currentCycle = 0;
            }
        }

        protected abstract void UpdateTraining();

        public double GetCurrentTraining()
        {
            return this.currentValue;
        }
    }
}
