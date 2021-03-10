using System;
using StorytellingConsoleAdventures.Controller;

namespace StorytellingConsoleAdventures
{
    /// <summary>
    /// Class used to init the game and make the initial call to the GameController.
    /// </summary>
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
