using System;
using StorytellingConsoleAdventures.Controller;
using StorytellingConsoleAdventures.Model;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures
{
    class Program
    {
        private static bool debug = true;

        static void Main(string[] args)
        {
            ScreenController.GoToFullScreen();
            GameLoop();
            Console.ReadLine();
        }

        static void Initialize(ref World world, ref Player player, ref Monster monster)
        {
            world = InitializeTestScenario();
            player = world.PlayerCharacter;
            monster = world.MonsterCharacter;
        }

        static void GameLoop()
        {
            World world = null;
            Player player = null;
            Monster monster = null;

            string commandLine = "";
            string[] commandTokens = null;
            string message = "";

            Initialize(ref world, ref player, ref monster);

            while (commandLine != Commands.EXIT)
            {
                bool isPlayerNearMonster = World.CheckEntityProximity(player, monster);
                bool isPlayerWithMonster = player.CurrentLocation == monster.CurrentLocation;

                if (isPlayerNearMonster)
                {
                    Console.WriteLine(Messages.PLAYERNEARMONSTER);
                }
                else if (isPlayerWithMonster)
                {
                    Console.WriteLine(Messages.PLAYERWITHMONSTER);
                }

                if (debug)
                {
                    Console.WriteLine("Player is at: " + player.CurrentLocation.Name);
                }

                commandLine = Console.ReadLine().ToLower();
                bool isValid = Parser.ParseAction(commandLine, ref commandTokens);

                if (commandTokens[0].Equals(Commands.SAVE))
                {
                    SaveController.Save(world);
                    continue;
                }
                else if (commandTokens[0].Equals(Commands.LOAD))
                {
                    bool loaded = SaveController.Load(world);
                    if (loaded)
                    {
                        player = world.PlayerCharacter;
                        monster = world.MonsterCharacter;
                        commandLine = "";
                        commandTokens = null;
                        message = "";
                    }

                    continue;
                }

                if (isValid)
                {
                    bool executed = world.ExecuteAction(player, commandTokens, ref message);
                    Console.WriteLine(message);

                    if (executed)
                    {
                        bool reachedMonsterTurn = world.IncreasePlayerActionCount();

                        if (!monster.IsAlive())
                        {
                            bool continueGame = HandleSuccess();
                            if (continueGame)
                            {
                                Initialize(ref world, ref player, ref monster);
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (isPlayerWithMonster)
                        {
                            player.LoseLife();

                            if (!player.IsAlive())
                            {
                                bool continueGame = HandleGameOver();

                                if (continueGame)
                                {
                                    Initialize(ref world, ref player, ref monster);
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        if (reachedMonsterTurn)
                        {
                            bool hasAction = monster.GetNextAction(ref commandTokens);
                            if (hasAction)
                            {
                                world.ExecuteAction(monster, commandTokens, ref message);

                                if (debug)
                                {
                                    Console.WriteLine(message);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine();
            }
        }

        static bool HandleGameOver()
        {
            string response = string.Empty;
            Console.WriteLine(Messages.GAMEOVERMESSAGE);
            Console.WriteLine(Messages.CONTINUEQUESTION);

            while (response != Commands.AFIRMATIVE && response != Commands.AFIRMATIVECOMPLETE &&
                   response != Commands.NEGATIVE && response != Commands.NEGATIVECOMPLETE) {
                response = Console.ReadLine();
            }

            if (response == Commands.AFIRMATIVE || response == Commands.AFIRMATIVECOMPLETE)
            {
                Console.WriteLine(Messages.CONTINUEMESSAGE);
                return true;
            }

            if (response == Commands.NEGATIVE || response == Commands.NEGATIVECOMPLETE)
            {
                Console.WriteLine(Messages.NOTCONTINUEMESSAGE);
                return false;
            }

            return false;
        }

        static bool HandleSuccess()
        {
            string response = string.Empty;
            Console.WriteLine(Messages.SUCCESSMESSAGE);
            Console.WriteLine(Messages.PLAYAGAINQUESTION);

            while (response != Commands.AFIRMATIVE && response != Commands.AFIRMATIVECOMPLETE &&
                   response != Commands.NEGATIVE && response != Commands.NEGATIVECOMPLETE)
            {
                response = Console.ReadLine();
            }

            if (response == Commands.AFIRMATIVE || response == Commands.AFIRMATIVECOMPLETE)
            {
                Console.WriteLine(Messages.REPLAYMESSAGE);
                return true;
            }

            if (response == Commands.NEGATIVE || response == Commands.NEGATIVECOMPLETE)
            {
                Console.WriteLine(Messages.NOTCONTINUEMESSAGE);
                return false;
            }

            return false;
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

            Path a3b3 = new Path(a3, b3);

            Path b1b2 = new Path(b1, b2);
            Path b1c1 = new Path(b1, c1);

            Path b3c3 = new Path(b3, c3);

            Path c1c2 = new Path(c1, c2);

            Path c2c3 = new Path(c2, c3);

            a1.AddPath(Commands.EAST, a1a2);
            a1.AddPath(Commands.SOUTH, a1b1);

            a2.AddPath(Commands.EAST, a2a3);
            a2.AddPath(Commands.WEST, a1a2);

            a3.AddPath(Commands.WEST, a2a3);
            a3.AddPath(Commands.SOUTH, a3b3);

            b1.AddPath(Commands.NORTH, a1b1);
            b1.AddPath(Commands.EAST, b1b2);
            b1.AddPath(Commands.SOUTH, b1c1);

            b2.AddPath(Commands.WEST, b1b2);

            b3.AddPath(Commands.NORTH, a3b3);
            b3.AddPath(Commands.SOUTH, b3c3);

            c1.AddPath(Commands.EAST, c1c2);
            c1.AddPath(Commands.NORTH, b1c1);

            c2.AddPath(Commands.EAST, c2c3);
            c2.AddPath(Commands.WEST, c1c2);

            c3.AddPath(Commands.WEST, c2c3);
            c3.AddPath(Commands.NORTH, b3c3);

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

            Monster monster = new Monster("Monster", 5, c3, world, Monster.Planning.CHASE);
            world.MonsterCharacter = monster;

            Item sword = new Item("Sword", "Attack");
            b2.AddItem(sword);
            world.AddItem(sword);

            Item key = new Item("Key", "Unlock");
            b1.AddItem(key);
            world.AddItem(key);

            Obstacle door = new Obstacle("door", "locked", false, key);
            b1b2.PathObstacle = door;
            return world;
        }
    }
}
