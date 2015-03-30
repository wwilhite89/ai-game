using UnityEngine;
using System.Collections;
using GameDB;

public class SpawnPointScript : MonoBehaviour {

    public enum SpawnCharacter
    { 
        EddardStark,
        JamieLannister
    }

    public bool ManualSpawn = false;
    public SpawnCharacter SpawnCharacterOverride;
    public Sprite SpriteOverride;

	// Use this for initialization
	void Start () {

        if (this.ManualSpawn)
            this.spawnCharacter();
	}

    private void spawnCharacter()
    {
        if (this.SpriteOverride == null)
            throw new UnityException("SpriteOverride is null.");

        var gameDb = new Database();
        Character character = null;

        switch (this.SpawnCharacterOverride)
        {
            case SpawnCharacter.EddardStark:
                character = gameDb.GetInitialCharacter("Eddard Stark");
                break;
            case SpawnCharacter.JamieLannister:
                character = gameDb.GetInitialCharacter("Jaime Lannister");
                break;
            default:
                break;
        }

        if (character == null)
            throw new UnityException("Cannot load character.");
        Debug.Log(character.SavedGameId + " loaded");
        // Get the resource
        var resource = Resources.Load(@"Players/BasePlayer") as GameObject;
        // Create the player
        var player = Instantiate(resource, this.transform.position, resource.transform.rotation) as GameObject;
        var renderer = player.GetComponentInChildren<SpriteRenderer>();
        renderer.sprite = this.SpriteOverride;

        // Remove the spawn point
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {

	}


}
