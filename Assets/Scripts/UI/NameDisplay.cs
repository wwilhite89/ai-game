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
        CharacterController character = lvlMgr.SelectedCharacterCtrl;
        var statVal = character != null ? character.character.Name : "";
		this.text.text = statVal;
	}
}
