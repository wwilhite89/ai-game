using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using SQLite4Unity3d;
using System.Collections.Generic;

namespace GameDB
{
    // Singleton
    public class Database {
        private readonly string connectionString = "GameDev.db";
        private static Database instance = null;

        public static Database getInstance()
        {
            if (instance == null)
            {
                instance = new Database();
                Debug.Log("Game database initialized.");
            }
            return instance;
        }

        private Database () {
            if (!System.IO.File.Exists("GameDev.db"))
            {
                // Create database
                using (var c = new Factory().Create(this.connectionString))
                {
                    c.BeginTransaction();

                    // Create tables
                    c.CreateTable<SavedGame>();
                    c.CreateTable<House>();
                    c.CreateTable<Character>();
                    
                    // Create characters
                    var houses = getDefaultHouses();
                    var initialGame = new SavedGame();

                    c.Insert(initialGame);
                    c.InsertAll(houses, false);

                    foreach (var h in houses)
                    {
                        var characters = getDefaultCharacters(h.Name);
                        
                        foreach (var ch in characters)
                        {
                            ch.setSavedGame(initialGame);
                            ch.setHouse(h);
                        }

                        c.InsertAll(characters, false);
                    }

                    c.Commit();
                }
            }
        }

        public IList<House> GetDefaultHouses()
        {
            using (var c = new SQLiteConnection(this.connectionString, false))
            {
                return c.Table<House>().ToList();
            }
        }

        public IList<Character> GetDefaultCharacters(House.HouseName house)
        {
            using (var c = new SQLiteConnection(this.connectionString, false))
            {
                var h = c.Table<House>().Where(x => x.Name == house).FirstOrDefault();
                return c.Table<Character>().Where(x => x.HouseId == h.Id).OrderBy(x => x.SavedGameId).ToList();
            }
        }

        public IList<Character> GetCharacters(int savedGameID)
        {
            using (var c = new SQLiteConnection(this.connectionString, false))
            {
                var game = c.Table<SavedGame>().FirstOrDefault(x => x.Id == savedGameID);

                // Ensure save exists
                if (game == null)
                    throw new UnityException("Cannot load characters for save with id=" + savedGameID);

                return c.Table<Character>().Where(x => x.SavedGameId == savedGameID).ToList();
            }
        }

        public SavedGame CreateNewGame(House.HouseName house)
        { 
            // Create new save
            var newGame = new SavedGame();

            using (var c = new SQLiteConnection(this.connectionString, false))
            {
                c.BeginTransaction();

                var h = c.Table<House>().Where(x => x.Name == house).FirstOrDefault();
                newGame.SetPlayerHouse(h);

                // Save game
                c.Insert(newGame);

                // Copy house characters
                var characters = getDefaultCharacters(house);
                characters.ToList().ForEach(x => { x.setHouse(h); x.setSavedGame(newGame); });
                c.InsertAll(characters, false);

                c.Commit();
            }

            return newGame;
        }

        private IList<House> getDefaultHouses()
        {
            return new List<House>()
            {
                new House { Name = House.HouseName.STARK },
                new House { Name = House.HouseName.GREYJOY },
                new House { Name = House.HouseName.LANNISTER },
                //new House { Name = House.HouseName.MARTELL },
                //new House { Name = House.HouseName.TYRELL },
                //new House { Name = House.HouseName.BARATHEON }
            };
        }

