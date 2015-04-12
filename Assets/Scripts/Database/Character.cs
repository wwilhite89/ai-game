using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;

namespace GameDB
{
    public class Character
    {

        #region Fields

        public string Name { get; set; }
        public int HP { get; set; }
        public int ATT { get; set; }
        public int DEF { get; set; }
        public float EVA { get; set; }
        public float CRIT { get; set; }
        public int MOV { get; set; }
        public int RANGE { get; set; }
        public string resourcePath { get; set; }

        public int HouseId { get; private set; }
        public int SavedGameId { get;  private set; }

        #endregion

        #region Constructors

        public Character()
        {
            // Default stats
            this.HP = 1000;
            this.ATT = 250;
            this.DEF = 100;
            this.EVA = 0.5f;
            this.CRIT = 0.01f;
            this.MOV = 6;
            this.RANGE = 3;
        }

        #endregion

        public void setHouse(House house)
        {
            if (house != null && house.Id != null)
                this.HouseId = house.Id;
            else
                throw new ArgumentNullException();
        }

        public void setSavedGame(SavedGame game)
        {
            if (game != null && game.Id != null)
                this.SavedGameId = game.Id;
            else
                throw new ArgumentNullException();
        }
    }
}