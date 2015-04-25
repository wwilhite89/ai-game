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
            var maxHP = controller.GetMaxHP();
            var currentHP = controller.GetStat(GameDB.Character.Stats.HP);
            this.currentValue = (double)currentHP / (double)maxHP;
        }
    }
}
