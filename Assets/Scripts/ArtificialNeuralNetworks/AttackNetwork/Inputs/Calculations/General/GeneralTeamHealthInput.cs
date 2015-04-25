using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ArtificialNeuralNetworks.Core;
using GameDB;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    /// <summary>
    /// Measures the current player's team health.
    /// </summary>
    public class GeneralTeamHealthInput : InputComponent
    {
        public GeneralTeamHealthInput(CharacterController controller, string description) : base(controller, description) { }

        protected override void UpdateTraining()
        {
            var teammates = controller.GetTeamMembers();
            var maxHP = teammates.Sum(x => x.GetMaxHP());
            var currentHP = teammates.Sum(x => x.GetStat(GameDB.Character.Stats.HP));
            this.currentValue = (double)currentHP / (double)maxHP;
        }
    }
}
