using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextTurnScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            StartCoroutine(wait());
        }
	}

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }
}