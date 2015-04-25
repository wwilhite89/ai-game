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
    /// Measures the current player's nearby team health.
    /// </summary>
    public class ImmediateTeamHealthInput : InputComponent
    {
        public ImmediateTeamHealthInput(CharacterController controller, string description) : base(controller, description) { }

        protected override void UpdateTraining()
        {
            var teammates = controller.GetTeamMembers().Where(x => x != controller);

            if (teammates == null || teammates.Count() == 0)
            {
                this.currentValue = 0;
                return;
            }

            var avgTeamRange = teammates.Average(x => x.GetStat(Character.Stats.MOV) + x.GetStat(Character.Stats.RANGE));
            var teammatesInRange = teammates.Where(x =>
                Mathf.Abs(Vector3.Distance(controller.gameObject.transform.position, x.gameObject.transform.position)) <= avgTeamRange);

            if (teammatesInRange == null || teammatesInRange.Count() == 0)
            {
                this.currentValue = 0;
                return;

            }

            var maxHP = teammatesInRange.Sum(x => x.GetMaxHP());
            var currentHP = teammatesInRange.Sum(x => x.GetStat(GameDB.Character.Stats.HP));
            this.currentValue = (double)currentHP / (double)maxHP;
        }
    }
}
