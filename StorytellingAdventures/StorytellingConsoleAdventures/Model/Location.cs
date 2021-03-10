using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /*
     * Location is the class that describes the places form the map
     * each location contain a name, a list of items, and a path to each cardinal point
     */ 
    class Location
    {
        private Dictionary<string, Path> paths;
        private string name = "";
        private List<Item> items = null;

        public Location(string name)
        {
            this.name = name;
            items = new List<Item>();
            paths = new Dictionary<string, Path>();
        }

        public bool AddItem(Item newItem)
        {
            foreach (Item item in items)
            {
                if (item.Name.Equals(newItem.Name))
                {
                    return false;
                }
            }

            items.Add(newItem);
            return true;
        }

        public bool RemoveItem(string itemName)
        {
            foreach(Item item in items)
            {
                if (item.Name.ToLower().Equals(itemName))
                {
                    items.Remove(item);
                    return true;
                }
            }

            return false;
        }

        public bool IsNear(Location location)
        {
            foreach (KeyValuePair<string, Path> path in paths)
            {
                if (path.Value.GetDestination(this) == location)
                {
                    return true;
                }
            }

            return false;
        }

        public Path GetPath(string direction)
        {
            Path path = null;

            if (paths.ContainsKey(direction))
            {
                path = paths[direction];
            }

            return path;
        }

        public Location GetDestination(string direction)
        {
            Location destination = null;

            if (paths.ContainsKey(direction))
            {
                destination = paths[direction].GetDestination(this);
            }

            return destination;
        }

        public List<string> GetPossibleDirections()
        {
            List<string> possibilities = new List<string>(paths.Keys);

            return possibilities;
        }

        public List<Obstacle> GetObstacles()
        {
            List<Obstacle> obstacles = new List<Obstacle>();

            foreach (KeyValuePair<string, Path> path in paths)
            {
                if (path.Value.HasObstacle())
                {
                    obstacles.Add(path.Value.PathObstacle);
                }
            }

            return obstacles;
        }

        public bool HasObstacle(Obstacle obstacle)
        {
            foreach (KeyValuePair<string, Path> path in paths)
            {
                if (path.Value.HasObstacle())
                {
                    return true;
                }
            }

            return false;
        }

        public bool AddPath(string direction, Path path)
        {
            if (!paths.ContainsKey(direction))
            {
                paths.Add(direction, path);

                return true;
            }

            return false;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }

        public Dictionary<string, Path> Paths
        {
            get
            {
                return paths;
            }
        }
    }
}
