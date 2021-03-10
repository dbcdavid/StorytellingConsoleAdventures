using System;
using StorytellingConsoleAdventures.Model;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Controller
{
    class GameController
    {
        private World world = null;
        private Player player = null;
        private Monster monster = null;
        private bool debug = true;
        private Location previousPlayerLocation = null;
        private Location currentPlayerLocation = null;
        private string previousCommand = "";

        public GameController()
        {
            Initialize();
        }

        public void GameLoop()
        {
            string commandLine = "";
            string[] commandTokens = null;
            string message = "";

            while (commandLine != Commands.EXIT)
            {
                currentPlayerLocation = world.PlayerCharacter.CurrentLocation;
                if (currentPlayerLocation != previousPlayerLocation)
                {
                    WriteLocationDescription();
                    previousPlayerLocation = currentPlayerLocation;
                }

                WritePlayerMonsterCondition();

                if (debug)
                {
                    Console.WriteLine("Player is at: " + player.CurrentLocation.Name);
                }

                commandLine = Console.ReadLine().ToLower();
                bool isValid = Parser.ParseAction(commandLine, ref commandTokens, ref message);

                if (isValid)
                {
                    bool specialCommand = ExecuteSpecialCommands(commandTokens);

                    if (specialCommand)
                    {
                        continue;
                    }

                    bool takeDamage = TakeDamageCondition();
                    bool executed = world.ExecuteAction(player, commandTokens, ref message);
                    
                    if (executed)
                    {
                        if (ReachedGoodEnding())
                        {
                            bool continueGame = HandleSuccess();
                            if (continueGame)
                            {
                                Console.ReadLine();
                                Initialize();
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (takeDamage)
                        {
                            player.LoseLife();
                            WriteMonsterAttack();
                            WritePlayerLifeCondition();

                            if (ReachedBadEnding())
                            {
                                bool continueGame = HandleGameOver();

                                if (continueGame)
                                {
                                    Console.ReadLine();
                                    Initialize();
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        Console.WriteLine(message);

                        world.IncreasePlayerActionCount();
                        if (world.IsMonsterTurn())
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
                    else
                    {
                        Console.WriteLine(message);
                    }
                }
                else
                {
                    Console.WriteLine(message);
                }

                Console.WriteLine();
            }
        }

        private bool TakeDamageCondition()
        {
            if (player.CurrentLocation == monster.CurrentLocation)
            {
                return true;
            }

            return false;
        }

        private bool ReachedGoodEnding()
        {
            if (!monster.IsAlive())
            {
                return true;
            }

            return false;
        }

        private bool ReachedBadEnding()
        {
            if (!player.IsAlive())
            {
                return true;
            }

            return false;
        }

        private bool ExecuteSpecialCommands(string[] commandTokens) 
        {
            if (commandTokens.Length > 0)
            {
                if (commandTokens[0].Equals(Commands.SAVE))
                {
                    SaveController.Save(world);

                    return true;
                }
                else if (commandTokens[0].Equals(Commands.LOAD))
                {
                    World loadedWorld = SaveController.Load();
                    if (loadedWorld != null)
                    {
                        world = loadedWorld;
                        player = world.PlayerCharacter;
                        monster = world.MonsterCharacter;

                        Console.ReadLine();
                        Console.Clear();

                        return true;
                    }
                }
            }

            return false;
        }

        private void WriteLocationDescription()
        {
            Location location = player.CurrentLocation;
            Console.WriteLine(location.Name.ToUpper());
            Console.WriteLine(location.Description);
        }

        private void WriteMonsterAttack()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Messages.MONSTERATTACK);
            Console.ForegroundColor = defaultColor;
        }

        private bool IsPlayerWithMonster()
        {
            bool isPlayerWithMonster = false;

            if (player.CurrentLocation == monster.CurrentLocation)
            {
                isPlayerWithMonster = true;
            }

            return isPlayerWithMonster;
        }

        private void WritePlayerMonsterCondition()
        {
            bool isPlayerNearMonster = World.CheckEntityProximity(player, monster);
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            
            if (isPlayerNearMonster)
            {
                Console.WriteLine(Messages.PLAYERNEARMONSTER);
            }
            else if (IsPlayerWithMonster())
            {
                Console.WriteLine(Messages.PLAYERWITHMONSTER);
            }
            Console.ForegroundColor = defaultColor;
        }

        private void WritePlayerLifeCondition()
        {
            if (player.LifePoints > 1)
            {
                Console.WriteLine(Messages.PLAYERINGOODCONDITION);
            }
            else if (player.LifePoints == 1)
            {
                Console.WriteLine(Messages.PLAYERINBADCONDITION);
            }
        }

        private void Initialize()
        {
            world = InitializeTestScenario();
            player = world.PlayerCharacter;
            monster = world.MonsterCharacter;
            Console.Clear();
            WriteWorldIntroduction();
            Console.WriteLine();
        }

        private void WriteWorldIntroduction()
        {
            if (world != null)
            {
                Console.WriteLine(world.Introduction);
            }
        }

        private bool HandleGameOver()
        {
            string response = string.Empty;
            
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Messages.GAMEOVERMESSAGE);
            Console.ForegroundColor = defaultColor;

            Console.WriteLine(Messages.CONTINUEQUESTION);

            while (response != Commands.AFIRMATIVE && response != Commands.AFIRMATIVECOMPLETE &&
                   response != Commands.NEGATIVE && response != Commands.NEGATIVECOMPLETE)
            {
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

        private bool HandleSuccess()
        {
            string response = string.Empty;
            Console.WriteLine(world.Ending);
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

        private World InitializeTestScenario()
        {
            Location a1 = new Location("Entrance");
            Location a2 = new Location("Statue Corridor");
            Location a3 = new Location("Turning Point East");
            a1.Description = "This is the dungeon entrance.\nFrom here, you can see a corridor to the east and a corridor to the south.";
            a2.Description = "As you reach this corridor, you notice a variety of statues. Probably they belonged to the mage that was living here long ago." +
                             "\nFrom here, the corridor continues to the west and to the east.";
            a3.Description = "Here the corridor continues to the south and to the west.";

            Location b1 = new Location("Main Corridor");
            Location b2 = new Location("Mage's Chamber");
            Location b3 = new Location("Portrait Corridor");
            b1.Description = "As you reach this corridor, you notice a door to the east and, incredibly enought, a mat standing on the ground in front of it." +
                             "\nFrom here, the corridor continues to the north and to the south.";
            b2.Description = "You enter the main room of the dungeon and notice the collection of magical items at display.";
            b3.Description = "Walking through this corridor, you see a sequence of portraits. Probably the mage's entire familty. You can go north or south.";

            Location c1 = new Location("Turning Point West");
            Location c2 = new Location("Foul Corridor");
            Location c3 = new Location("Turning Point South");
            c1.Description = "Here the corridor continues to the north and to the east.";
            c2.Description = "Apparently, this was the place where the monster stayed for the most of his time, because you can sense a putrid stench." +
                             "\nYou can go east and west from here.";
            c3.Description = "Here the corridor continues to the north and to the west.";


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
            world.Introduction = "IN SEARCH OF POWER\n" +
                "                 \nDays passed after you decided to get the ring of anger." +
                                 "\nYou needed it in order to have your revenge against that stupid princess and those noble bastards." +
                                 "\nHowever, it would not be an easy task to get the ring. It was in possession of a quite dangerous dragon." +
                                 "\nOne whose scales could only be affected by certain weapons and you found out that one of those weapons was lost inside an old mage's house, now pratically a dungeon." +
                                 "\nYou finally found the dungeon, in the middle of eastern mountains, but before entering, you remembered the legends about a guardian monster" +
                                 "that could be living inside." +
                                 "\nOnly with the weapon that you are searching, you would be able to hurt the creature. Without it, the best you can do is run." +
                                 "\nWith those words in mind, you enter the first room.";

            world.Ending = "You thrust the sword into the guardian monster's belly, and after some vomiting, it drops dead in front of you." +
                            "\nMission complete. At least, this one." +
                            "\nYou put the sword back into the scabbard and head straight to the entrance. There is still much to do before resting.";
            
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
