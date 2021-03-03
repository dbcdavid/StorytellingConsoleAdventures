using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /*
     * Path is the class that describes the connection between two locations
     * Currently, the program considers that paths are simmetric, which means that going from A to B is the same as going from B to A
     * It contains two locations (the locations that are connected by the path) and an item that may represent an obstacle in the path
     */ 
    class Path
    {
        private Location location1 = null;
        private Location location2 = null;
        private Obstacle obstacle = null;

        public Path (Location location1, Location location2)
        {
            this.location1 = location1;
            this.location2 = location2;
        }
    }
}
