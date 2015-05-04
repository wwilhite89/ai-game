using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {
    LevelManager levelManager;
    public GameObject manager;
    public GameObject characterCard;
    public GameObject abilities;
	// Use this for initialization
	void Start () {
        levelManager = manager.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (levelManager.ActiveCharacter == null && levelManager.SelectedCharacter == null)
        {
            this.characterCard.SetActive(false);
            this.abilities.SetActive(false);
        }
        else
        {
            this.characterCard.SetActive(true);
            this.abilities.SetActive(true);
        }
	}
}
