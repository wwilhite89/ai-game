using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class WaitButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;
    private Button btn;

	// Use this for initialization
	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
        this.btn = this.gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		character = lvlMgr.ActiveCharacter;

        this.setActive(lvlMgr.CurrentTurn == LevelManager.Turn.PLAYER && lvlMgr.ActiveCharacterCtrl != null 
            && (!lvlMgr.ActiveCharacterCtrl.HasMoved || !lvlMgr.ActiveCharacterCtrl.HasAttacked)
            && !lvlMgr.IsMoving());
	}

	public void OnClick() {

        var player = lvlMgr.ActiveCharacterCtrl;

        if (player != null && !player.HasMoved && !lvlMgr.IsMoving())
        {
            player.ForfeitMovement();
            player.UpdateEnemiesInRange();

            if (player.enemiesInRange.Length == 0)
            {
                lvlMgr.EndAttackSequence();
                player.ForfeitAttack();
            }
        }
        else if (player != null && !player.HasAttacked)
        {
            lvlMgr.EndAttackSequence();
            player.ForfeitAttack();
        }
	}

    private void setActive(bool active)
    {
        this.btn.image.color = active ? Color.white : Color.grey;
    }
}
