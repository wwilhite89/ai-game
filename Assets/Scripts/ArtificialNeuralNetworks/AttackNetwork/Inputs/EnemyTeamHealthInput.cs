using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialNeuralNetworks.Core;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    public class EnemyTeamHealthInput : InputComponent
    {
        public EnemyTeamHealthInput(CharacterController controller) : base(controller) { }

        protected override void UpdateTraining()
        {
            var enemyTeam = controller.GetEnemyTeamMembers();

            if (enemyTeam == null)
                return;

            var maxHP = enemyTeam.Sum(x => x.GetMaxHP());
            var currentHP = enemyTeam.Sum(x => x.GetStat(GameDB.Character.Stats.HP));
            this.currentValue = (double)currentHP / (double)currentHP;
        }
    }
}
