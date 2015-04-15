using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class StatsDisplay : MonoBehaviour {
	public string prefix;
	public Character.Stats stat;
    private LevelManager lvlMgr;
    private Text text = null;

	// Use this for initialization
	void Start () {
        this.lvlMgr = LevelManager.getInstance();
        this.text = gameObject.GetComponent<Text>();
		// gameObject.guiText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        GameObject player = lvlMgr.ActivePlayer;
        var statVal = player != null ? player.GetComponent<PlayerScript>().GetStat(stat).ToString() : "";
        this.text.text = prefix + statVal;
	}
}
