using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;

public class Database : MonoBehaviour {

    
    private readonly string connectionString = "Game.db";

	// Use this for initialization
	void Start () {
        StartCoroutine(this.setup());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator setup() {

        using (var c = new SQLiteConnection(connectionString))
        {
            this.createCharacters(c);
        }

        yield return new WaitForSeconds(0);
    }

    private void createCharacters(SQLiteConnection c)
    {
       
        //var characters = c.CreateTable<Character>();

        c.Insert(new Character { 
            Name = "The Hound",
            HP = 1000
        });

        var q = c.Table<Character>().Where(x => x.Name == "The Hound");

        foreach (var r in q)
            Debug.Log(r.Name + " found!");


        /*
        var cmd = c.CreateCommand("create table IF NOT EXISTS Character(Name varchar(256), HP int, " +
                "ATT int, DEF int, EVA int, CRIT int, MOV int, House varchar(256));");
        */

           /* using (var tran = c.BeginTransaction())
            {
                cmd.CommandText = string.Format("INSERT INTO Character VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", 
                    "The Hound", 1000, 500, 350, 25, 5, 6, "Lannister");
                tran.Commit();
            }
        }*/
    }


}