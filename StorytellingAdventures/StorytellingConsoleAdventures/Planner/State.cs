using System;
using System.Collections.Generic;
using System.Text;
using StorytellingConsoleAdventures.Model;

namespace StorytellingConsoleAdventures.Planner
{
    /// <summary>
    /// Class used by the monster's planning method to register the possible situations.
    /// It contains a description of the action used to reach it, a reference to the previous state, a list of the reacheable states,
    /// and the location that is being planned.
    /// </summary>
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

        /// <summary>
        /// Adds a state to the list of reacheable states.
        /// </summary>
        public void AddNextState(State state)
        {
            nextStates.Add(state);
        }

        /// <summary>
        /// Goes up to the initial state to check which is the first action to be executed to reach this state.
        /// </summary>
        /// <returns>
        /// A string with the description of the action that need to be executed to reach this state.
        /// </returns>
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

        /// <summary>
        /// Handler of the state's location.
        /// </summary>
        public Location PlannedLocation
        {
            get
            {
                return plannedLocation;
            }
        }

        /// <summary>
        /// Handler of the state's previous action.
        /// </summary>
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

        /// <summary>
        /// Handler of the state's previous state.
        /// </summary>
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
