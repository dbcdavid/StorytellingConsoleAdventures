using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.Planner;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// Monster is a class that inherits from entity and describes the parameters of the monster that will try to kill the player
    /// It contains a knowledge, which is a copy of the world (but in future iterations it could be different to represent that it may make errors),
    /// a description of planning method, which can be random (it will walk randomly) or chase (it will chase the player),
    /// and a variable that generates the random numbers necessary for the random behavior.
    /// </summary>
    class Monster : Entity
    {
        public enum Planning
        {
            RANDOM, CHASE
        };

        private World knowledge = null;
        private Planning planning = Planning.RANDOM;
        private Random rnd = new Random();

        public Monster(string name, int lifePoints, Location location, World knowledge, Planning planning)
        {
            this.name = name;
            this.location = location;
            this.lifePoints = lifePoints;
            this.knowledge = knowledge;
            this.planning = planning;
            items = new List<Item>();
        }

        /// <summary>
        /// Gets the monster's planned next action.
        /// </summary>
        /// <returns>
        /// A bool that indicates if it was capable of finding an action to execute, and
        /// an array of tokens that describe the monster's intended action.
        /// It returns a list of tokens to make it similar to the player's input.
        /// </returns>
        public bool GetNextAction(ref string[] actionTokens)
        {
            actionTokens = new string[1];
            actionTokens[0] = Think();

            if (actionTokens[0] == string.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Plans the action the monster must execute. The function creates a search tree, performing a broad search to get the best action. 
        /// Given the current map size this approach should not cause a perceivable performance problem.
        /// One optimization was implemented: the planning avoids states already visited.
        /// The planning process currently only considers the move action. In future iterations it could include different actions such as get items.
        /// </summary>
        /// <returns>
        /// A string that describes the chosen action. If no action is found, it returns an empty string.
        /// </returns>
        private string Think()
        {
            if (planning == Planning.RANDOM)
            {
                List<string> possibilities = location.GetPossibleDirections();
                int randomIndex = rnd.Next(0, possibilities.Count);

                string direction = possibilities[randomIndex];
                return direction;
            }
            else if (planning == Planning.CHASE)
            {
                List<State> visitedStates = new List<State>();
                Queue<State> plannedStates = new Queue<State>();

                State currentState = new State(location);

                plannedStates.Enqueue(currentState);

                while (plannedStates.Count > 0)
                {
                    State plannedState = plannedStates.Dequeue();
                    Location plannedLocation = plannedState.PlannedLocation;

                    bool visitedBefore = false;
                    foreach(State visitedState in visitedStates)
                    {
                        if (visitedState.PlannedLocation == plannedLocation)
                        {
                            visitedBefore = true;
                            break;
                        }
                    }

                    if (visitedBefore)
                    {
                        continue;
                    }

                    if (plannedLocation == knowledge.PlayerCharacter.CurrentLocation)
                    {
                        return plannedState.GetFirstAction();
                    }
                    else
                    {
                        List<string> possibilities = plannedLocation.GetPossibleDirections();
                        foreach(string possibility in possibilities)
                        {
                            Location newLocation = plannedLocation.GetDestination(possibility);
                            State newState = new State(newLocation);
                            newState.PreviousAction = possibility;
                            newState.PreviousState = plannedState;
                            plannedState.AddNextState(newState);
                            plannedStates.Enqueue(newState);
                        }

                        visitedStates.Add(plannedState);
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Handler of the monster's planning method.
        /// </summary>
        public Planning PlanningMethod
        {
            get
            {
                return planning;
            }

            set
            {
                planning = value;
            }
        }
    }
}
