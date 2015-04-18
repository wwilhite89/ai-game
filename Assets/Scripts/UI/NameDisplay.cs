using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameDisplay : MonoBehaviour {
    private LevelManager lvlMgr;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
		GetComponent<Text>().text = "";
	}
	
	void Update () {
        GameObject character = lvlMgr.ActiveCharacter;
        var statVal = character != null ? character.GetComponent<CharacterController>().name.ToString() : "";
		GetComponent<Text>().text = character.name;
	}
}
