using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class AttackButton : MonoBehaviour {
    
    private Button btn;
    private LevelManager lvlMgr;
    private CharacterController currentPlayer = null;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
		this.btn = this.gameObject.GetComponent<Button>();
        this.setActive(false);
	}

	void Update() {
        
        if (!lvlMgr.IsAttacking())
            this.updateWithSelectedCharacter();

	}

	public void OnClick() {
        if (!lvlMgr.IsAttacking())
            lvlMgr.BeginAttackSequence();
	}

	private void setActive(bool active) {
        this.btn.image.color = active ? Color.white : Color.grey;
	}

    private void updateWithSelectedCharacter()
    {
        // Update the current player
        this.currentPlayer = lvlMgr.ActiveCharacterCtrl;
        var playerSelected = this.currentPlayer != null;

        // Update button's active status
        this.setActive(playerSelected && !this.currentPlayer.HasAttacked);
    }
}