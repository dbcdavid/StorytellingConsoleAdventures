using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /*
     * Player is the class that describes the relevant attributes of the player character
     * It contains the player's current life points, current location and the items he is carrying
     */
    class Player : Entity
    {
        public Player (string name, int lifePoints, Location location)
        {
            this.name = name;
            this.lifePoints = lifePoints;
            this.location = location;
            items = new List<Item>();
        }
    }
}
