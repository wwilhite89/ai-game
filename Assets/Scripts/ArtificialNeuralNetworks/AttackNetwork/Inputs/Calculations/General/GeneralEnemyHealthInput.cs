using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialNeuralNetworks.Core;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    /// <summary>
    /// Measures the enemies' health.
    /// </summary>
    public class GeneralEnemyHealthInput : InputComponent
    {
        public GeneralEnemyHealthInput(CharacterController controller, string description) : base(controller, description) { }

        protected override void UpdateTraining()
        {
            var enemyTeam = controller.GetEnemyTeamMembers();

            if (enemyTeam == null || enemyTeam.Count() == 0)
            {
                this.currentValue = 0;
                return;
            }

            var maxHP = enemyTeam.Sum(x => x.GetMaxHP());
            var currentHP = enemyTeam.Sum(x => x.GetStat(GameDB.Character.Stats.HP));
            this.currentValue = (double)currentHP / (double)maxHP;
        }
    }
}
