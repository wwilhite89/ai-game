using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class AttackButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
	}

	void Update() {
		character = lvlMgr.ActiveCharacter;
	}

	public void OnClick() {
		character.GetComponent<CharacterController>().currentStatus = CharacterController.Status.ATTACKING;
	}
}
