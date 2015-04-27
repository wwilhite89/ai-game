using UnityEngine;
using System.Collections;
using ArtificialNeuralNetworks.AttackNetwork;

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
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AttackNetwork.DECISION.ATTACK_CLOSEST);
	}
	public void Run( ) {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AttackNetwork.DECISION.RUN);
	}

	public void RunClose() {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AttackNetwork.DECISION.RUN);
	}
	public void AttWeak( ) {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AttackNetwork.DECISION.ATTACK_WEAKEST);
	}

	public void AttWeakInRange() {
		LvlMgr.GetComponent<LevelManager> ().manageAIMove (AttackNetwork.DECISION.ATTACK_WEAKEST_IN_RANGE);
	}
}
