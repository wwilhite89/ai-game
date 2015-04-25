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
            var team = controller.GetTeamMembers();

            if (team == null)
                return;

            var maxHP = team.Sum(x => x.GetMaxHP());
            var currentHP = team.Sum(x => x.GetStat(GameDB.Character.Stats.HP));
            this.currentValue = (double)currentHP / (double)currentHP;
        }
    }
}
