using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialNeuralNetworks.Core;
using GameDB;
using UnityEngine;

namespace ArtificialNeuralNetworks.AttackNetwork.Inputs
{
    /// <summary>
    /// Approximates the confidence of the current player with its nearby enemies.
    /// Confidence: 33% numbers, 33% relative strength, 33% relative defense
    /// </summary>
    public class ImmediatePlayerPowerInput : InputComponent
    {
        public ImmediatePlayerPowerInput(CharacterController controller, string description) : base(controller, description) { }

        protected override void UpdateTraining()
        {
            var enemies = controller.GetEnemyTeamMembers();
            var avgEnemyRange = enemies.Average(x => x.GetStat(Character.Stats.MOV) + x.GetStat(Character.Stats.RANGE));
            var enemiesInRange = enemies.Where(x =>
                Mathf.Abs(Vector3.Distance(controller.gameObject.transform.position, x.gameObject.transform.position)) <= avgEnemyRange);

            // Only care about the immediate threat
            if (enemiesInRange.Count() == 0) 
            {
                this.currentValue = 1;
                return;
            }

            // Calculate tiers of confidence (might need to be tweaked)
            var strDefRatio = this.getRatio(enemiesInRange, Character.Stats.ATT, Character.Stats.DEF);
            var attackingConfidence = strDefRatio < 1 ? 0 : (strDefRatio < 2 ? 0.5 : (strDefRatio < 3 ? 0.75 : 1.00));

            var defStrRatio = this.getRatio(enemiesInRange, Character.Stats.DEF, Character.Stats.ATT);
            var defensiveConfidence = defStrRatio >= 1 ? 1 : (defStrRatio >= 0.75 ? 0.75 : (defStrRatio >= 0.5 ? 0.5 : (defStrRatio >= 0.25 ? 0.25 : 0.00)));

            var teammates = controller.GetTeamMembers().Where(x => x != controller);
            var avgTeamRange = teammates.Average(x => x.GetStat(Character.Stats.MOV) + x.GetStat(Character.Stats.RANGE));
            var teammatesInRange = teammates.Where(x =>
                Mathf.Abs(Vector3.Distance(controller.gameObject.transform.position, x.gameObject.transform.position)) <= avgTeamRange);
            var numbersConfidence = enemiesInRange.Count() > (teammatesInRange.Count()+1) ? 0 : (enemiesInRange.Count() < (teammatesInRange.Count()+1) ? 1 : 0.5);

            this.currentValue = (1.00 / 3.00) * attackingConfidence + (1.00 / 3.00) * defensiveConfidence + (1.00 / 3.00) * numbersConfidence;
        }


        private double getRatio(IEnumerable<CharacterController> enemies, Character.Stats characterStat, Character.Stats enemyStat)
        {
            var avgEnemyStat = enemies.Average(x => x.GetStat(enemyStat));
            return controller.GetStat(characterStat) / avgEnemyStat;
        }

    }
}
