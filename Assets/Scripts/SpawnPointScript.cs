using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDB;

public class SpawnPointScript : MonoBehaviour {

    // Spawn override properties
    public bool OverrideSpawn = false;
    public Sprite SpriteOverride;
    public string OvName = "Test Character";
    public int OvHP = 1000;
    public int OvATT = 250;
    public int OvDEF = 100;
    public float OvEVA = 0.5f;
    public float OvCRIT = 0.10f;
    public int OvMOV = 6;
    public int OvRANGE = 3;

    public void SpawnCharacter(CharacterController script, Character character)
    {
        var renderer = GetComponentInChildren<SpriteRenderer>();
        script.SetCharacter(character);

        if (this.OverrideSpawn)
        {
            Debug.Log("Overriding spawn for character: " + character.Name);

            // Override stats
            character.Name = OvName;
            character.attack = OvATT;
            character.defense = OvDEF;
            character.evade = OvEVA;
            character.critical = OvCRIT;
            character.movement = OvMOV;
            character.range = OvRANGE;

            // Override sprite
            renderer.sprite = this.SpriteOverride;
        }
        else
        {
            Debug.Log("Spawning " + character.Name);
            var sprite = Resources.Load<Sprite>(@"Characters/" + character.resourcePath);
            renderer.sprite = sprite;
        }
    }
}
