using UnityEngine;
using System.Collections;

public class TrainingScript : MonoBehaviour {
	
	public GameObject LvlMgr;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AttClose( ) {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (CharacterController.Decision.ATTCLOSE);
	}
}
