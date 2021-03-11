using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// This class is responsible for holding the attributes of worlds, each world representing an entire map and or situation in time.
    /// It contains the texts of introduction and ending, all the locations and items, references to the player and monster
    /// and a variable (playeractioncount) which is used to verify if the monster must act.
    /// </summary>
    class World
    {
        private string introduction = "";
        private string ending = "";
        private Dictionary<string, Location> map = null;
        private Dictionary<string, Item> items = null;
        private Player player = null;
        private Monster monster = null;
        private int playerActionCount = 0;

        public World(Player player)
        {
            this.player = player;
            items = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
            map = new Dictionary<string, Location>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Increases the current playeractioncount. It has a maximum amount determined by the constants. If the maximum is reached, the value resets to zero.
        /// </summary>
        public void IncreasePlayerActionCount()
        {
            playerActionCount = (playerActionCount + 1) % Constants.MAXACTIONCOUNT;
        }

        /// <summary>
        /// Checks if the monster must act now.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the monster must act.
        /// </returns>
        public bool IsMonsterTurn()
        {
            if (playerActionCount == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a location to the list of locations that can be found in the world.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the location was added (it is not possible to have two locations of the same name, which must be handled during the world creation).
        /// </returns>
        public bool AddLocation(Location newLocation)
        {
            if (map.ContainsKey(newLocation.Name) || newLocation.Name.Length == 0)
            {
                return false;
            }

            map.Add(newLocation.Name, newLocation);
            return true;
        }

        /// <summary>
        /// Adds an item to the dictionary of items that can be found in the world.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the item was added (it is not possible to have two items of the same name, which must be handled during the world creation).
        /// </returns>
        public bool AddItem(Item newItem)
        {
            if (items.ContainsKey(newItem.Name) || newItem.Name.Length == 0)
            {
                return false;
            }

            items.Add(newItem.Name, newItem);
            return true;
        }

        /// <summary>
        /// Searches for the given item
        /// </summary>
        /// <returns>
        /// An instance to the item if it is found. Otherwise returns null.
        /// </returns>
        public Item GetItem(string itemName)
        {
            if (items.ContainsKey(itemName))
            {
                return items[itemName];
            }

            return null;
        }

        /// <summary>
        /// Searches for the given location
        /// </summary>
        /// <returns>
        /// An instance to the location if it is found. Otherwise returns null.
        /// </returns>
        public Location GetLocation(string locationName)
        {
            if (map.ContainsKey(locationName))
            {
                return map[locationName];
            }

            return null;
        }

        /// <summary>
        /// Checks if two given entities are in locations connected with a path
        /// </summary>
        /// <returns>
        /// A bool that indicates if the entities are in connected locations
        /// </returns>
        public static bool CheckEntityProximity(Entity entity1, Entity entity2)
        {
            Location location1 = entity1.CurrentLocation;
            Location location2 = entity2.CurrentLocation;

            bool isNear = location1.IsNear(location2);

            return isNear;
        }

        /// <summary>
        /// Changes the world with the given action consequences.
        /// For each possible command there is a specific set of instructions to follow.
        /// If the action "use" is received, this function prepares a set of generic objects for the C# reflection function used inside the Item class.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the action was executed
        /// A string message with the description of the execution attempt.
        /// </returns>
        public bool ExecuteAction(Entity actor, bool isPlayerWithMonster, string[] actionDescription, ref string message)
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
                        if (actor == player && isPlayerWithMonster)
                        {
                            message = Messages.NOACTMONSTERMESSAGE;
                            return false;
                        }

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
                        if (actor == player && isPlayerWithMonster)
                        {
                            message = Messages.NOACTMONSTERMESSAGE;
                            return false;
                        }

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

        /// <summary>
        /// Attempts to change the location of the given actor with the given direction
        /// </summary>
        /// <returns>
        /// A bool that indicates if the action was executed
        /// A string message with the description of the move attempt.
        /// </returns>
        private bool MoveEntity(Entity entity, string direction, ref string message)
        {
            Location entityLocation = entity.CurrentLocation;
            LocationPath path = entityLocation.GetPath(direction);

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

        /// <summary>
        /// Handler of the world's items.
        /// </summary>
        public Dictionary<string, Item> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Handler of the world's locations.
        /// </summary>
        public Dictionary<string, Location> Map
        {
            get
            {
                return map;
            }
        }

        /// <summary>
        /// Handler of the world's player instance.
        /// </summary>
        public Player PlayerCharacter
        {
            get
            {
                return player;
            }
        }

        /// <summary>
        /// Handler of the world's monster instance.
        /// </summary>
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

        /// <summary>
        /// Handler of the world's counting of the player turn.
        /// </summary>
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

        /// <summary>
        /// Handler of the world's introduction text.
        /// </summary>
        public string Introduction
        {
            get
            {
                return introduction;
            }
            set
            {
                introduction = value;
            }
        }

        /// <summary>
        /// Handler of the world's ending text.
        /// </summary>
        public string Ending
        {
            get
            {
                return ending;
            }
            set
            {
                ending = value;
            }
        }
    }
}
