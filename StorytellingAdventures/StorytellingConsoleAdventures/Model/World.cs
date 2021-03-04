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
        private Monster monster = null;

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

        public Monster MonsterCharacter
        {
            set
            {
                monster = value;
            }

            get
            {
                return monster;
            }
        }

        public bool ExecuteAction(string[] actionDescription, ref string message)
        {
            bool result = false;
            if (actionDescription.Length > 1)
            {
                string actor = actionDescription[0];
                string actionName = actionDescription[1];
                Entity entity = null;

                if (actor.Equals("player"))
                {
                    entity = player;
                }
                else if (actor.Equals("monster"))
                {
                    entity = monster;
                }

                if (entity != null)
                {
                    if (actionName.Equals("north") || actionName.Equals("south") || actionName.Equals("east") || actionName.Equals("west"))
                    {
                        result = MoveEntity(entity, actionName, ref message);
                    }
                }
            }

            return result;
        }

        private bool MoveEntity(Entity entity, string direction, ref string message)
        {
            Location entityLocation = entity.CurrentLocation;
            Location destination = null;
            switch (direction) {
                case "north":
                    destination = entityLocation.GetDirection(Location.Direction.NORTH);
                    break;
                case "south":
                    destination = entityLocation.GetDirection(Location.Direction.SOUTH);
                    break;
                case "east":
                    destination = entityLocation.GetDirection(Location.Direction.EAST);
                    break;
                case "west":
                    destination = entityLocation.GetDirection(Location.Direction.WEST);
                    break;
            }

            if (destination != null)
            {
                message = Messages.MOVEMESSAGE;

                message = message.Replace("%entity", entity.Name);
                message = message.Replace("%from", entity.CurrentLocation.Name);
                message = message.Replace("%to", destination.Name);
                entity.CurrentLocation = destination;

                return true;
            }

            message = Messages.MOVEFAILMESSAGE;
            message = message.Replace("%entity", entity.Name);
            message = message.Replace("%direction", direction);

            return false;
        }
    }
}
