using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class StatsDisplay : MonoBehaviour {
	private string prefix;
	public Character.Stats stat;
    private LevelManager lvlMgr;
    private Text text;

	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
        this.text = gameObject.GetComponent<Text>();
		this.prefix = stat.ToString() + ": ";
	}
	
	void Update () {
		//TODO (wil) display stats of enemy characters as well
        GameObject character = lvlMgr.ActiveCharacter;
        var statVal = character != null ? character.GetComponent<CharacterController>().GetStat(stat).ToString() : "";
        this.text.text = prefix + statVal;
	}
}
