using System;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// Obstacle is the class that describes a problem that can be found in a path (a locked door, for instance)
    /// each obstacle has a name, a condition (a text that describes the situation), 
    /// it may be solved or not and has an item that references what can be used to solve it
    /// </summary>
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

        /// <summary>
        /// Checks if the given item is the solution and, if so, changes the condition of the obstacle to solved.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the obstacle was solved
        /// </returns>
        public bool Solve(Item possibleSolution)
        {
            if (possibleSolution == solution)
            {
                solved = true;
            }

            return solved;
        }

        /// <summary>
        /// Handler of the obstacle's name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Handler of the obstacle's condition description.
        /// </summary>
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

        /// <summary>
        /// Handler of the obstacle's indicator of blocking.
        /// </summary>
        public bool Solved
        {
            get
            {
                return solved;
            }
        }

        /// <summary>
        /// Handler of the obstacle's item that solves the blocking problem.
        /// </summary>
        public Item Solution
        {
            get
            {
                return solution;
            }
        }
    }
}
