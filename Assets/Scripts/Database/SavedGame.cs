using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;
using System.Collections.Generic;

namespace GameDB
{

    public class SavedGame
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; private set; }
        public int PlayerHouseId { get; private set; }

        public void SetPlayerHouse (House house)
        {
            this.PlayerHouseId = house.Id;
        }

        /// <summary>
        /// SavedGame saves the state of the current player's game.
        /// </summary>
        public SavedGame()
        {

        }
    }

}