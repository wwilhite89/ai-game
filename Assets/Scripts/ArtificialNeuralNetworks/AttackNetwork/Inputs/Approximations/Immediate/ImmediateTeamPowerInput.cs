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
    /// Approximates the confidence of the current player's team with its nearby enemies.
    /// Confidence: 33% numbers, 33% relative strength, 33% relative defense
    /// </summary>
    public class ImmediateTeamPowerInput : InputComponent
    {
        public ImmediateTeamPowerInput(CharacterController controller, string description) : base(controller, description) { }

        protected override void UpdateTraining()
        {
            var enemies = controller.GetEnemyTeamMembers();

            if (enemies == null || enemies.Count() == 0)
            {
                this.currentValue = 1;
                return;
            }

            var avgEnemyRange = enemies.Average(x => x.GetStat(Character.Stats.MOV) + x.GetStat(Character.Stats.RANGE));
            var enemiesInRange = enemies.Where(x =>
                Mathf.Abs(Vector3.Distance(controller.gameObject.transform.position, x.gameObject.transform.position)) <= avgEnemyRange);

            if (enemiesInRange == null || enemiesInRange.Count() == 0)
            {
                this.currentValue = 1;
                return;
            }

            var teammates = controller.GetTeamMembers();
            var avgTeamRange = teammates.Average(x => x.GetStat(Character.Stats.MOV) + x.GetStat(Character.Stats.RANGE));
            var teammatesInRange = teammates.Where(x =>
                Mathf.Abs(Vector3.Distance(controller.gameObject.transform.position, x.gameObject.transform.position)) <= avgTeamRange);

            // Calculate tiers of confidence (might need to be tweaked)
            var strDefRatio = this.getRatio(enemiesInRange, teammatesInRange.Average(x => x.GetStat(Character.Stats.ATT)), Character.Stats.DEF);
            var attackingConfidence = strDefRatio < 1 ? 0 : (strDefRatio < 2 ? 0.5 : (strDefRatio < 3 ? 0.75 : 1.00));

            var defStrRatio = this.getRatio(enemiesInRange, teammatesInRange.Average(x => x.GetStat(Character.Stats.DEF)), Character.Stats.ATT);
            var defensiveConfidence = defStrRatio >= 1 ? 1 : (defStrRatio >= 0.75 ? 0.75 : (defStrRatio >= 0.5 ? 0.5 : (defStrRatio >= 0.25 ? 0.25 : 0.00)));

            var numbersConfidence = enemiesInRange.Count() > teammatesInRange.Count() ? 0 : (enemiesInRange.Count() < teammatesInRange.Count() ? 1 : 0.5);

            this.currentValue = (1.00 / 3.00) * attackingConfidence + (1.00 / 3.00) * defensiveConfidence + (1.00 / 3.00) * numbersConfidence;
        }


        private double getRatio(IEnumerable<CharacterController> enemies, double characterAvg, Character.Stats enemyStat)
        {
            var avgEnemyStat = enemies.Average(x => x.GetStat(enemyStat));
            return characterAvg / avgEnemyStat;
        }
    }
}
