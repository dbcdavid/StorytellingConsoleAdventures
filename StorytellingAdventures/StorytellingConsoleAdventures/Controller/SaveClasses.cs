using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.Model;

namespace StorytellingConsoleAdventures.Controller
{
    /// <summary>
    /// Class that contains "saveable" version of the World class
    /// </summary>
    class WorldSave
    {
        public List<LocationSave> map = new List<LocationSave>();
        public List<ItemSave> items = new List<ItemSave>();
        public EntitySave player = null;
        public MonsterSave monster = null;
        public int playerActionCount = 0;
        public string introduction = "";
        public string ending = "";
    }

    /// <summary>
    /// Class that contains "saveable" version of the Location class
    /// </summary>
    class LocationSave
    {
        public string name = "";
        public string description = "";
        public List<string> items = new List<string>();
        public Dictionary<string, PathSave> paths = new Dictionary<string, PathSave>();
    }

    /// <summary>
    /// Class that contains "saveable" version of the LocationPath class
    /// </summary>
    class PathSave
    {
        public string location1 = "";
        public string location2 = "";
        public ObstacleSave obstacleSave = null;
    }

    /// <summary>
    /// Class that contains "saveable" version of the Obstacle class
    /// </summary>
    class ObstacleSave
    {
        public string name = "";
        public string condition = "";
        public bool solved = false;
        public string solution = null;
    }

    /// <summary>
    /// Class that contains "saveable" version of the Item class
    /// </summary>
    class ItemSave
    {
        public string name;
        public string effect;
    }

    /// <summary>
    /// Class that contains "saveable" version of the Entity class
    /// </summary>
    class EntitySave
    {
        public string location = null;
        public string name = string.Empty;
        public int lifePoints = 0;
        public List<string> items = new List<string>();
    }

    /// <summary>
    /// Class that contains "saveable" version of the Monster class
    /// </summary>
    class MonsterSave
    {
        public string location = null;
        public string name = string.Empty;
        public int lifePoints = 0;
        public List<string> items = new List<string>();
        public Monster.Planning planning;
    }
}
