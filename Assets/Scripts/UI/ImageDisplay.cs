using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay : MonoBehaviour {
	Image image;
    private LevelManager lvlMgr;

	void Start () {
		image = GetComponent<Image>();
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
	}
	
	void Update () {
        GameObject player = lvlMgr.SelectedCharacter;

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
