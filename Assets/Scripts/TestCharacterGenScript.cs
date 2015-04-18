using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDB;

public class TestCharacterGenScript : MonoBehaviour {

    private IList<GameObject> characters = new List<GameObject>();

	// Use this for initialization
	void Start () {
        var jaime = new Character { Name = "Jaime", resourcePath = @"Characters/JaimeLannister" };
        spawnCharacters(new List<Character> { jaime });
	}

    // Input should be teams or charcaters
    private void spawnCharacters(IList<Character> characters)
    {
        // Try instatiating some players
        foreach (var c in characters)
        {
            if (c.resourcePath != null)
            {
                var resource = Resources.Load(c.resourcePath) as GameObject;
                var newChar = Instantiate(resource, new Vector3(-4, 1, 0), resource.transform.rotation) as GameObject;
                this.characters.Add(newChar);
            }
        }
    }
}