using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using GameDB;

public class AdjacentAgentSensor : MonoBehaviour {
	public LevelManager LvlMgr;
	private GameObject[] enemies;
	private GameObject enemy;
	public float range;
	public float speed;
	private float distToEnemy;
	private bool intitailize = false;

    void Start()
    {
		LvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
    }

    void FixedUpdate()
    {

		/*if (intitailize == false) {
			range = this.GetComponent<CharacterController> ().GetStat (Character.Stats.RANGE) + 1;
			speed = this.GetComponent<CharacterController> ().GetStat (Character.Stats.MOV);
		}

		Vector3 newPos;

		enemies = LvlMgr.getEnemies();
        
		for (int i = 0; i < enemies.Length; i++) {
			distToEnemy = Vector3.Distance(enemies[0].transform.position, this.transform.position);

			if (distToEnemy < range) {
				newPos.x = (float)Math.Round(this.transform.position.x);
				newPos.y = (float)Math.Round(this.transform.position.y);
				newPos.z = (float)Math.Round(this.transform.position.z);

				this.GetComponent<CharacterController>().Move( newPos );
			}
		}*/
    }
}
