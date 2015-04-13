using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay : MonoBehaviour {
	Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag("Selected");
		image.sprite = player.GetComponentInChildren<SpriteRenderer>().sprite;
		image.SetNativeSize();
	}
}
