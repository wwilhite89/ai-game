using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class StatsDisplay : MonoBehaviour {
	public string prefix;
	public Character.Stats stat;

	// Use this for initialization
	void Start () {
		gameObject.guiText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag("Selected");
		gameObject.GetComponent<Text>().text = prefix + player.GetComponent<PlayerScript>().GetStat(stat).ToString();
	}
}
