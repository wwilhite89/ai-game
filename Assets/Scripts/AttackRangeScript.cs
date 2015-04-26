using UnityEngine;
using System.Collections;
using System.Linq;

public class AttackRangeScript : MonoBehaviour
{

    public Vector3 startDir = new Vector3(0, 0, 1);
	public float realtiveAngle;
    
    private int[] actLevels = new int[8];

    private CharacterController charCtrl;
    private GameObject[] opponents;
	
    private string opponent;
    private int range;
    private bool rangeSet = false;


    enum Slice
    {
        TOP_RIGHT = 0,
        TOP_LEFT = 1,
        BOTTOM_RIGHT = 2,
        BOTTOM_LEFT = 3,
		FRONT = 4,
		LEFT =5,
		BACK = 6,
		RIGHT = 7
    }

    // Use this for initialization
    void Start()
    {
        this.charCtrl = gameObject.GetComponent<CharacterController>();

		if (gameObject.tag == "Enemy") {
			opponents = GameObject.FindGameObjectsWithTag ("Player");
			opponent = "Player";
		} else if (gameObject.tag == "Player") {
			opponents = GameObject.FindGameObjectsWithTag ("Enemy");
			opponent = "Enemy";
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.rangeSet && this.charCtrl.IsInitialized())
        {
            this.range = (int) this.charCtrl.GetStat(GameDB.Character.Stats.RANGE);
            this.rangeSet = true;
        }
    }

    void FixedUpdate()
    {
        if (this.rangeSet)
        {
            drawPieSlices();
            updateActivationLevels();
        }
    }

    private void drawPieSlices() {
        Debug.DrawRay(transform.position, this.transform.up * range, Color.red);
        Debug.DrawRay(transform.position, -this.transform.up * range, Color.blue);
        Debug.DrawRay(transform.position, this.transform.right * range, Color.green);
        Debug.DrawRay(transform.position, -this.transform.right * range, Color.yellow);
    }

    private void updateActivationLevels() {
        int[] oldLevels = new int[8];
        actLevels.CopyTo(oldLevels, 0);

        actLevels[0] = getActivationLevel(Slice.TOP_RIGHT);
        actLevels[1] = getActivationLevel(Slice.TOP_LEFT);
        actLevels[2] = getActivationLevel(Slice.BOTTOM_RIGHT);
        actLevels[3] = getActivationLevel(Slice.BOTTOM_LEFT);
		actLevels[4] = getActivationLevel(Slice.FRONT);
		actLevels[5] = getActivationLevel(Slice.LEFT);
		actLevels[6] = getActivationLevel(Slice.BACK);
		actLevels[7] = getActivationLevel(Slice.RIGHT);


        if (oldLevels [0] != actLevels [0] ||
			oldLevels [1] != actLevels [1] ||
			oldLevels [2] != actLevels [2] ||
			oldLevels [3] != actLevels [3] ||
			oldLevels [4] != actLevels [4] ||
			oldLevels [5] != actLevels [5] ||
			oldLevels [6] != actLevels [6] ||
			oldLevels [7] != actLevels [7]) {
			Debug.Log (string.Format ("Activation Levels: {0},{1},{2},{3},{4},{5},{6},{7}", actLevels [0], actLevels [1], actLevels [2], actLevels [3], actLevels [4], actLevels [5], actLevels [6], actLevels [7]));

		}
	}


	
	/// <summary>
    /// Returns the number of agents in an activation level given a slice.
    /// </summary>
    /// <param name="slice">Slice area relative to the main agent</param>
    /// <returns>Number activation levels</returns>
    private int getActivationLevel(Slice slice)
    {
        int levels = 0;
        float offsetRotation = transform.rotation.eulerAngles.y;


        foreach (var agent in this.getObjectsInRadius(opponent,true))
        {
            Vector3 objDir = (agent.transform.position - gameObject.transform.position).normalized;
            objDir.y = 0;

            float angle = Vector3.Angle(startDir, objDir);

            if (Vector3.Cross(startDir, objDir).y < 0)
                angle = 180 + (180 - angle);

            float relativeAngle = (360.0f + angle - offsetRotation) % 360.0f;
			//relativeAngle= (relativeAngle+45.0f)%360.0f;
			//Debug.Log(relativeAngle);
            switch (slice)
            {
                case Slice.TOP_RIGHT:
                    levels += relativeAngle >= 15.0f && relativeAngle < 75.0f ? 1 : 0;
                    break;
                case Slice.BOTTOM_RIGHT:
                    levels += relativeAngle >= 105.0f && relativeAngle < 165.0f ? 1 : 0;
                    break;
                case Slice.BOTTOM_LEFT:
                    levels += relativeAngle >= 195.0f && relativeAngle < 255.0f ? 1 : 0;
                    break;
                case Slice.TOP_LEFT:
					levels += relativeAngle >= 285.0f && relativeAngle < 345.0f ? 1 : 0;
					break;
				case Slice.FRONT:
					levels += relativeAngle >= 345.0f || relativeAngle < 15.0f ? 1 : 0;
					break;
				case Slice.RIGHT:
					levels += relativeAngle >= 75.0f && relativeAngle < 105.0f ? 1 : 0;
					break;
				case Slice.BACK:
					levels += relativeAngle >= 165.0f && relativeAngle < 195.0f ? 1 : 0;
					break;
				case Slice.LEFT:
					levels += relativeAngle >= 255.0f && relativeAngle < 285.0f ? 1 : 0;
					break;
			default:
				break;
			}
		}
		
		return levels;
    }

    public GameObject[] getObjectsInRadius(string agentName,bool extend)
    {
		int searchRange;
		if (extend) {
			searchRange = this.range + 2;
		} else {
			searchRange = this.range;
		}
        Vector3 pos = gameObject.transform.position;

        var agents = GameObject.FindGameObjectsWithTag(agentName)
            .Where(x => Mathf.Abs((x.transform.position - pos).magnitude) <= searchRange)
            .ToArray();

        return agents;
    }
	public int[] getLvls(){
		return actLevels;
	}
	private void setRelativeAngle(float a){
		realtiveAngle = a;
	}
}