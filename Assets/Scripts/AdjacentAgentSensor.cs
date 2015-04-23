using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class AdjacentAgentSensor : MonoBehaviour {
	public LevelManager LvlMgr;
	private GameObject[] enemies;
	private GameObject enemy;
	public int range;
	private float dist;

    void Start()
    {
		this.GetComponent<CharacterController> ().GetStat (GameDB.Character.Stats.RANGE);
		LvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
    }

    void FixedUpdate()
    {
		Vector3 newPos;

		enemies = LvlMgr.getEnemies();
        
		for (int i = 0; i < enemies.Length; i++) {
			dist = Vector3.Distance(enemies[0].transform.position, this.transform.position);

			if (dist < range) {
				newPos.x = (float)Math.Round(this.transform.position.x);
				newPos.y = (float)Math.Round(this.transform.position.y);
				newPos.z = (float)Math.Round(this.transform.position.z);

				this.GetComponent<CharacterController>().Move( newPos );
			}
		}
    }
}
