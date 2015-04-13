using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;

namespace GameDB
{
    public class Character
    {
		public enum Stats {
			HP,
			ATT,
			DEF,
			EVA,
			CRIT,
			MOV,
			RANGE
		}

        #region Fields

        public string Name { get; set; }
        public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public float evade { get; set; }
        public float critical { get; set; }
        public int movement { get; set; }
        public int range { get; set; }
        public string resourcePath { get; set; }

        public int HouseId { get; private set; }
        public int SavedGameId { get;  private set; }

        #endregion

        #region Constructors

        public Character()
        {
            // Default stats
            this.health = 1000;
            this.attack = 250;
            this.defense = 100;
            this.evade = 0.5f;
            this.critical = 0.01f;
            this.movement = 6;
            this.range = 3;
            this.resourcePath = "Rob";
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