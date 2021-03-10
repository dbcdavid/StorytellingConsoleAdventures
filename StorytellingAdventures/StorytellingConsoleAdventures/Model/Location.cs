using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// Location is the class that describes the places from the map where items and entities may be.
    /// Each location contains a name, a list of items, a description text, and a path to each connecting location
    /// </summary>
    class Location
    {
        private Dictionary<string, LocationPath> paths;
        private string name = "";
        private string description = "";
        private List<Item> items = null;

        public Location(string name)
        {
            this.name = name;
            items = new List<Item>();
            paths = new Dictionary<string, LocationPath>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Adds an item to the list of items that can be found at the location.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the item was added (it is not possible to have two items of the same name, which must be handled during the world creation).
        /// </returns>
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

        /// <summary>
        /// Removes an item from the list of items.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the item was removed.
        /// </returns>
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

        /// <summary>
        /// Verifies if the given location has a path to this location.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the given location has a path to this location.
        /// </returns>
        public bool IsNear(Location location)
        {
            foreach (KeyValuePair<string, LocationPath> path in paths)
            {
                if (path.Value.GetDestination(this) == location)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searches for the path that can be reached by following a given direction.
        /// </summary>
        /// <returns>
        /// The path that can be found using the given direction. If there is no such path, the function returns null.
        /// </returns>
        public LocationPath GetPath(string direction)
        {
            LocationPath path = null;

            if (paths.ContainsKey(direction))
            {
                path = paths[direction];
            }

            return path;
        }

        /// <summary>
        /// Searches for the location that can be reached by following a given direction.
        /// </summary>
        /// <returns>
        /// The location that can be found using the given direction. If there is no such location, the function returns null.
        /// </returns>
        public Location GetDestination(string direction)
        {
            Location destination = null;

            if (paths.ContainsKey(direction))
            {
                destination = paths[direction].GetDestination(this);
            }

            return destination;
        }

        /// <summary>
        /// Gets the name of all possible directions from this location.
        /// </summary>
        /// <returns>
        /// A list of all the possible direction names from this location.
        /// </returns>
        public List<string> GetPossibleDirections()
        {
            List<string> possibilities = new List<string>(paths.Keys);

            return possibilities;
        }

        /// <summary>
        /// Searches for all the obstacles in the middle of the paths reacheables from this location.
        /// </summary>
        /// <returns>
        /// A list of all the obstacles found at the reacheable paths from this location.
        /// </returns>
        public List<Obstacle> GetObstacles()
        {
            List<Obstacle> obstacles = new List<Obstacle>();

            foreach (KeyValuePair<string, LocationPath> path in paths)
            {
                if (path.Value.HasObstacle())
                {
                    obstacles.Add(path.Value.PathObstacle);
                }
            }

            return obstacles;
        }

        /// <summary>
        /// Checks if there is an obstacle in any of the paths reacheables from this location.
        /// </summary>
        /// <returns>
        /// A bool that indicates if there are any obstacles.
        /// </returns>
        public bool HasObstacle(Obstacle obstacle)
        {
            foreach (KeyValuePair<string, LocationPath> path in paths)
            {
                if (path.Value.HasObstacle())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a new path from this location
        /// </summary>
        /// <returns>
        /// A bool indicating if the path was added. If there was already a path with the same direction, it is not added.
        /// </returns>
        public bool AddPath(string direction, LocationPath path)
        {
            if (!paths.ContainsKey(direction))
            {
                paths.Add(direction, path);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Handler of the location's name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Handler of the location's item list.
        /// </summary>
        public List<Item> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Handler of the location's paths.
        /// </summary>
        public Dictionary<string, LocationPath> Paths
        {
            get
            {
                return paths;
            }
        }

        /// <summary>
        /// Handler of the location's descriptions.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
    }
}
