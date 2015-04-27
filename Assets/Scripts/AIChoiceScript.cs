using UnityEngine;
using System.Collections;
using ArtificialNeuralNetworks.AttackNetwork;
using System.Linq;
using GameDB;

public class AIChoiceScript : MonoBehaviour {

	
	private GameObject levelManager;
	public GameObject[] walkable;
	public GameObject[] enemies;

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveAI (AttackNetwork.DECISION decision ) {
		GameObject enemy = null;
		float dist = 0.0f;
		float newDist = 100.0f;
		Vector3 newPos;
		CharacterController controller = this.GetComponent<CharacterController>();
	
		enemies = this.GetComponent<CharacterController>().enemies;
		walkable = this.GetComponent<CharacterController> ().getWalkableLand ();
		
		switch(decision) {
		case AttackNetwork.DECISION.ATTACK_CLOSEST:
			// look through the enemies and see if any are in reach
            enemies.ToList().ForEach(x =>
                {
                    dist = Vector3.Distance(this.transform.position, x.transform.position);
                    
                    if (dist < newDist)
                    {
                        newDist = dist;
                        enemy = x;
                    }
                });

			// Move
            if (newDist > controller.GetStat(Character.Stats.RANGE))
            {
                newPos = walkTo(enemy);
                controller.Move(newPos);
                newDist = Vector3.Distance(newPos, enemy.transform.position);
            }
            else
                controller.ForfeitMovement();

			// Attack
            if (newDist - 1 < controller.character.range)
            {
                controller.StartAttack();
                controller.FinalizeAttack(enemy.GetComponent<CharacterController>());
            }
            else
                controller.ForfeitAttack();


			break;
		case AttackNetwork.DECISION.ATTACK_WEAKEST:
			float health;
			float lowHealth;
			if ( enemies != null ) {
				lowHealth = enemies[0].GetComponent<CharacterController>().GetStat(Character.Stats.HP);
				enemy = enemies[0];
				// find the enemy with the lowest health
				for (int i = 1; i < enemies.Length; i++) {
					health = enemies[i].GetComponent<CharacterController>().GetStat(Character.Stats.HP);
					if (health < lowHealth) {
						lowHealth = health;
						enemy = enemies[i];
					}
				}
			}
			newDist = Vector3.Distance(this.transform.position, enemy.transform.position);
			if (newDist > controller.GetStat(Character.Stats.RANGE)) {
				newPos = walkTo(enemy);
				controller.Move(newPos);
				newDist = Vector3.Distance(newPos, enemy.transform.position);
			}
            else
                controller.ForfeitMovement();

			if(newDist-1 < controller.character.range) {
				controller.StartAttack();
				controller.FinalizeAttack(enemy.GetComponent<CharacterController>());
			}
            else
                controller.ForfeitAttack();

			break;
            
            case AttackNetwork.DECISION.ATTACK_WEAKEST_IN_RANGE:
			// look through the enemies and see if any are in reach
			if ( enemies != null ) {
				// arbitrary high value, we can replace this with the highest health anyone can have
				lowHealth = 1000;
				for (int i = 0; i < enemies.Length; i++) {
					dist = Vector3.Distance(this.transform.position, enemies[i].transform.position);
					if (dist < newDist) {
						health = enemies[i].GetComponent<CharacterController>().GetStat(Character.Stats.HP);
						if (health < lowHealth) {
							lowHealth = health;
							newDist = dist;
							enemy = enemies[i];
						}
					}
				}
			}
            if (newDist > controller.GetStat(Character.Stats.RANGE))
            {
                newPos = walkTo(enemy);
                controller.Move(newPos);
                newDist = Vector3.Distance(newPos, enemy.transform.position);
            }
            else
                controller.ForfeitMovement();

			if(newDist-1 < controller.character.range) {
				controller.StartAttack();
				controller.FinalizeAttack(enemy.GetComponent<CharacterController>());
			}
            else
                controller.ForfeitAttack();
			break;
		case AttackNetwork.DECISION.RUN:
			Debug.Log ("RUN");
			int zMoves =0; // moves to make on the z-axis
			int xMoves =0; // moves to make on  the x-axis
			int top=0;	   // Enemies above the player
			int bottom=0;  // Enemies bellow the player
			int left=0;    // Enemies to the left of the player
			int right=0;   // Enemies to the right of the player
			Vector3 curLocation = this.transform.position; //current location of the player
			Vector3 newLocation;  // Location for the player to move to;
			int[]actLvls=this.GetComponent<AttackRangeScript>().getLvls();
			CharacterController charCtrl = gameObject.GetComponent<CharacterController>();
			int moveRange= (int) charCtrl.GetStat(GameDB.Character.Stats.RANGE);

			//Debug.Log ("actlevel 0: "+actLvls[4]);
			top = actLvls[0]+actLvls[1]+actLvls[4];
			bottom = actLvls[2]+actLvls[3]+actLvls[6];
			left = actLvls[1]+actLvls[3]+actLvls[5];
			right = actLvls[0]+actLvls[2]+actLvls[7];
			zMoves = bottom-top;
			xMoves = left - right;
			Debug.Log (zMoves);
			Debug.Log (xMoves);
			Debug.Log (string.Format ("Activation Levels: {0},{1},{2},{3},{4},{5},{6},{7}", actLvls[0], actLvls [1], actLvls [2], actLvls [3], actLvls [4], actLvls [5], actLvls [6], actLvls [7]));
			//Debug.Log (left+" "+right);
			//Debug.Log (top+" "+bottom);
			if(zMoves>moveRange){
				zMoves=moveRange;
			}
			if(xMoves>moveRange){
				xMoves=moveRange;
			}
			if(zMoves<-moveRange){
				zMoves=-moveRange;
			}
			if(xMoves<-moveRange){
				xMoves=-moveRange;
			}

			newLocation = new Vector3(curLocation.x+xMoves,curLocation.y,curLocation.z+zMoves);
			//Debug.Log ("player"+this.transform.position.ToString());
			//Debug.Log ("cur"+curLocation.x+" "+curLocation.z);
			//Debug.Log (newLocation.x+" "+newLocation.y);

			// who is the closest enemy
			for (int i = 0; i < enemies.Length; i++) {
				dist = Vector3.Distance(this.transform.position, enemies[i].transform.position);
				if (dist < newDist) {
					newDist = dist;
					enemy = enemies[i];
				}
			}
						
			if((zMoves!=0||xMoves!=0)&&walkable.Length>0){
				runMove(newLocation);
			}
			else{
				this.GetComponent<CharacterController>(). Move (walkFrom(enemy));
			}

            controller.ForfeitAttack();

			break;

            case AttackNetwork.DECISION.REST:
                controller.Rest();
                break;
		}
        
	}

