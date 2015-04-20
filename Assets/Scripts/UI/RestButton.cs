using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class RestButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;

	// Use this for initialization
	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		character = lvlMgr.ActiveCharacter;
	}

	public void OnClick() {
		character.GetComponent<CharacterController>().character.status = Character.Status.RESTING;
	}
}
