using UnityEngine;
using System.Collections;

public class MapScript : MonoBehaviour {

    private LevelManager lvlMgr;

	// Use this for initialization
	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
        this.lvlMgr.StartNewLevel();
	}
	
	// Update is called once per frame
	void Update () {

	}

}
