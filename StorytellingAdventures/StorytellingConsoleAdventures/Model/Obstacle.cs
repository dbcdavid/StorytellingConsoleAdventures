using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /*
     * Obstacle is the class that describes a problem that can be found in a path (a locked door, for instance)
     * each obstacle has a name, it may be solved or not and has an item that references what can be used to solve it
     */
    class Obstacle
    {
        string name = "";
        private bool solved = false;
        private Item solution = null;
    }
}
