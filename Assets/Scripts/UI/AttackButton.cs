using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class AttackButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;
	private bool engaged;
	Button b;
	ColorBlock normalColor;
	public ColorBlock engagedColor;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
		this.character = null;
		this.engaged = false;
		this.b = gameObject.GetComponent<Button>();
		this.normalColor = b.colors;
	}

	void Update() {
		if(lvlMgr.ActiveCharacter != null) {
			this.character = lvlMgr.ActiveCharacter;
		}
	}

	public void OnClick() {
		if(this.engaged) {
			this.engaged= false;
			this.ChangeColor(normalColor);
		} else if(this.character != null) {
			this.character.GetComponent<CharacterController>().character.status = Character.Status.ATTACKING;
			this.ChangeColor(engagedColor);
			this.engaged = true;
		}
	}

	private void ChangeColor(ColorBlock cb) {
		b.colors = cb;
	}
}
