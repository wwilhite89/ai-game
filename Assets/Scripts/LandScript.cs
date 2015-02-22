using UnityEngine;
using System.Collections;

public class LandScript : MonoBehaviour {
	public string LandType;

	// Modifies movement speed of characters. 1 is normal, 0 is unwalkable.
	public float speed;
	private Color landColor;

	// Use this for initialization
	void Start () {
		landColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver() {
		renderer.material.color = new Color(255, renderer.material.color.g, renderer.material.color.b);
	} 

	void OnMouseExit ()
	{
		renderer.material.color = landColor;
	}
}
