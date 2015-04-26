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
	public void Run( ) {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AIChoiceScript.Decision.RUN);
	}

	public void RunClose() {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AIChoiceScript.Decision.RUN);
	}
	public void AttWeak( ) {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AIChoiceScript.Decision.ATTWEAK);
	}

	public void AttWeakInRange() {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AIChoiceScript.Decision.ATTWEAK_INRANGE);
	}
}
