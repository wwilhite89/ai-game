using UnityEngine;
using System.Collections;

public class NavScript : MonoBehaviour {
	public GameObject gameManager;
	
	
	NavMeshAgent agent;
	
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameController");
		agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
		
	}
	
	public void moveAgent ( Vector3 loc ) {
		agent.SetDestination(loc);
	}
	/*
	void OnMouseDown () {
		gameManager.GetComponent<CharacterController> ().activePlayer = this.gameObject;
		
	}*/
}
