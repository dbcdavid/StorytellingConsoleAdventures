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

        public Location(string name)
        {
            this.name = name;
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
    }
}
