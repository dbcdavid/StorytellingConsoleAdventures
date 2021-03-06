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
        public enum Direction
        {
            NORTH, SOUTH, EAST, WEST
        };

        private string name = "";
        private Path north = null;
        private Path south = null;
        private Path west = null;
        private Path east = null;
        private List<Item> items = null;

        public Location(string name)
        {
            this.name = name;
            items = new List<Item>();
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
            if (north != null && north.GetDestination(this) == location)
            {
                return true;
            }
            if (south != null && south.GetDestination(this) == location)
            {
                return true;
            }
            if (east != null && east.GetDestination(this) == location)
            {
                return true;
            }
            if (west != null && west.GetDestination(this) == location)
            {
                return true;
            }

            return false;
        }

        public Path GetPath(Direction direction)
        {
            Path path = null;
            if (direction == Direction.NORTH)
            {
                path = north;
            }
            else if (direction == Direction.SOUTH)
            {
                path = south;
            }
            else if (direction == Direction.EAST)
            {
                path = east;
            }
            else if (direction == Direction.WEST)
            {
                path = west;
            }

            return path;
        }

        public Location GetDirection(Direction direction)
        {
            Location destination = null;
            if (direction == Direction.NORTH && north != null)
            {
                destination = north.GetDestination(this);
            }
            else if (direction == Direction.SOUTH && south != null)
            {
                destination = south.GetDestination(this);
            }
            else if (direction == Direction.EAST && east != null)
            {
                destination = east.GetDestination(this);
            }
            else if (direction == Direction.WEST && west != null)
            {
                destination = west.GetDestination(this);
            }

            return destination;
        }

        public List<string> GetPossibleDirections()
        {
            List<string> possibilities = new List<string>();

            if (north != null)
            {
                possibilities.Add("north");
            }
            if (south != null)
            {
                possibilities.Add("south");
            }
            if (east != null)
            {
                possibilities.Add("east");
            }
            if (west != null)
            {
                possibilities.Add("west");
            }

            return possibilities;
        }

        public bool HasObstacle(Obstacle obstacle)
        {
            if (north != null && north.HasObstacle(obstacle))
            {
                return true;
            }
            if (south != null && south.HasObstacle(obstacle))
            {
                return true;
            }
            if (east != null && east.HasObstacle(obstacle))
            {
                return true;
            }
            if (west != null && west.HasObstacle(obstacle))
            {
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

        public Path NorthPath
        {
            set
            {
                north = value;
            }

            get
            {
                return north;
            }
        }

        public Path SouthPath
        {
            set
            {
                south = value;
            }

            get
            {
                return south;
            }
        }

        public Path WestPath
        {
            set
            {
                west = value;
            }

            get
            {
                return west;
            }
        }

        public Path EastPath
        {
            set
            {
                east = value;
            }

            get
            {
                return east;
            }
        }

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }
    }
}
