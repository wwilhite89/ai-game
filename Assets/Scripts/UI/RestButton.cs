using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GameDB;

public class RestButton : MonoBehaviour {
    private LevelManager lvlMgr;
	GameObject character;
    private Button btn;

	// Use this for initialization
	void Start () {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();
        this.btn = this.gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		character = lvlMgr.ActiveCharacter;

        this.setActive(lvlMgr.CurrentTurn == LevelManager.Turn.PLAYER && lvlMgr.ActiveCharacterCtrl != null 
            && !lvlMgr.ActiveCharacterCtrl.HasMoved && !lvlMgr.ActiveCharacterCtrl.HasAttacked
            && !lvlMgr.IsMoving());
	}

	public void OnClick() {

        if (lvlMgr.ActiveCharacterCtrl != null && !lvlMgr.ActiveCharacterCtrl.HasMoved 
            && !lvlMgr.ActiveCharacterCtrl.HasAttacked && !lvlMgr.IsMoving())
            lvlMgr.ActiveCharacterCtrl.Rest();
	}

    private void setActive(bool active)
    {
        this.btn.image.color = active ? Color.white : Color.grey;
    }
}
