using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDB;

public class SpawnPointScript : MonoBehaviour {

    // Spawn override properties
    public bool OverrideSpawn = false;
    public Sprite SpriteOverride;
    public string OvName = "Test Player";
    public int OvHP = 1000;
    public int OvATT = 250;
    public int OvDEF = 100;
    public float OvEVA = 0.5f;
    public float OvCRIT = 0.10f;
    public int OvMOV = 6;
    public int OvRANGE = 3;

	// Use this for initialization
	void Start () {
	}
	
    // Update is called once per frame
	void Update () {

	}

    public void SpawnPlayer(PlayerScript script, Character player)
    {
        var renderer = GetComponentInChildren<SpriteRenderer>();
        script.SetCharacter(player);

        if (this.OverrideSpawn)
        {
            Debug.Log("Overriding spawn for character: " + player.Name);

            // Override stats
            player.Name = OvName;
            player.attack = OvATT;
            player.defense = OvDEF;
            player.evade = OvEVA;
            player.critical = OvCRIT;
            player.movement = OvMOV;
            player.range = OvRANGE;

            // Override sprite
            renderer.sprite = this.SpriteOverride;
        }
        else
        {
            Debug.Log("Spawning " + player.Name);
            var sprite = Resources.Load<Sprite>(@"Players/" + player.resourcePath);
            renderer.sprite = sprite;


        }
    }
}
