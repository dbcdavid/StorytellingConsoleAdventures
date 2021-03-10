using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using StorytellingConsoleAdventures.Model;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Controller
{
    /// <summary>
    /// Class responsible for saving and loading the condition of the game
    /// </summary>
    static class SaveController
    {
        private static string FILENAME = "save.json";

        /// <summary>
        /// Receives a world and generates the file with complete description of the world
        /// </summary>
        public static void Save(World world)
        {
            WorldSave worldSave = CreateSaveObject(world);

            try
            {
                string jsonString = JsonConvert.SerializeObject(worldSave, Formatting.Indented);
                File.WriteAllText(FILENAME, jsonString);
                Console.WriteLine(Messages.SAVESUCCESSFUL);
            }
            catch(Exception ex)
            {
                Console.WriteLine(Messages.SAVEFAILED);
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Reads a save file and generates a new world instance with the file descriptions.
        /// </summary>
        /// <returns>
        /// A World with the file descriptions
        /// </returns>
        public static World Load()
        {
            try
            {
                string jsonString = File.ReadAllText(FILENAME);
                WorldSave worldSave = JsonConvert.DeserializeObject<WorldSave>(jsonString);
                World world = LoadWorldSave(worldSave);
                Console.WriteLine(Messages.LOADSUCCESSFUL);

                return world;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Messages.LOADFAILED);
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Creates an instance of the WorldSave class with the description of the world passed as parameter.
        /// </summary>
        /// <returns>
        /// The WorldSave instance of the world passed as parameter.
        /// </returns>
        private static WorldSave CreateSaveObject(World world)
        {
            WorldSave worldSave = new WorldSave();
            worldSave.playerActionCount = world.PlayerActionCount;
            worldSave.introduction = world.Introduction;
            worldSave.ending = world.Ending;

            foreach (KeyValuePair<string, Item> itemPair in world.Items)
            {
                ItemSave itemSave = new ItemSave();
                itemSave.name = itemPair.Value.Name;
                itemSave.effect = itemPair.Value.Effect;

                worldSave.items.Add(itemSave);
            }

            Player player = world.PlayerCharacter;
            EntitySave playerSave = new EntitySave();
            playerSave.name = player.Name;
            playerSave.lifePoints = player.LifePoints;
            playerSave.location = player.CurrentLocation.Name;
            
            foreach(Item item in player.Items)
            {
                playerSave.items.Add(item.Name);
            }
            worldSave.player = playerSave;

            Monster monster = world.MonsterCharacter;
            MonsterSave monsterSave = new MonsterSave();
            monsterSave.name = monster.Name;
            monsterSave.lifePoints = monster.LifePoints;
            monsterSave.location = monster.CurrentLocation.Name;
            monsterSave.planning = monster.PlanningMethod;

            foreach (Item item in monster.Items)
            {
                monsterSave.items.Add(item.Name);
            }
            worldSave.monster = monsterSave;

            foreach (KeyValuePair<string, Location> locationPair in world.Map)
            {
                LocationSave locationSave = new LocationSave();
                locationSave.name = locationPair.Value.Name;
                locationSave.description = locationPair.Value.Description;

                foreach (Item item in locationPair.Value.Items)
                {
                    locationSave.items.Add(item.Name);
                }

                foreach (KeyValuePair<string, LocationPath> path in locationPair.Value.Paths)
                {
                    PathSave pathSave = new PathSave();
                    pathSave.location1 = path.Value.Location1.Name;
                    pathSave.location2 = path.Value.Location2.Name;
                    
                    if (path.Value.HasObstacle())
                    {
                        Obstacle obstacle = path.Value.PathObstacle;

                        ObstacleSave obstacleSave = new ObstacleSave();
                        obstacleSave.name = obstacle.Name;
                        obstacleSave.condition = obstacle.Condition;
                        obstacleSave.solved = obstacle.Solved;
                        obstacleSave.solution = obstacle.Solution.Name;

                        pathSave.obstacleSave = obstacleSave;
                    }

                    locationSave.paths.Add(path.Key, pathSave);
                }

                worldSave.map.Add(locationSave);
            }

            return worldSave;
        }

        /// <summary>
        /// Transforms a WorldSave instance into a World instance.
        /// </summary>
        /// <returns>
        /// The World instance obtainable with a given WorldSave instance.
        /// </returns>
        private static World LoadWorldSave(WorldSave worldSave)
        {
            List<LocationSave> mapSave = worldSave.map;
            EntitySave playerSave = worldSave.player;
            MonsterSave monsterSave = worldSave.monster;
            Dictionary<string, Location> map = new Dictionary<string, Location>(StringComparer.OrdinalIgnoreCase);
            Location playerLocation = null;
            Location monsterLocation = null;
            
            foreach (LocationSave locationSave in mapSave)
            {
                Location location = new Location(locationSave.name);
                location.Description = locationSave.description;
                map.Add(location.Name, location);

                if (location.Name.Equals(playerSave.location))
                {
                    playerLocation = location;
                }

                if (location.Name.Equals(monsterSave.location))
                {
                    monsterLocation = location;
                }
            }

            Player player = new Player(playerSave.name, playerSave.lifePoints, playerLocation);
            World world = new World(player);
            world.Introduction = worldSave.introduction;
            world.Ending = worldSave.ending;
            Monster monster = new Monster(monsterSave.name, monsterSave.lifePoints, monsterLocation, world, monsterSave.planning);
            world.MonsterCharacter = monster;
            world.PlayerActionCount = worldSave.playerActionCount;

            foreach (ItemSave itemSave in worldSave.items)
            {
                Item item = new Item(itemSave.name, itemSave.effect);
                world.AddItem(item);
                if (playerSave.items.Contains(itemSave.name))
                {
                    player.AddItem(item);
                }

                if (monsterSave.items.Contains(monsterSave.name))
                {
                    monster.AddItem(item);
                }
            }

            List<LocationPath> paths = new List<LocationPath>();
            foreach (LocationSave locationSave in worldSave.map)
            {
                Location location = map[locationSave.name];
                
                foreach (string itemName in locationSave.items)
                {
                    Item item = world.GetItem(itemName);
                    location.AddItem(item);
                }

                Dictionary<string, PathSave> pathsSaves = locationSave.paths;

                foreach (KeyValuePair<string, PathSave> pathSavePair in pathsSaves)
                {
                    LocationPath path = null;
                    PathSave pathSave = pathSavePair.Value;
                    path = paths.Find(p => p.Location1.Name.Equals(pathSave.location1) && p.Location2.Name.Equals(pathSave.location2));
                    
                    if (path == null)
                    {
                        Location location1 = map[pathSave.location1];
                        Location location2 = map[pathSave.location2];

                        path = new LocationPath(location1, location2);

                        ObstacleSave obstacleSave = pathSave.obstacleSave;
                        if (obstacleSave != null)
                        {
                            Item solution = world.GetItem(obstacleSave.solution);
                            Obstacle obstacle = new Obstacle(obstacleSave.name, obstacleSave.condition, obstacleSave.solved, solution);
                            path.PathObstacle = obstacle;
                        }
                    }
                    paths.Add(path);

                    location.AddPath(pathSavePair.Key, path);
                }

                world.AddLocation(location);
            }

            return world;
        }
    }
}
