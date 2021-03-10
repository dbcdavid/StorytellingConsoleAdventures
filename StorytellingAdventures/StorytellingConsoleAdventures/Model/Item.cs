using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// Item is the class that describes objects that the player may find and eventually use
    /// It contains a name, and an effect, which is a function name called when the item is used
    /// </summary>
    class Item
    {
        private string name = "";
        private string effect = "";

        public Item(string name, string effect)
        {
            this.name = name;
            this.effect = effect;
        }

        /// <summary>
        /// Calls the function correponding to the effect of the item.
        /// This special call uses c# reflection. This was chosen to ease future implementations. If the designer wishes to create new items with different effects,
        /// the programmer will only need to create a function with those effects' name.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the effect function was executed.
        /// A string that contains the action description
        /// </returns>
        public bool Use(Object[] parameters, ref string message)
        {
            try
            {
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(effect);
                message = (string)theMethod.Invoke(this, parameters);

                if (message.Length > 0)
                {
                    return true;
                }
                else
                {
                    message = name + " " + Messages.NOUSEMESSAGE;
                    return false;
                }
            }
            catch (Exception)
            {
                message = name + " " + Messages.NOUSEMESSAGE;
                return false;
            }
        }

        /// <summary>
        /// This is the sword function. It kill the target given as parameter by reducing its life points to 0
        /// </summary>
        /// <returns>
        /// A string containing the message of the action
        /// </returns>
        public string Attack(Object actorObject, Object targetEntity)
        {
            Entity actor = (Entity)actorObject;
            Entity target = (Entity)targetEntity;
            
            if (actor.CurrentLocation == target.CurrentLocation)
            {
                target.Die();
                string message = actor.Name + " " + Messages.ATTACKVERB + " the " + target.Name;
                return message;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// This is the "Key" function. It unlocks the door which is the obstacle to open the place where the sword is.
        /// </summary>
        /// <returns>
        /// A string containing the message of the action.
        /// </returns>
        public string Unlock(Object actorObject, Object targetEntity)
        {
            Entity actor = (Entity)actorObject;
            Location location = actor.CurrentLocation;
            List<Obstacle> obstacles = location.GetObstacles();

            foreach (Obstacle obstacle in obstacles)
            {
                if (obstacle.Solve(this))
                {
                    obstacle.Condition = Messages.UNLOCKVERB;
                    string message = actor.Name + " " + Messages.UNLOCKVERB + " the " + obstacle.Name;
                    return message;
                }
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Handler of the item's name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Handler of the item's effect.
        /// </summary>
        public string Effect
        {
            get
            {
                return effect;
            }
        }
    }
}
