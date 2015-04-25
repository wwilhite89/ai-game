using UnityEngine;
using System.Collections;

public class TrainingScript : MonoBehaviour {
	
	public GameObject LvlMgr;
	// Use this for initialization
	void Start () {
		LvlMgr = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AttClose( ) {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AIChoiceScript.Decision.ATTCLOSE);
	}

	public void RunClose() {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AIChoiceScript.Decision.RUN);
	}
}
