using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using GameDB;
using GameDB.SessionData;
using UnityEngine;

public class BattleManager
{
    private LevelManager lvlMgr;

    #region Constructors

    public BattleManager()
    {
        this.lvlMgr = LevelManager.getInstance();
    }

    #endregion

    public void DoBattle(PlayerScript attacker, PlayerScript attackee)
    {
        System.Random r = new System.Random();
        
        // Determine if we hit (EVA)
        bool evaded = r.NextDouble() <= attackee.GetStat(Character.Stats.EVA);

        if (evaded)
        {
            Debug.Log("Evaded!");
            return;
        }

        // Determine if there's a critical (CRIT)
        var critVal = r.NextDouble() <= attacker.GetStat(Character.Stats.CRIT) ? 3 : 1;

        if (critVal > 1)
            Debug.Log("Critical hit!");

        // Determine damage
        var dmg = (attacker.GetStat(Character.Stats.ATT) * critVal) - attackee.GetStat(Character.Stats.DEF);
        Debug.Log("Dealing " + dmg + " points of dmg.");
        
        // Apply damage
        this.applyDamage(attackee, (int) dmg);

    }

    private void applyDamage(PlayerScript player, int amount)
    {
        var newHP = player.GetStat(Character.Stats.HP) - amount;

        // Update health
        player.UpdateStat(Character.Stats.HP, Math.Max(0, newHP));

        // Death
        if (newHP <= 0)
            this.lvlMgr.Kill(player.gameObject);
    }



}