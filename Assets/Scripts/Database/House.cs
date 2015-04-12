using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;

namespace GameDB
{
    public class House
    {

        #region Enumerations
        public enum HouseName
        {
            STARK = 0,
            GREYJOY = 1,
            LANNISTER = 2,
            MARTELL = 3,
            TYRELL = 4,
            BARATHEON = 5,
        }

        #endregion

        #region Fields

        [AutoIncrement, PrimaryKey]
        public int Id { get; private set; }
        [Unique]
        public HouseName Name { get; set; }

        #endregion

    }
}