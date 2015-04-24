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
        CharacterController character = lvlMgr.SelectedCharacterCtrl;
        var statVal = character != null ? character.GetStat(stat).ToString() : "";
        this.text.text = prefix + statVal;
	}
}
