using UnityEngine;
using System.Collections;
using System;

public class MouseClick : MonoBehaviour {

	Transform player;
	private Vector3 blockPossition;
	private float distx = .0f;
	private float distz = .0f;

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

		distx = player.position.x - blockPossition.x;
		distz = player.position.z - blockPossition.z;
		player.position = blockPossition;
	}

	void OnMouseOver() {

	}
}
