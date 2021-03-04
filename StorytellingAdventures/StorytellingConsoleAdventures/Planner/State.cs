using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.Model;

namespace StorytellingConsoleAdventures.Planner
{
    class State
    {
        private string previousAction = String.Empty;
        private State previousState = null;
        private List<State> nextStates = null;
        Location plannedLocation = null;

        public State(Location location)
        {
            nextStates = new List<State>();
            plannedLocation = location;
        }

        public void AddNextState(State state)
        {
            nextStates.Add(state);
        }

        public string GetFirstAction()
        {
            if (previousState != null)
            {
                string previousStateAction = previousState.previousAction;
                if (previousStateAction != String.Empty)
                {
                    return previousState.GetFirstAction();
                }
                else
                {
                    return previousAction;
                }
            }
            else
            {
                return previousAction;
            }
        }

        public Location PlannedLocation
        {
            get
            {
                return plannedLocation;
            }
        }

        public string PreviousAction
        {
            get
            {
                return previousAction;
            }

            set
            {
                previousAction = value;
            }
        }

        public State PreviousState
        {
            get
            {
                return previousState;
            }

            set
            {
                previousState = value;
            }
        }
    }
}
