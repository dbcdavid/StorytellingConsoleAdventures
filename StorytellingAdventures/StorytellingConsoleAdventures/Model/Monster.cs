using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.Planner;

namespace StorytellingConsoleAdventures.Model
{
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
