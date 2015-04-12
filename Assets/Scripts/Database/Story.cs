using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;
using GameDB;

namespace GameDB
{
    class Story
    {

        #region Enumerations
        public enum Map
        {
            Introduction = 0,
            Winterfell = 1,
            WhiteHarbor = 2,
            TheEyrie = 3,
            Pyke = 4,
            Ironstone = 5,
        }

        #endregion

        #region Fields

        private Map currentMap;
        // private Character.HouseName house;

        #endregion

        #region Constructors

        public Story (/*Character.HouseName house, */Map currentMap)
        {
            // this.house = house;
            this.currentMap = currentMap;
        }

        #endregion

        public Map GetNextMap() {
            if (this.currentMap == Map.Introduction)
                return Map.Winterfell;
            return Map.TheEyrie;
        }

    }
}