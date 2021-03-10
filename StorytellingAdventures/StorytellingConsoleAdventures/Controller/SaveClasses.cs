using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.Model;

namespace StorytellingConsoleAdventures.Controller
{
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

    class LocationSave
    {
        public string name = "";
        public string description = "";
        public List<string> items = new List<string>();
        public Dictionary<string, PathSave> paths = new Dictionary<string, PathSave>();
    }

    class PathSave
    {
        public string location1 = "";
        public string location2 = "";
        public ObstacleSave obstacleSave = null;
    }

    class ObstacleSave
    {
        public string name = "";
        public string condition = "";
        public bool solved = false;
        public string solution = null;
    }

    class ItemSave
    {
        public string name;
        public string effect;
    }

    class EntitySave
    {
        public string location = null;
        public string name = string.Empty;
        public int lifePoints = 0;
        public List<string> items = new List<string>();
    }

    class MonsterSave
    {
        public string location = null;
        public string name = string.Empty;
        public int lifePoints = 0;
        public List<string> items = new List<string>();
        public Monster.Planning planning;
    }
}
