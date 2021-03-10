using System;
using StorytellingConsoleAdventures.Controller;

namespace StorytellingConsoleAdventures
{
    class Program
    {
        static void Main(string[] args)
        {
            ScreenController.GoToFullScreen();
            GameController gameController = new GameController();
            gameController.GameLoop();
            Console.ReadLine();
        }
    }
}
