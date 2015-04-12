using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using GameDB;
using GameDB.SessionData;

// TODO: GUI for saving and loading games

public class GOTMenuScript : MonoBehaviour {

    public KeyCode StartKey = KeyCode.Return;
    public KeyCode AltStartKey = KeyCode.KeypadEnter;
    public int HouseColumns = 2;

    #region Private Fields
    private Database gamedb = null;
    private IList<House> houses;

    private bool started = false;
    private GUIStyle displayStyle = new GUIStyle();
    private Rect displayRect = new Rect();
    private bool isBlinking = false;
    private string displayText = "Press Start";

    private const int buttonWidth = 150, buttonHeight = 60, padding = 10;
    private IList<HouseButton> houseButtons = new List<HouseButton>();

    private CultureInfo gameCulture;
    private bool loadingLevel = false;
    #endregion

    void Start () {
        gamedb = Database.getInstance();

        // Default values
        var w = 0.3f;
        var h = 0.2f;
        displayRect.x = (Screen.width * (1 - w)) / 2;
        displayRect.y = (Screen.height * (1 - h)) / 1.5f; // 2/3rds down 80% of the screen
        displayRect.width = Screen.width * w;
        displayRect.height = Screen.height * h;
        displayStyle.alignment = TextAnchor.MiddleCenter;
        displayStyle.fontSize = 50;
        displayStyle.normal.textColor = Color.white;

        this.gameCulture = new CultureInfo("en-US", false);
	}
	
	void Update () {

        // Press Start
        if ((Input.GetKey(this.StartKey) || (AltStartKey != null && Input.GetKey(this.AltStartKey))) && !this.started)
        {
            this.started = true;
            this.createHouseSelection();
            this.displayText = "Select your House";
        }
        
	}

    private void createHouseSelection()
    {
        this.houses = gamedb.GetDefaultHouses().ToList();
        int numCols = this.HouseColumns,
            col = 0, 
            row = 0;
        var startY = (0.6f) * Screen.height;
        var maxCols = (float) Screen.width / (buttonWidth + padding);
        var emptyCols = maxCols - numCols;
        var startX = ((emptyCols / 2) * (buttonWidth + padding));

        foreach (var house in this.houses)
        {
            var hb = new HouseButton();
            hb.house = house;
            var xVal = startX + (col * (buttonWidth + padding));
            var yVal = startY + ((row + 1) * (buttonHeight + padding));

            hb.rect = new Rect(xVal, yVal, buttonWidth, buttonHeight);

            this.houseButtons.Add(hb);

            if (++col == numCols)
            {
                col = 0;
                row++;
            }
        }

    }

	void OnGUI()
	{
        GUI.Label(this.displayRect, this.displayText, displayStyle);

        // Press Start
        if (!this.started)
        {
            
            if (!this.isBlinking)
            {
                this.isBlinking = true;
                StartCoroutine(this.blinkStartMessage());
            }

            return;
        }

        // Select a House
        foreach (var btn in this.houseButtons)
        {
            if (GUI.Button(btn.rect, "House " + this.gameCulture.TextInfo.ToTitleCase(btn.house.Name.ToString().ToLower())))
            {
                if (!this.loadingLevel)
                {
                    if (btn.house.Name == House.HouseName.STARK)
                        Application.LoadLevel("AdamScene2");
                    else
                        Application.LoadLevel("AdamScene");

                    this.loadingLevel = true;

                    PlayerPrefs.SetInt(GameConstants.CURRENT_HOUSE, (int)btn.house.Name);
                }
            }
        }
	}

    private IEnumerator blinkStartMessage()
    {
        yield return new WaitForSeconds(0.7f);
        
        if (!this.started)
            this.displayText = "";
        
        yield return new WaitForSeconds(0.7f);
        
        if (!this.started)
            this.displayText = "Press Start";
        
        this.isBlinking = false;
    }

    internal class HouseButton
    {
        public Rect rect;
        public House house;
    }
}

