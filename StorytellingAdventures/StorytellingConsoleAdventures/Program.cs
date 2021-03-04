using System;
using StorytellingConsoleAdventures.Controller;
using StorytellingConsoleAdventures.Model;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures
{
    class Program
    {
        static void Main(string[] args)
        {
            ScreenController.GoToFullScreen();
            GameLoop();
            Console.ReadLine();
        }

        static void GameLoop()
        {
            World world = InitializeTestScenario();
            Player player = world.PlayerCharacter;
            Monster monster = world.MonsterCharacter;
            string commandLine = "";
            string[] commandTokens = null;
            string message = "";
            int playerActionCount = 0;

            while (commandLine != "exit")
            {
                Console.WriteLine("Player is at: " + player.CurrentLocation.Name);
                commandLine = Console.ReadLine();
                bool isValid = Parser.ParseAction("player " + commandLine, ref commandTokens);

                if (isValid)
                {
                    bool executed = world.ExecuteAction(commandTokens, ref message);
                    Console.WriteLine(message);

                    if (executed)
                    {
                        playerActionCount++;

                        if (playerActionCount >= 2)
                        {
                            commandTokens = monster.GetNextAction();
                            if (commandTokens[1] != string.Empty)
                            {
                                world.ExecuteAction(commandTokens, ref message);
                                Console.WriteLine(message);
                            }
                            playerActionCount = 0;
                        }
                    }
                }

                Console.WriteLine();
            }
        }

        static World InitializeTestScenario()
        {
            Location a1 = new Location("a1");
            Location a2 = new Location("a2");
            Location a3 = new Location("a3");

            Location b1 = new Location("b1");
            Location b2 = new Location("b2");
            Location b3 = new Location("b3");

            Location c1 = new Location("c1");
            Location c2 = new Location("c2");
            Location c3 = new Location("c3");

            Path a1a2 = new Path(a1, a2);
            Path a1b1 = new Path(a1, b1);

            Path a2a3 = new Path(a2, a3);
            Path a2b2 = new Path(a2, b2);

            Path a3b3 = new Path(a3, b3);

            Path b1b2 = new Path(b1, b2);
            Path b1c1 = new Path(b1, c1);

            Path b2b3 = new Path(b2, b3);
            Path b2c2 = new Path(b2, c2);

            Path b3c3 = new Path(b3, c3);

            Path c1c2 = new Path(c1, c2);

            Path c2c3 = new Path(c2, c3);

            a1.EastPath = a1a2;
            a1.SouthPath = a1b1;

            a2.EastPath = a2a3;
            a2.WestPath = a1a2;
            a2.SouthPath = a2b2;

            a3.WestPath = a2a3;
            a3.SouthPath = a3b3;

            b1.NorthPath = a1b1;
            b1.EastPath = b1b2;
            b1.SouthPath = b1c1;

            b2.NorthPath = a2b2;
            b2.EastPath = b2b3;
            b2.WestPath = b1b2;
            b2.SouthPath = b2c2;

            b3.NorthPath = a3b3;
            b3.WestPath = b2b3;
            b3.SouthPath = b3c3;

            c1.EastPath = c1c2;
            c1.NorthPath = b1c1;

            c2.EastPath = c2c3;
            c2.WestPath = c1c2;
            c2.NorthPath = b2c2;

            c3.WestPath = c2c3;
            c3.NorthPath = b3c3;

            Player player = new Player("Player", 3, a1);

            World world = new World(player);
            world.AddLocation(a1);
            world.AddLocation(a2);
            world.AddLocation(a3);

            world.AddLocation(b1);
            world.AddLocation(b2);
            world.AddLocation(b3);

            world.AddLocation(c1);
            world.AddLocation(c2);
            world.AddLocation(c3);

            Monster monster = new Monster("Monster", c3, world, Monster.Planning.CHASE);
            world.MonsterCharacter = monster;

            return world;
        }
    }
}
