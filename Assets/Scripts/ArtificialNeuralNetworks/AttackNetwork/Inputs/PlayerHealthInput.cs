using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialNeuralNetworks.Core;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    public class PlayerHealthInput : InputComponent
    {
        public PlayerHealthInput(CharacterController controller) : base(controller) { }

        protected override void UpdateTraining()
        {
            this.currentValue = controller.GetStat(GameDB.Character.Stats.HP);
        }
    }
}
