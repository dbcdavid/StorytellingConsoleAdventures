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
        private int playerActionCount = 0;

        public World(Player player)
        {
            this.player = player;
            items = new List<Item>();
            map = new List<Location>();
        }

        public void IncreasePlayerActionCount()
        {
            playerActionCount = (playerActionCount + 1) % Constants.MAXACTIONCOUNT;
        }

        public bool IsMonsterTurn()
        {
            if (playerActionCount == 0)
            {
                return true;
            }

            return false;
        }

        public bool AddLocation(Location newLocation)
        {
            foreach(Location location in map)
            {
                if (location.Name.Equals(newLocation.Name))
                {
                    return false;
                }
            }

            map.Add(newLocation);
            return true;
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

        public Item GetItem(string itemName)
        {
            foreach(Item item in items)
            {
                if (item.Name.ToLower().Equals(itemName))
                {
                    return item;
                }
            }

            return null;
        }

        public static bool CheckEntityProximity(Entity entity1, Entity entity2)
        {
            Location location1 = entity1.CurrentLocation;
            Location location2 = entity2.CurrentLocation;

            bool isNear = location1.IsNear(location2);

            return isNear;
        }

        public bool ExecuteAction(Entity actor, string[] actionDescription, ref string message)
        {
            bool result = false;
            if (actionDescription.Length > 0)
            {
                string actionName = actionDescription[0];

                if (actor != null)
                {
                    if (actionName.Equals(Commands.NORTH) || actionName.Equals(Commands.SOUTH) || actionName.Equals(Commands.EAST) || actionName.Equals(Commands.WEST))
                    {
                        result = MoveEntity(actor, actionName, ref message);
                    }
                    else if (actionName.Equals(Commands.SEARCH))
                    {
                        List<Item> itemsFound = actor.CurrentLocation.Items;

                        if (itemsFound.Count > 0)
                        {
                            message = actor.Name + " " + Messages.FOUNDMESSAGE + ":";
                            foreach (Item item in itemsFound)
                            {
                                message += " " + item.Name;
                            }
                        }
                        else
                        {
                            message = Messages.EMPTYLOCATIONMESSAGE;
                        }

                        return true;
                    }
                    else if (actionName.Equals(Commands.GET))
                    {
                        string target = actionDescription[1];
                        bool gotItem = actor.CurrentLocation.RemoveItem(target);

                        if (gotItem)
                        {
                            Item item = GetItem(target);
                            message = actor.Name + " " + Messages.GOTITEMMESSAGE + " " + target;
                            actor.AddItem(item);
                        }
                        else
                        {
                            message = actor.Name + " " + Messages.DIDNOTGOTITEMMESSAGE + " " + target;
                        }

                        return (gotItem);
                    }
                    else if (actionName.Equals(Commands.USE))
                    {
                        string itemName = actionDescription[1];
                        if (actor.HasItem(itemName))
                        {
                            Item item = GetItem(itemName);
                            Object[] parameters = new Object[2];
                            parameters[0] = actor;
                            if (actor == player)
                            {
                                parameters[1] = monster;
                            }
                            else
                            {
                                parameters[1] = player;
                            }

                            bool used = item.Use(parameters, ref message);
                            return used;
                        }
                        else
                        {
                            message = actor.Name + " " + Messages.HASNOITEM + " " + itemName;
                            return false;
                        }
                    }
                }
            }

            return result;
        }

        private bool MoveEntity(Entity entity, string direction, ref string message)
        {
            Location entityLocation = entity.CurrentLocation;
            Path path = entityLocation.GetPath(direction);

            if (path != null)
            {
                if (!path.HasUnsolvedObstacle())
                {
                    Location destination = path.GetDestination(entityLocation);
                    message = Messages.MOVEMESSAGE;

                    message = message.Replace("%entity", entity.Name);
                    message = message.Replace("%from", entityLocation.Name);
                    message = message.Replace("%to", destination.Name);
                    entity.CurrentLocation = destination;

                    return true;
                }
                else
                {
                    Obstacle obstacle = path.PathObstacle;
                    message = "A " + obstacle.Condition + " " + obstacle.Name + " blocks your way";

                    return false;
                }
            }

            message = Messages.MOVEFAILMESSAGE;
            message = message.Replace("%entity", entity.Name);
            message = message.Replace("%direction", direction);

            return false;
        }

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }

        public List<Location> Map
        {
            get
            {
                return map;
            }
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

        public int PlayerActionCount
        {
            set
            {
                playerActionCount = value;
            }
            get
            {
                return playerActionCount;
            }
        }
    }
}
