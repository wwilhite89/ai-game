using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameDB;

namespace ArtificialNeuralNetworks.Core
{
    public abstract class InputComponent
    {
        protected double currentValue;
        protected CharacterController controller;

        public InputComponent(CharacterController controller)
        {
            this.controller = controller;
        }

        protected abstract void UpdateTraining();

        public double GetCurrentTraining()
        {
            this.UpdateTraining();
            return this.currentValue;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
