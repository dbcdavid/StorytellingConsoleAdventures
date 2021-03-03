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
        private string name = "";
        private Path north = null;
        private Path south = null;
        private Path west = null;
        private Path east = null;

        public Location(string name)
        {
            this.name = name;
        }

        public Path North
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

        public Path South
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

        public Path West
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

        public Path East
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
