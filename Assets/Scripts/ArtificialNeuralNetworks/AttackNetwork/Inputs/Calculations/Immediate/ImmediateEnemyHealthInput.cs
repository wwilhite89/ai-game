using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDB;
using UnityEngine;
using ArtificialNeuralNetworks.Core;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    /// <summary>
    /// Measures the nearby enemies' health.
    /// </summary>
    public class ImmediateEnemyHealthInput : InputComponent
    {
        public ImmediateEnemyHealthInput(CharacterController controller, string description) : base(controller, description) { }

        protected override void UpdateTraining()
        {
            var enemyTeam = controller.GetEnemyTeamMembers();

            if (enemyTeam == null || enemyTeam.Count() == 0)
            {
                this.currentValue = 0;
                return;
            }

            var avgTeamRange = enemyTeam.Average(x => x.GetStat(Character.Stats.MOV) + x.GetStat(Character.Stats.RANGE));
            var enemiesInRange = enemyTeam.Where(x =>
                Mathf.Abs(Vector3.Distance(controller.gameObject.transform.position, x.gameObject.transform.position)) <= avgTeamRange);

            if (enemiesInRange == null || enemiesInRange.Count() == 0)
            {
                this.currentValue = 0;
                return;
            }

            var maxHP = enemiesInRange.Sum(x => x.GetMaxHP());
            var currentHP = enemiesInRange.Sum(x => x.GetStat(GameDB.Character.Stats.HP));
            this.currentValue = (double)currentHP / (double)maxHP;
        }
    }
}
