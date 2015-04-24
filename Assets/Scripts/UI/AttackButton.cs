using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class AttackButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;
	CharacterController characterCtrl;
	private bool engaged;
	Button b;
	ColorBlock normalColor;
	public ColorBlock engagedColor;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
		this.character = null;
		this.engaged = false;
		this.b = this.gameObject.GetComponent<Button>();
		this.normalColor = b.colors;
	}

	void Update() {
		if(lvlMgr.ActiveCharacter != null) { 
			if(!lvlMgr.ActiveCharacter.Equals(character)) {
				this.character = lvlMgr.ActiveCharacter;
				this.characterCtrl = character.GetComponent<CharacterController>();
			}
			if(this.characterCtrl != null) {
				if(this.characterCtrl.character.status == Character.Status.READY) {
					this.ChangeColor(Color.white);
					lvlMgr.resetCharColor();
				} else if(this.characterCtrl.character.status == Character.Status.ATTACKING) {
					this.ChangeColor(Color.red);
				}
			}
		}
	}

	public void OnClick() {
		if(this.engaged) {
			this.engaged= false;
			this.characterCtrl.character.status = Character.Status.READY;
		} else if(this.character != null && !this.characterCtrl.HasAttacked) {
			this.engaged = true;
			this.characterCtrl.character.status = Character.Status.ATTACKING;
		}
	}

	private void ChangeColor(Color c) {
		this.b.image.color = c;
	}
}