using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameDisplay : MonoBehaviour {
    private LevelManager lvlMgr;
	private Text text;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
		this.text = gameObject.GetComponent<Text>();
	}
	
	void Update () {
		//TODO (wil) view name of enemies as well
        GameObject character = lvlMgr.ActiveCharacter;
        var statVal = character != null ? character.GetComponent<CharacterController>().character.Name : "";
		this.text.text = statVal;
	}
}
