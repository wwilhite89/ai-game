using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using GameDB;
using GameDB.SessionData;
using UnityEngine;

public class GameManager {

    #region Enumerations
    public enum Map
    {
        StartMenu = 0,
        Winterfell = 1,
        WhiteHarbor = 2,
        TheEyrie = 3,
        Pyke = 4,
        Ironstone = 5,
    }

    #endregion

    #region Fields

    #endregion

    /// <summary>
    /// Starts a new game by loading the 1st Level
    /// </summary>
    /// <param name="house"></param>
    public void StartNewGame(House.HouseName house)
    {
        var db = Database.getInstance();
        var game = db.CreateNewGame(house);

        // Set defaults
        PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, (int)Map.StartMenu); // Level
        PlayerPrefs.SetInt(GameConstants.CURRENT_HOUSE, (int) house); // House
        PlayerPrefs.SetInt(GameConstants.CURRENT_GAME, game.Id); // House

        LoadNextLevel();
    }

    /// <summary>
    /// Gets current players based off the saved game being played.
    /// </summary>
    /// <returns></returns>
    public IList<Character> GetCurrentPlayers()
    {
        var db = Database.getInstance();
        IList<Character> players;

        if (PlayerPrefs.HasKey(GameConstants.CURRENT_GAME))
        {
            var game = PlayerPrefs.GetInt(GameConstants.CURRENT_GAME);
            try
            {
                players = db.GetCharacters(game);
            }
            catch (UnityException ex)
            {
                Debug.LogWarning(ex);
                Debug.LogWarning("Loading default characters for house.");
                PlayerPrefs.DeleteKey(GameConstants.CURRENT_GAME);
                players = db.GetDefaultCharacters(House.HouseName.STARK);
            }
        }
        else
        {
            if (PlayerPrefs.HasKey(GameConstants.CURRENT_HOUSE))
                players = db.GetDefaultCharacters((House.HouseName)PlayerPrefs.GetInt(GameConstants.CURRENT_HOUSE));
            else
                players = db.GetDefaultCharacters(House.HouseName.STARK);
        }

        return players;
    }

    /// <summary>
    /// Gets current enemies for the current level being played on. Currently just the subsequent house in the list.
    /// </summary>
    /// <returns></returns>
    public IList<Character> GetCurrentEnemies()
    {
        var house = PlayerPrefs.HasKey(GameConstants.CURRENT_HOUSE) ? (House.HouseName) PlayerPrefs.GetInt(GameConstants.CURRENT_HOUSE) : House.HouseName.STARK;
        // var map = PlayerPrefs.HasKey(GameConstants.CURRENT_LEVEL) ? (Map) PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL) : Map.Winterfell;
        var db = Database.getInstance();

        // Temporary
        var enemyHouse = (House.HouseName) ((int)house == 3 ? 0 : (int)house + 1);
        return db.GetDefaultCharacters(enemyHouse);
    }

    /// <summary>
    /// Handles logic for loading the next level based off current session house and level.
    /// </summary>
    public void LoadNextLevel() {
        Map currentLevel = (Map)PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL);
        House.HouseName house = (House.HouseName)PlayerPrefs.GetInt(GameConstants.CURRENT_HOUSE);

        if (currentLevel == Map.StartMenu)
        {
            if (house == House.HouseName.STARK)
            {
                PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, (int)Map.Winterfell);
                Application.LoadLevel(GameConstants.SCENE_WINTERFELL);
            }
            else
            {
                PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, (int)Map.WhiteHarbor);
                Application.LoadLevel(GameConstants.SCENE_WHITE_HARBOR);
            }

        }

    }


    /// <summary>
    /// Logic for game over
    /// </summary>
    public void GameOver()
    {
        PlayerPrefs.DeleteAll();
        Application.LoadLevel(GameConstants.SCENE_MENU);
    }
}