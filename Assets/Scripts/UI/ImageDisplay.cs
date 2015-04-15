using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay : MonoBehaviour {
	Image image;
    private LevelManager lvlMgr;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
        this.lvlMgr = LevelManager.getInstance();
	}
	
	// Update is called once per frame
	void Update () {
        GameObject player = lvlMgr.ActivePlayer;

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
