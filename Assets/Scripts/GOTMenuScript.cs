using UnityEngine;
using System.Collections;

public class GOTMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		const int buttonWidth = 110;
		const int buttonHeight = 60;
		
		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect buttonRect1 = new Rect(
			Screen.width / 2 + 2 * buttonWidth,
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			);

		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect buttonRect2 = new Rect(
			Screen.width / 4 - (buttonWidth / 4),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			);
		
		// Draw a button to start the game
		if(GUI.Button(buttonRect1,"House Stark"))
		{
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("AdamScene2");
		}

		// Draw a button to start the game
		if(GUI.Button(buttonRect2,"House Lannister"))
		{
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("AdamScene");
		}
	}
}
