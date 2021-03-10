using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// LocationPath is the class that describes the connection between two locations
    /// Currently, the program considers that paths are simmetric, which means that going from A to B is the same as going from B to A
    /// It contains two locations (the locations that are connected by the path) and an obstacle that may represent a problem that blocks the path
    /// If there is no obstacle, the variable will be null, if there is an obstacle it will contain an instance to the corresponding class
    /// </summary>
    class LocationPath
    {
        private Location location1 = null;
        private Location location2 = null;
        private Obstacle obstacle = null;

        public LocationPath (Location location1, Location location2)
        {
            this.location1 = location1;
            this.location2 = location2;
        }

        /// <summary>
        /// Gets the location that is reacheable from the given location.
        /// </summary>
        /// <returns>
        /// The location reacheable from the given location. If there is no such location, it returns null.
        /// </returns>
        public Location GetDestination(Location from)
        {
            if (from == location1 && from != location2)
            {
                return location2;
            }
            else if (from != location1 && from == location2)
            {
                return location1;
            }

            return null;
        }

        /// <summary>
        /// Checks if the given obstacle is the obstacle of this path
        /// </summary>
        /// <returns>
        /// A bool that indicates if the the given obstacle is the obstacle of this path
        /// </returns>
        public bool HasObstacle(Obstacle obstacle)
        {
            if (this.obstacle != null && this.obstacle == obstacle)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if there is an obstacle in this path.
        /// </summary>
        /// <returns>
        /// A bool that indicates if there is an obstacle in this path.
        /// </returns>
        public bool HasObstacle()
        {
            if (obstacle != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if there is an obstacle still blocking this path.
        /// </summary>
        /// <returns>
        /// A bool that indicates if there is an obstacle blocking this path.
        /// </returns>
        public bool HasUnsolvedObstacle()
        {
            if (obstacle != null && !obstacle.Solved)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handler of the path's obstacle.
        /// </summary>
        public Obstacle PathObstacle
        {
            get
            {
                return obstacle;
            }

            set
            {
                obstacle = value;
            }
        }

        /// <summary>
        /// Handler of the path's first location.
        /// </summary>
        public Location Location1
        {
            get
            {
                return location1;
            }
        }

        /// <summary>
        /// Handler of the path's second location.
        /// </summary>
        public Location Location2
        {
            get
            {
                return location2;
            }
        }
    }
}
