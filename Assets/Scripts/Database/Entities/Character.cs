using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;

class Character
{

    #region Enumerations
    public enum HouseName
    {
        STARK,
        GREYJOY,
        LANNISTER,
        MARTELL,
        TYRELL,
        BARATHEON,
        UNAFFILIATED,
    }
    #endregion

    #region Fields

    [PrimaryKey]
    public string Name { get; set; }
    public int HP { get; set; }
    public int ATT { get; set; }
    public int DEF { get; set; }
    public float EVA { get; set; }
    public float CRIT { get; set; }
    public int MOV { get; set; }
    public HouseName House { get; set; }

    #endregion

    #region Constructors

    public Character() {
        // Default stats
        this.HP = 1000;
        this.ATT = 250;
        this.DEF = 100;
        this.EVA = 0.5f;
        this.CRIT = 0.01f;
        this.MOV = 6;
        this.House = Character.HouseName.UNAFFILIATED;
    }

    #endregion

}
