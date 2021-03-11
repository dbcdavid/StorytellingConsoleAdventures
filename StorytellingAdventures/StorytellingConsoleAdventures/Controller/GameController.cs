using System;
using StorytellingConsoleAdventures.Model;
using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Controller
{
    /// <summary>
    /// Class responsible of controlling the main game elements, the game loop, conditions of continuing and finishing.
    /// This version also contains a function that creates an example map to test the game.
    /// </summary>
    class GameController
    {
        private World world = null;
        private Player player = null;
        private Monster monster = null;
        private Location previousPlayerLocation = null;
        private Location currentPlayerLocation = null;

        public GameController()
        {
            Initialize();
        }

        /// <summary>
        /// Function that controls the main execution of the game, with the conditions of starting, continuing and ending
        /// </summary>
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

                if (Constants.DEBUG)
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
                    bool executed = world.ExecuteAction(player, IsPlayerWithMonster(), commandTokens, ref message);
                    
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
                                world.ExecuteAction(monster, IsPlayerWithMonster(), commandTokens, ref message);

                                if (Constants.DEBUG)
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

        /// <summary>
        /// Checks if the player is in condition of taking damage this turn.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the player must take damage.
        /// </returns>
        private bool TakeDamageCondition()
        {
            if (player.CurrentLocation == monster.CurrentLocation)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the game has reached the conditions of the good ending.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the good ending conditions were met.
        /// </returns>
        private bool ReachedGoodEnding()
        {
            if (!monster.IsAlive())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the game has reached the conditions of the bad ending.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the bad ending conditions were met.
        /// </returns>
        private bool ReachedBadEnding()
        {
            if (!player.IsAlive())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the game received a special command (which are save and load).
        /// </summary>
        /// <returns>
        /// A bool that indicates if the game received a special command.
        /// </returns>
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

        /// <summary>
        /// Writes in console the description of the player current location.
        /// </summary>
        private void WriteLocationDescription()
        {
            Location location = player.CurrentLocation;
            Console.WriteLine(location.Name.ToUpper());
            Console.WriteLine(location.Description);
        }

        /// <summary>
        /// Writes in console the description of the monster attack.
        /// </summary>
        private void WriteMonsterAttack()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Messages.MONSTERATTACK);
            Console.ForegroundColor = defaultColor;
        }

        /// <summary>
        /// Checks if the player is currently at the same place as the monster.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the player is at the same plce as the monster.
        /// </returns>
        private bool IsPlayerWithMonster()
        {
            bool isPlayerWithMonster = false;

            if (player.CurrentLocation == monster.CurrentLocation)
            {
                isPlayerWithMonster = true;
            }

            return isPlayerWithMonster;
        }

        /// <summary>
        /// Writes in console the proximity condition between the player and the monster.
        /// </summary>
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

        /// <summary>
        /// Writes in console the message related to the player life condition.
        /// </summary>
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

        /// <summary>
        /// Initializes the game conditions: the main variables of the game loop (world, player, monster) and initial text.
        /// </summary>
        private void Initialize()
        {
            world = InitializeTestScenario();
            player = world.PlayerCharacter;
            monster = world.MonsterCharacter;
            Console.Clear();
            WriteWorldIntroduction();
            Console.WriteLine();
        }

        /// <summary>
        /// Writes in console the world introduction.
        /// </summary>
        private void WriteWorldIntroduction()
        {
            if (world != null)
            {
                Console.WriteLine(world.Introduction);
            }
        }

        /// <summary>
        /// Checks if the game reached the game over condition, writes the corresponding messages and checks if the player wants to try again.
        /// </summary>
        /// /// <returns>
        /// A bool that indicates if the player wants to try again.
        /// </returns>
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

        /// <summary>
        /// Checks if the game reached the success condition, writes the corresponding messages and checks if the player wants to play again.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the player wants to play again.
        /// </returns>
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

        /// <summary>
        /// Function that creates a test world to play the game.
        /// </summary>
        /// <returns>
        /// The world created for test.
        /// </returns>
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


            LocationPath a1a2 = new LocationPath(a1, a2);
            LocationPath a1b1 = new LocationPath(a1, b1);

            LocationPath a2a3 = new LocationPath(a2, a3);

            LocationPath a3b3 = new LocationPath(a3, b3);

            LocationPath b1b2 = new LocationPath(b1, b2);
            LocationPath b1c1 = new LocationPath(b1, c1);

            LocationPath b3c3 = new LocationPath(b3, c3);

            LocationPath c1c2 = new LocationPath(c1, c2);

            LocationPath c2c3 = new LocationPath(c2, c3);

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

            Player player = new Player("You", 3, a1);

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
