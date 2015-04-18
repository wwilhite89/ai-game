using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ArtificialNeuralNetworks.Training;
using ArtificialNeuralNetworks.AttackNetwork.Inputs;

namespace ArtificialNeuralNetworks.AttackNetwork
{
    class PlayerTraining : MonoBehaviour
    {
        public string FilePath = "..\\AttackNetworkTraining.txt";
        public int CycleUnits = 5;
        public CharacterController player;

        private Trainer trainer;
        private int cycle;

        void Start()
        {
            // Create training
            this.trainer = new Trainer(FilePath);

            // Register components
            TrainingComponent health = new PlayerHealthInput(player, CycleUnits);
            this.trainer.RegisterComponent(health);

            // Start training
            this.trainer.StartTraining();
        }

        void Update()
        {
            if (++this.cycle % CycleUnits == 0)
            {
                this.trainer.PrintTraining();
                this.cycle = 0;
            }
        }

        void OnApplication()
        {
            this.trainer.StopTraining();
        }
    }
}
