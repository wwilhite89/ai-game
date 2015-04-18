using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class StatsDisplay : MonoBehaviour {
	public string prefix;
	public Character.Stats stat;
    private LevelManager lvlMgr;
    private Text text = null;

	void Start () {
        this.lvlMgr = LevelManager.getInstance();
        this.text = gameObject.GetComponent<Text>();
	}
	
	void Update () {
        GameObject character = lvlMgr.ActiveCharacter;
        var statVal = character != null ? character.GetComponent<CharacterController>().GetStat(stat).ToString() : "";
        this.text.text = prefix + statVal;
	}
}
