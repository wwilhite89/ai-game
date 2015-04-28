using UnityEngine;
using System.Collections;

public class GuideScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		const int buttonWidth = 60;
		const int buttonHeight = 20;

		Rect buttonRect = new Rect (Screen.width/2, Screen.height/3, buttonWidth, buttonHeight);

		if (GUI.Button (buttonRect, "Return")) {
			Application.LoadLevel("MenuScene");
		}

	}
}
