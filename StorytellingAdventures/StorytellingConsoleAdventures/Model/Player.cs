using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// Player is the class that describes the relevant attributes of the player character
    /// It inherits all attributes from the Entity class
    /// </summary>
    
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
