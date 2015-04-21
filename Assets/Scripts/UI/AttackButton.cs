using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class AttackButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
		gameObject.GetComponent<Button> ().interactable = false;
	}

	void Update() {
		character = lvlMgr.ActiveCharacter;
	}

	public void OnClick() {
		character.GetComponent<CharacterController>().character.status = Character.Status.ATTACKING;
	}

	public void buttonOn() {
		gameObject.GetComponent<Button> ().interactable = true;
	}
}
