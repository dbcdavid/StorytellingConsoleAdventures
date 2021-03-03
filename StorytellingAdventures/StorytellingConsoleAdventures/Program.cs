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
            Console.WriteLine("Hello World!");
            ScreenController.GoToFullScreen();
            GameLoop();
            Console.ReadLine();
        }

        static void GameLoop()
        {
            InitializeTestScenario();
            string actionLine = "";
            string[] tokens = null;

            while (actionLine != "exit")
            {
                Console.WriteLine("Player is at: " + player);
                actionLine = Console.ReadLine();
                bool isValid = Parser.ParseAction(actionLine, ref tokens);

                if (isValid)
                {
                    ExecuteAction(tokens);
                }
            }
        }

        static void InitializeTestScenario()
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

            a1.East = a1a2;
            a1.South = a1b1;

            a2.East = a2a3;
            a2.West = a1a2;
            a2.South = a2b2;

            a3.West = a2a3;
            a3.South = a3b3;

            b1.North = a1b1;
            b1.East = b1b2;
            b1.South = b1c1;

            b2.North = a2b2;
            b2.East = b2b3;
            b2.West = b1b2;
            b2.South = b2c2;

            b3.North = a3b3;
            b3.West = b2b3;
            b3.South = b3c3;

            c1.East = c1c2;
            c1.North = b1c1;

            c2.East = c2c3;
            c2.West = c1c2;
            c2.North = b2c2;

            c3.West = c2c3;
            c3.North = b3c3;

            Player player = new Player(3, a1);
        }
    }
}
