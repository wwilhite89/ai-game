using UnityEngine;
using System.Collections;
using ArtificialNeuralNetworks.AttackNetwork;
using System.Linq;
using GameDB;

public class AIChoiceScript : MonoBehaviour {

	
	private LevelManager levelManager;

	// Use this for initialization
	void Start () {
        GameObject manager = GameObject.Find("Manager");
        this.levelManager = manager.GetComponent<LevelManager>();
	}

	void Update () {
	
	}

	public void MoveAI (AttackNetwork.DECISION decision, out int messagesGenerated) {
		CharacterController controller = this.GetComponent<CharacterController>();
        
		var enemies = this.GetComponent<CharacterController>().enemies;

        var currentMsgCount = this.levelManager.GetMessageQueueCount();

		switch(decision) {
		    case AttackNetwork.DECISION.ATTACK_CLOSEST:
                this.attackClosest(enemies, controller);
			    break;
		    case AttackNetwork.DECISION.ATTACK_WEAKEST:
                this.attackWeakest(enemies, controller);
			    break;
            case AttackNetwork.DECISION.ATTACK_WEAKEST_IN_RANGE:
                this.attackWeakestInRange(enemies, controller);
			    break;
		    case AttackNetwork.DECISION.RUN:
                this.run(enemies, controller);
			    break;
            case AttackNetwork.DECISION.REST:
                controller.Rest();
                break;
		}

        messagesGenerated = Mathf.Max(0, levelManager.GetMessageQueueCount() - currentMsgCount);
	}

    private void run(GameObject[] enemies, CharacterController controller)
    {
        int zMoves = 0; // moves to make on the z-axis
        int xMoves = 0; // moves to make on  the x-axis
        int top = 0;	   // Enemies above the player
        int bottom = 0;  // Enemies bellow the player
        int left = 0;    // Enemies to the left of the player
        int right = 0;   // Enemies to the right of the player
        Vector3 curLocation = this.transform.position; //current location of the player
        Vector3 newLocation;  // Location for the player to move to;
        int[] actLvls = this.GetComponent<AttackRangeScript>().getLvls();
        CharacterController charCtrl = gameObject.GetComponent<CharacterController>();
        int moveRange = (int)charCtrl.GetStat(GameDB.Character.Stats.RANGE);

        top = actLvls[0] + actLvls[1] + actLvls[4];
        bottom = actLvls[2] + actLvls[3] + actLvls[6];
        left = actLvls[1] + actLvls[3] + actLvls[5];
        right = actLvls[0] + actLvls[2] + actLvls[7];

        zMoves = bottom - top;
        xMoves = left - right;

        if (zMoves > moveRange) zMoves = moveRange; 
        if (xMoves > moveRange) xMoves = moveRange;
        if (zMoves < -moveRange) zMoves = -moveRange;
        if (xMoves < -moveRange) xMoves = -moveRange;
        
        newLocation = new Vector3(curLocation.x + xMoves, curLocation.y, curLocation.z + zMoves);

        float dist = 0.0f;
        float newDist = float.MaxValue;
        GameObject enemy = null;

        // Find the closest enemy
        for (int i = 0; i < enemies.Length; i++)
        {
            dist = Vector3.Distance(this.transform.position, enemies[i].transform.position);
            if (dist < newDist)
            {
                newDist = dist;
                enemy = enemies[i];
            }
        }

        // Run away from him/her
        if ((zMoves != 0 || xMoves != 0) && this.GetComponent<CharacterController>().GetWalkableLand().Length > 0)
            runMove(newLocation);
        else
            this.GetComponent<CharacterController>().Move(walkFrom(enemy));

        controller.ForfeitAttack();
    }

    private void attackClosest(GameObject[] enemies, CharacterController controller)
    {
        float dist = 0.0f;
        float newDist = float.MaxValue;
        GameObject enemy = null;
        Vector3 newPos;

        // Find enemies in range
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
        this.tryAttack(newDist, controller, enemy.GetComponent<CharacterController>());
    }

    private void attackWeakest(GameObject[] enemies, CharacterController controller)
    {
        float dist = 0.0f;
        float newDist = float.MaxValue;
        Vector3 newPos;

        // Find enemy with lowest health
        var enemy = enemies.First(x => x.GetComponent<CharacterController>().GetStat(Character.Stats.HP) == 
            enemies.Min(y => y.GetComponent<CharacterController>().GetStat(Character.Stats.HP)));


        newDist = Vector3.Distance(this.transform.position, enemy.transform.position);
        
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
        this.tryAttack(newDist, controller, enemy.GetComponent<CharacterController>());
    }

    private void attackWeakestInRange(GameObject[] enemies, CharacterController controller)
    {
        var weakest = enemies.FirstOrDefault(x => x.GetComponent<CharacterController>().GetStat(Character.Stats.HP) ==
            enemies.Select(y => y.GetComponent<CharacterController>()).Min(y => y.GetStat(Character.Stats.HP)));

        // This should not happen
        if (weakest == null)
        {
            var generated = 0;
            this.MoveAI(AttackNetwork.DECISION.ATTACK_WEAKEST, out generated);
            return;
        }

        // Move if out of range
        var newDist = Vector3.Distance(this.transform.position, weakest.transform.position);

        if (newDist > controller.GetStat(Character.Stats.RANGE))
        {
            var newPos = walkTo(weakest);
            controller.Move(newPos);
            newDist = Vector3.Distance(newPos, weakest.transform.position);
        }
        else
            controller.ForfeitMovement();

        // Attack
        this.tryAttack(newDist, controller, weakest.GetComponent<CharacterController>());
    }

	private Vector3 walkTo(GameObject enemy) {
		float dist;
		float newDist = float.MaxValue;
        int destination = -1;
		GameObject Land = null;
        var walkable = this.GetComponent<CharacterController>().GetWalkableLand();
		// what walkable land is closest to the enemy
		for (int i = 0; i < walkable.Length; i++) {

			dist = Vector3.Distance(enemy.transform.position, walkable[i].transform.position);
			if (dist < newDist) {
				newDist = dist;
				Land = walkable[i];
                destination = i;
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
        var walkable = this.GetComponent<CharacterController>().GetWalkableLand();

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

	private Vector3 walkFrom(GameObject enemy) {
		float dist;
		float newDist = 0.0f;
		GameObject Land = null;
        var walkable = this.GetComponent<CharacterController>().GetWalkableLand();

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

    private void tryAttack(float distance, CharacterController attacker, CharacterController attackee)
    {
		var otherReachable = reachable ();
        if (distance - 1 < attacker.character.range)
        {
            attacker.StartAttack();
            attacker.FinalizeAttack(attackee);
        }

        else if (otherReachable != null) {
			attacker.StartAttack();
			attacker.FinalizeAttack(otherReachable.GetComponent<CharacterController>());
		}
		else
            attacker.ForfeitAttack();
    }

	private GameObject reachable() {
		var enemies = this.GetComponent<CharacterController> ().enemies;

		for (int i = 0; i < enemies.Length; i++) {
			var dist = Vector3.Distance(this.transform.position,enemies[i].transform.position);
			if ( dist <= this.GetComponent<CharacterController>().GetStat(Character.Stats.RANGE))
                return enemies[i];
            }
		return null;
	}
}
