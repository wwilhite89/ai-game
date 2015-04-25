using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialNeuralNetworks.Core;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    public class PlayerTeamHealthInput : InputComponent
    {
        public PlayerTeamHealthInput(CharacterController controller) : base(controller) { }

        protected override void UpdateTraining()
        {
            this.currentValue = 0;
        }
    }
}
