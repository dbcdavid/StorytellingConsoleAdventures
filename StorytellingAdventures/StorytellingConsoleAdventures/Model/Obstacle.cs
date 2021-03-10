using System;

namespace StorytellingConsoleAdventures.Model
{
    /*
     * Obstacle is the class that describes a problem that can be found in a path (a locked door, for instance)
     * each obstacle has a name, it may be solved or not and has an item that references what can be used to solve it
     */
    class Obstacle
    {
        private string name = "";
        private string condition = "";
        private bool solved = false;
        private Item solution = null;

        public Obstacle (string name, string condition, bool solved, Item solution)
        {
            this.name = name;
            this.condition = condition;
            this.solved = solved;
            this.solution = solution;
        }

        public bool Solve(Item possibleSolution)
        {
            if (possibleSolution == solution)
            {
                solved = true;
            }

            return solved;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Condition
        {
            get
            {
                return condition;
            }

            set
            {
                condition = value;
            }
        }

        public bool Solved
        {
            get
            {
                return solved;
            }
        }

        public Item Solution
        {
            get
            {
                return solution;
            }
        }
    }
}
