using UnityEngine;
using System.Collections;

public class ParentPlayerScript : MonoBehaviour {

	public GameObject[] humans;
	public GameObject[] ai;

	// Use this for initialization
	void Start () {

		humans = GameObject.FindGameObjectsWithTag ("Player");
		ai = GameObject.FindGameObjectsWithTag ("Enemy");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void resetActive() {
		for (int i = 0; i < humans.Length; i++) {
			humans [i].GetComponent<PlayerScript> ().isActive = 0;
		}
	}
}
