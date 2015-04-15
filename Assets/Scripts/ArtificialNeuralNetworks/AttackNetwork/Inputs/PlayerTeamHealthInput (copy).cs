using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialNeuralNetworks.Training;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    public class PlayerTeamHealthInput : TrainingComponent
    {

		public PlayerTeamHealthInput(PlayerScript player, int cycleTime)
            : base(player, cycleTime)
        { }

        protected override void UpdateTraining()
        {
            if (this.player != null)
                this.newValue = this.player.GetStat(GameDB.Character.Stats.HP);
        }

    }
}
