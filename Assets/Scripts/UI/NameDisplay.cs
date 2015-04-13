using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text>().text = "";
	}
	
	// Update is called once per frame
	void Update () {
		GameObject character = GameObject.FindGameObjectWithTag("Selected");
		GetComponent<Text>().text = character.name;
	}
}
