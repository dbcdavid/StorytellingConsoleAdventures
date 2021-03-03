using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Model
{
    class World
    {
        private List<Location> map = null;
        private List<Item> items = null;
        private Player player = null;

        public World(Player player)
        {
            this.player = player;
            items = new List<Item>();
            map = new List<Location>();
        }

        public void AddLocation(Location location)
        {
            map.Add(location);
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public Player PlayerCharacter
        {
            get
            {
                return player;
            }
        }

        public bool ExecuteAction(string[] actionDescription, ref string message)
        {
            bool result = false;
            if (actionDescription.Length > 0)
            {
                string actionName = actionDescription[0];
                if (actionName.Equals("north") || actionName.Equals("south") || actionName.Equals("east") || actionName.Equals("west"))
                {
                    result = MovePlayer(actionName, ref message);
                }
            }

            return result;
        }

        private bool MovePlayer(string direction, ref string message)
        {
            Location playerLocation = player.CurrentLocation;
            Location destination = null;
            switch (direction) {
                case "north":
                    destination = playerLocation.GetDirection(Location.Direction.NORTH);
                    break;
                case "south":
                    destination = playerLocation.GetDirection(Location.Direction.SOUTH);
                    break;
                case "east":
                    destination = playerLocation.GetDirection(Location.Direction.EAST);
                    break;
                case "west":
                    destination = playerLocation.GetDirection(Location.Direction.WEST);
                    break;
            }

            if (destination != null)
            {
                message = Messages.MOVEMESSAGE;
                message = message.Replace("%from", player.CurrentLocation.Name);
                message = message.Replace("%to", destination.Name);

                player.CurrentLocation = destination;
                return true;
            }

            message = Messages.MOVEFAILMESSAGE;
            message = message.Replace("%direction", direction);
            return false;
        }
    }
}