        private IEnumerable<Character> getDefaultCharacters(House.HouseName house)
        {
            var c = new List<Character>();

            switch (house) 
            {
                case House.HouseName.GREYJOY:
                    #region Greyjoy
//                    c.Add(new Character { Name = "Balon Greyjoy"});
                    c.Add(new Character { Name = "Theon Greyjoy", health = 22, attack = 11, defense = 7, evade = .05f, critical = .01f, movement = 2, range = 2, resourcePath = "Theon" });
				c.Add(new Character { Name = "Victarian Greyjoy", health = 30, attack = 12, defense = 7, evade = .02f, critical = .05f, movement = 2, range = 1, resourcePath = "victarian"  });
//                    c.Add(new Character { Name = "Euron Crow's Eye"});
				c.Add(new Character { Name = "Ashsa Greyjoy", health = 20, attack = 10, defense = 5, evade = .01f, critical = .01f, movement = 3, range = 1, resourcePath = "ashsa" });
//                    c.Add(new Character { Name = "Dagmar Cleftjaw" });
//                    c.Add(new Character { Name = "Aaron Damphair" });
                    #endregion
                    break;
                case House.HouseName.STARK:
                    #region Stark
					c.Add(new Character { Name = "Eddard Stark", health = 25, attack = 10, defense = 6, evade = .10f, critical = .15f, movement = 2, range = 1, resourcePath="Ned"});
					c.Add(new Character { Name = "Robb Stark", health = 23, attack = 12, defense = 6, evade = .20f, critical = .20f, movement = 3, range = 1, resourcePath="Rob"});
//                    c.Add(new Character { Name = "Catelyn Stark" });
				c.Add(new Character { Name = "Roose Bolton", health = 23, attack = 11, defense = 5, evade = .15f, critical = .40f, movement = 2, range = 1, resourcePath = "roose"  });
//                    c.Add(new Character { Name = "Greation Umber" });
//                    c.Add(new Character { Name = "The Blackfish" });
//                    c.Add(new Character { Name = "Ser Rodrick Cassel" });
                    #endregion
                    break;
                case House.HouseName.LANNISTER:
                    #region Lannister
					c.Add(new Character { Name = "Jaime Lannister", health = 24, attack = 10, defense = 5, evade = .15f, critical = .15f, movement = 2, range = 2, resourcePath="Jaime" });
					c.Add(new Character { Name = "The Hound", health = 28, attack = 10, defense = 6, evade = .05f, critical = .10f, movement = 2, range = 1, resourcePath="The Hound" });
//					c.Add(new Character { Name = "The Mountain", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="" });
					c.Add(new Character { Name = "Tywin Lannister", health = 25, attack = 12, defense = 7, evade = .10f, critical = .10f, movement = 3, range = 1, resourcePath="Tywin" });
//                    c.Add(new Character { Name = "Cersei Lannister" });
//                    c.Add(new Character { Name = "Ser Kevan Lannister" });
//                    c.Add(new Character { Name = "Tyrion Lannister" });
                    #endregion
                    break;
//                case House.HouseName.MARTELL:
//                    #region MARTELL
//                c.Add(new Character { Name = "The Red Viper", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime" });
//                c.Add(new Character { Name = "Areo Hotah", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime" });
////                    c.Add(new Character { Name = "Obara Sand" });
//                c.Add(new Character { Name = "Darkstar", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime" });
////                    c.Add(new Character { Name = "Nymeria Sand" });
////                    c.Add(new Character { Name = "Arianne Martell" });
////                    c.Add(new Character { Name = "Doran Martell" });
//                    #endregion
//                    break;
//                case House.HouseName.TYRELL:
//                    #region TYRELL
//                    c.Add(new Character { Name = "Randell Tarli", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime" });
////                    c.Add(new Character { Name = "Ser Garlen Tyrell" });
//                    c.Add(new Character { Name = "Ser Loras Tyrell", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime" });
////                    c.Add(new Character { Name = "Mace Tyrell" });
////                    c.Add(new Character { Name = "Queen of Thorns" });
////                    c.Add(new Character { Name = "Margaery Tyrell" });
//                    c.Add(new Character { Name = "Alester Florent", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime" });
//                    #endregion
//                    break;
//                case House.HouseName.BARATHEON:
//                    #region BARATHEON
//                    c.Add(new Character { Name = "Stannis Baratheon", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime"  });
//                    c.Add(new Character { Name = "Renly Baratheon", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime"  });
//                    c.Add(new Character { Name = "Ser Davos Seaworth", health = 10, attack = 5, defense = 5, evade = .05f, critical = .05f, movement = 2, range = 1, resourcePath="Jaime"  });
////                    c.Add(new Character { Name = "Brienne of Tarth" });
////                    c.Add(new Character { Name = "Melisandre" });
////                    c.Add(new Character { Name = "Sallandhor Saan" });
////                    c.Add(new Character { Name = "Patchface" });
//                    #endregion
//                    break;
                default:
                    break;
            }

            return c.AsEnumerable();
        }
    }
}