using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using StorytellingConsoleAdventures.Model;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Controller
{
    static class SaveController
    {
        private static string FILENAME = "save.json";

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

        public static bool Load(World world)
        {
            try
            {
                string jsonString = File.ReadAllText(FILENAME);
                WorldSave worldSave = JsonConvert.DeserializeObject<WorldSave>(jsonString);
                world = LoadWorldSave(worldSave);
                Console.WriteLine(Messages.LOADSUCCESSFUL);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Messages.LOADFAILED);
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        private static WorldSave CreateSaveObject(World world)
        {
            WorldSave worldSave = new WorldSave();
            worldSave.playerActionCount = world.PlayerActionCount;
            worldSave.introduction = world.Introduction;

            foreach (Item item in world.Items)
            {
                ItemSave itemSave = new ItemSave();
                itemSave.name = item.Name;
                itemSave.effect = item.Effect;

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

            foreach (Location location in world.Map)
            {
                LocationSave locationSave = new LocationSave();
                locationSave.name = location.Name;
                locationSave.description = location.Description;

                foreach (Item item in location.Items)
                {
                    locationSave.items.Add(item.Name);
                }

                Dictionary<string, Model.Path> paths = location.Paths;

                foreach (KeyValuePair<string, Model.Path> path in location.Paths)
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

        private static World LoadWorldSave(WorldSave worldSave)
        {
            List<LocationSave> mapSave = worldSave.map;
            EntitySave playerSave = worldSave.player;
            MonsterSave monsterSave = worldSave.monster;
            List<Location> map = new List<Location>();
            Location playerLocation = null;
            Location monsterLocation = null;
            
            foreach (LocationSave locationSave in mapSave)
            {
                Location location = new Location(locationSave.name);
                location.Description = locationSave.description;
                map.Add(location);

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
            Monster monster = new Monster(monsterSave.name, monsterSave.lifePoints, monsterLocation, world, monsterSave.planning);
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

            foreach (LocationSave locationSave in worldSave.map)
            {
                Location location = map.Find(l => l.Name.Equals(locationSave.name));
                
                foreach (string itemName in locationSave.items)
                {
                    Item item = world.Items.Find(i => i.Name.Equals(itemName));
                    location.AddItem(item);
                }

                Dictionary<string, PathSave> pathsSaves = locationSave.paths;
                List<Model.Path> paths = new List<Model.Path>();

                foreach (KeyValuePair<string, PathSave> pathSavePair in pathsSaves)
                {
                    Model.Path path = null;
                    PathSave pathSave = pathSavePair.Value;
                    path = paths.Find(p => p.Location1.Name.Equals(pathSave.location1));
                    
                    if (path == null)
                    {
                        Location location1 = map.Find(l => l.Name.Equals(pathSave.location1));
                        Location location2 = map.Find(l => l.Name.Equals(pathSave.location2));

                        path = new Model.Path(location1, location2);

                        ObstacleSave obstacleSave = pathSave.obstacleSave;
                        if (obstacleSave != null)
                        {
                            Item solution = world.Items.Find(i => i.Name.Equals(obstacleSave.solution));
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
