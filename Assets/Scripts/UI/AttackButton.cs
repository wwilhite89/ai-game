using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class AttackButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;
	GameObject opponent;
	bool selected = false;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
	}
	
	void Update () {
        this.character = lvlMgr.ActiveCharacter;
		if(selected) {
			GameObject[] opponents = character.GetComponent<CharacterController>().enemiesInRange;
			foreach(GameObject opponent in opponents) {
				if(opponent.GetComponent<CharacterController>().selected) {
					character.GetComponent<CharacterController>().attackPrompt(opponent);
					selected = false;
				}
			}
		}
	}

	public void OnClick() {
		//TODO (wil) make this better
		if(selected) {
			selected = false;
		} else {
			selected = true;
		}
	}
}
