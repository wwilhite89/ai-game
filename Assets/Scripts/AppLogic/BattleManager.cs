using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using GameDB;
using GameDB.SessionData;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    private LevelManager lvlMgr;
    private Queue<BattleMessage> battleMsgQueue = new Queue<BattleMessage>();
    private Rect battleBanner;
    private bool initialized;
    private GUIStyle style;
    private string currentMessage;
    private Color currentColor;

	void Start() {
        this.lvlMgr = gameObject.GetComponent<LevelManager>();
        this.battleBanner = new Rect(0, -50 + Screen.height / 2, Screen.width, 100);
	}

    public void DoBattle(CharacterController attacker, CharacterController attackee)
    {
        System.Random r = new System.Random();
        
        // Determine if we hit (EVA)
        bool evaded = r.NextDouble() <= attackee.GetStat(Character.Stats.EVA);

        if (evaded)
        {
            this.EnqueueBattleMessage(string.Format("{0} dodges {1}'s attack!", attackee.GetCharacterName(), attacker.GetCharacterName()), Color.yellow);
            return;
        }

        string msg = "";

        // Determine if there's a critical (CRIT)
        var critVal = r.NextDouble() <= attacker.GetStat(Character.Stats.CRIT) ? 3 : 1;

        if (critVal > 1)
            msg += "Critical hit! ";

        // Determine damage
        var dmg = (int) Mathf.Max(0, (attacker.GetStat(Character.Stats.ATT) * critVal) - attackee.GetStat(Character.Stats.DEF));
        msg += string.Format("{0} dealt {1} points of damage to {2}", attacker.GetCharacterName(), dmg, attackee.GetCharacterName());
        this.EnqueueBattleMessage(msg, Color.red);
        
        // Apply damage
        this.applyDamage(attackee, (int) dmg);
    }

    public void EnqueueBattleMessage(string message, Color color)
    {
        this.battleMsgQueue.Enqueue(new BattleMessage { content = message, color = color });
    }

    private void applyDamage(CharacterController player, int amount)
    {
        var newHP = player.GetStat(Character.Stats.HP) - amount;

        // Update health
        player.UpdateStat(Character.Stats.HP, Math.Max(0, newHP));

        // Death
        if (newHP <= 0)
        {
            this.EnqueueBattleMessage(string.Format("{0} died!", player.GetCharacterName()), Color.red);
            this.lvlMgr.Kill(player.gameObject);
        }
    }

    internal class BattleMessage
    {
        public string content;
        public Color color;
    }

    void Update()
    {
        if (this.battleMsgQueue.Count() > 0 && this.currentMessage == null)
        {
            var nextMsg = this.battleMsgQueue.Dequeue();
            this.currentMessage = nextMsg.content;
            this.currentColor = nextMsg.color;
        }
    }

    void OnGUI()
    {
        if (!this.initialized)
        {
            style = GUI.skin.box;
            style.fontSize = 30;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.alignment = TextAnchor.MiddleCenter;
            this.initialized = true;
        }

        if (this.currentMessage != null)
            StartCoroutine(this.emptyMessageQueue());
    }
    
    private IEnumerator emptyMessageQueue()
    {
        Color temp = GUI.contentColor;
        GUI.contentColor = this.currentColor;
        GUI.Box(this.battleBanner, this.currentMessage, this.style);
        
        yield return new WaitForSeconds(1.5f);
        
        this.currentMessage = null;
        GUI.contentColor = temp;
        
        yield return new WaitForSeconds(0);
    }

}