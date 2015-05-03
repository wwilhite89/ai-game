using UnityEngine;
using System.Collections;

public class NavScript : MonoBehaviour {
	public LevelManager levelManager;
	
	
	NavMeshAgent agent;
	
	void Start () {
        GameObject manager = GameObject.Find("Manager");
        this.levelManager = manager.GetComponent<LevelManager>();
		agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
		
	}
	
	public void moveAgent ( Vector3 loc ) {
        agent.SetDestination(loc);
        this.levelManager.SetMovementAgent(this);
	}

    public bool IsMoving()
    {
        var distance = this.agent.remainingDistance;

        if (distance == Mathf.Infinity)
            return false;

        return agent.pathStatus != NavMeshPathStatus.PathComplete
            || agent.remainingDistance > 0;
    }

	/*
	void OnMouseDown () {
		gameManager.GetComponent<CharacterController> ().activePlayer = this.gameObject;
		
	}*/
}
