using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay : MonoBehaviour {
	Image image;
    private LevelManager lvlMgr;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		//TODO (wil) view image of enemies as well
        GameObject player = lvlMgr.ActiveCharacter;

        if (player != null)
        {
            image.sprite = player.GetComponentInChildren<SpriteRenderer>().sprite;
            image.SetNativeSize();
        }
        else
        {
            image.sprite = null;
        }
	}
}
