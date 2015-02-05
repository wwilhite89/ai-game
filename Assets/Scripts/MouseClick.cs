using UnityEngine;
using System.Collections;
using System;

public class MouseClick : MonoBehaviour {

	Transform player;
	private Vector3 blockPossition;

	// Use this for initialization
	void Start () {
	
	}

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		blockPossition = gameObject.transform.position;
		blockPossition.y += 1;
		player.position = blockPossition;
	}
}