	Vector3 walkTo(GameObject enemy) {
		float dist;
		float newDist = 1000.0f;
		GameObject Land = null;

		// what walkable land is closest to the enemy
		for (int i = 0; i < walkable.Length; i++) {

			dist = Vector3.Distance(enemy.transform.position, walkable[i].transform.position);
			if (dist < newDist) {
				newDist = dist;
				Land = walkable[i];
			}
		}
		if (Land != null)
			return Land.transform.position;

		return this.transform.position;
	}

	private void runMove(Vector3 v){
		bool moved = false;
		float dist;
		float newDist = 1000.0f;
		Vector3 Land=new Vector3();
		for (int i = 0; i < walkable.Length; i++) {
			Vector3 square = walkable[i].transform.position;
			dist = Vector3.Distance(v, walkable[i].transform.position);
			if (dist < newDist) {
				newDist = dist;
				Land = walkable[i].transform.position;
			}
			if (dist<1.0f) {
			
				this.GetComponent<CharacterController>(). Move (walkable[i].transform.position);
				moved = true;
			}
		}
		if(!moved&&Land!=null){
			this.GetComponent<CharacterController>(). Move (Land);
		}

	}
	

	Vector3 walkFrom(GameObject enemy) {
		float dist;
		float newDist = 0.0f;
		GameObject Land = null;
		
		// what walkable land is closest to the enemy
		for (int i = 0; i < walkable.Length; i++) {
			dist = Vector3.Distance(enemy.transform.position, walkable[i].transform.position);
			if (dist > newDist) {
				newDist = dist;
				Land = walkable[i];
			}
		}
		if (Land != null)
			return Land.transform.position;
		
		return this.transform.position;
	}
}
