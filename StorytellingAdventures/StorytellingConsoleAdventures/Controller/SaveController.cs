using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using StorytellingConsoleAdventures.Model;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Controller
{
    static class SaveController
    {
        private static string FILENAME = "save.json";

        public static void Save(World world)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(world);
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
                world = JsonSerializer.Deserialize<World>(jsonString);
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
    }
}
