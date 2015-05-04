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
    /// Approximates the confidence of the current player with its enemies.
    /// Confidence: 33% numbers, 33% relative strength, 33% relative defense
    /// </summary>
    public class GeneralPlayerPowerInput : InputComponent
    {
        public GeneralPlayerPowerInput(CharacterController controller, string description) : base(controller, description) { }

        protected override void UpdateTraining()
        {
            var enemies = controller.GetEnemyTeamMembers();

            if (enemies.Count() == 0) 
            {
                this.currentValue = 1;
                return;
            }

            // Calculate tiers of confidence (might need to be tweaked)
            var strDefRatio = this.getRatio(enemies, Character.Stats.ATT, Character.Stats.DEF);
            var attackingConfidence = strDefRatio < 1 ? 0 : (strDefRatio < 2 ? 0.5 : (strDefRatio < 3 ? 0.75 : 1.00));

            var defStrRatio = this.getRatio(enemies, Character.Stats.DEF, Character.Stats.ATT);
            var defensiveConfidence = defStrRatio >= 1 ? 1 : (defStrRatio >= 0.75 ? 0.75 : (defStrRatio >= 0.5 ? 0.5 : (defStrRatio >= 0.25 ? 0.25 : 0.00)));

            var teammates = controller.GetTeamMembers();

            var numbersConfidence = enemies.Count() > (teammates.Count() + 1) ? 0 : (enemies.Count() < (teammates.Count() + 1) ? 1 : 0.5);

            this.currentValue = (1.00 / 3.00) * attackingConfidence + (1.00 / 3.00) * defensiveConfidence + (1.00 / 3.00) * numbersConfidence;
        }


        private double getRatio(IEnumerable<CharacterController> enemies, Character.Stats characterStat, Character.Stats enemyStat)
        {
            var avgEnemyStat = enemies.Average(x => x.GetStat(enemyStat));
            return controller.GetStat(characterStat) / avgEnemyStat;
        }

    }
}
