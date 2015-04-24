using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {
    LevelManager levelManager;
    public GameObject manager;
    Canvas canvas;
	// Use this for initialization
	void Start () {
        levelManager = manager.GetComponent<LevelManager>();
        canvas = gameObject.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
        if (levelManager.ActiveCharacterCtrl == null)
        {
            this.canvas.enabled = false;
        }
        else
        {
            this.canvas.enabled = true;
        }
	}
}
