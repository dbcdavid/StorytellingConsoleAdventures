using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures
{
    /// <summary>
    /// Class with a description of all constants available to the game.
    /// </summary>
    static class Constants
    {
        public static int MAXACTIONCOUNT = 2; //How many actions the player may execute before the monster acts
        public static bool DEBUG = false;     //The game must show the current player location and the monster's movements?
    }
}
