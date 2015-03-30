using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;
using System.Collections.Generic;

public class Database : MonoBehaviour {

    
    private readonly string connectionString = "Game.db";
    public bool isInitialized { get; private set; }

	// Use this for initialization
	void Start () {
        this.isInitialized = false;
        StartCoroutine(this.initializeDatabase());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator initializeDatabase() {

        using (var c = new SQLiteConnection(connectionString, false))
        {
            c.BeginTransaction();
            //c.Execute("IF EXISTS ..Character
            // Create characters

            //var droppage = c.DropTable<Character>();
            //Debug.Log(droppage);
            var table = c.Execute("", new object[]{});
            Debug.Log(table); // check result
            //c.InsertAll(getInitialCharacters(), false);

            c.Commit();
        }

        this.isInitialized = true;
        yield return new WaitForSeconds(0);
    }
   

    private IEnumerable<Character> getInitialCharacters()
    {
        var c = new List<Character>();
        
        #region Greyjoy
        c.Add(new Character { Name = "Balon Greyjoy", House = Character.HouseName.GREYJOY});
        c.Add(new Character { Name = "Theon Greyjoy", House = Character.HouseName.GREYJOY });
        c.Add(new Character { Name = "Victarian Greyjoy", House = Character.HouseName.GREYJOY });
        c.Add(new Character { Name = "Euron Crow's Eye", House = Character.HouseName.GREYJOY });
        c.Add(new Character { Name = "Ashsa Greyjoy", House = Character.HouseName.GREYJOY });
        c.Add(new Character { Name = "Dagmar Cleftjaw", House = Character.HouseName.GREYJOY });
        c.Add(new Character { Name = "Aaron Damphair", House = Character.HouseName.GREYJOY });
        #endregion

        #region Stark
        c.Add(new Character { Name = "Eddard Stark", House = Character.HouseName.STARK });
        c.Add(new Character { Name = "Robb Stark", House = Character.HouseName.STARK });
        c.Add(new Character { Name = "Catelyn Stark", House = Character.HouseName.STARK });
        c.Add(new Character { Name = "Roose Bolton", House = Character.HouseName.STARK });
        c.Add(new Character { Name = "Greation Umber", House = Character.HouseName.STARK });
        c.Add(new Character { Name = "The Blackfish", House = Character.HouseName.STARK });
        c.Add(new Character { Name = "Ser Rodrick Cassel", House = Character.HouseName.STARK });
        #endregion

        #region Lannister

        c.Add(new Character { Name = "Jaime Lannister", House = Character.HouseName.LANNISTER });
        c.Add(new Character { Name = "The Hound", House = Character.HouseName.LANNISTER });
        c.Add(new Character { Name = "The Mountain", House = Character.HouseName.LANNISTER });
        c.Add(new Character { Name = "Tywin Lannister", House = Character.HouseName.LANNISTER });
        c.Add(new Character { Name = "Cersei Lannister", House = Character.HouseName.LANNISTER });
        c.Add(new Character { Name = "Ser Kevan Lannister", House = Character.HouseName.LANNISTER });
        c.Add(new Character { Name = "Tyrion Lannister", House = Character.HouseName.LANNISTER });

        #endregion

        #region MARTELL

        c.Add(new Character { Name = "The Red Viper", House = Character.HouseName.MARTELL });
        c.Add(new Character { Name = "Areo Hotah", House = Character.HouseName.MARTELL });
        c.Add(new Character { Name = "Obara Sand", House = Character.HouseName.MARTELL });
        c.Add(new Character { Name = "Darkstar", House = Character.HouseName.MARTELL });
        c.Add(new Character { Name = "Nymeria Sand", House = Character.HouseName.MARTELL });
        c.Add(new Character { Name = "Arianne Martell", House = Character.HouseName.MARTELL });
        c.Add(new Character { Name = "Doran Martell", House = Character.HouseName.MARTELL });
 

        #endregion

        #region TYRELL

        c.Add(new Character { Name = "Randell Tarli", House = Character.HouseName.TYRELL });
        c.Add(new Character { Name = "Ser Garlen Tyrell", House = Character.HouseName.TYRELL });
        c.Add(new Character { Name = "Ser Loras Tyrell", House = Character.HouseName.TYRELL });
        c.Add(new Character { Name = "Mace Tyrell", House = Character.HouseName.TYRELL });
        c.Add(new Character { Name = "Queen of Thorns", House = Character.HouseName.TYRELL });
        c.Add(new Character { Name = "Margaery Tyrell", House = Character.HouseName.TYRELL });
        c.Add(new Character { Name = "Alester Florent", House = Character.HouseName.TYRELL });

        #endregion
        
        #region BARATHEON

        c.Add(new Character { Name = "Stannis Baratheon", House = Character.HouseName.BARATHEON });
        c.Add(new Character { Name = "Renly Baratheon", House = Character.HouseName.BARATHEON });
        c.Add(new Character { Name = "Ser Davos Seaworth", House = Character.HouseName.BARATHEON });
        c.Add(new Character { Name = "Brienne of Tarth", House = Character.HouseName.BARATHEON });
        c.Add(new Character { Name = "Melisandre", House = Character.HouseName.BARATHEON });
        c.Add(new Character { Name = "Sallandhor Saan", House = Character.HouseName.BARATHEON });
        c.Add(new Character { Name = "Patchface", House = Character.HouseName.BARATHEON });
       
        #endregion

        return c.AsEnumerable();
    }


}