using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using StorytellingConsoleAdventures.View;

namespace StorytellingConsoleAdventures.Model
{
    /*
     * Item is the class that describes objects that the player may find and eventually use
     * It contains a name, its current location, an effect, which is a function name called when the item is used, 
     * and an array of the parameters used in the effect call
     */
    class Item
    {
        private string name = "";
        private string effect = "";

        public Item(string name, string effect)
        {
            this.name = name;
            this.effect = effect;
        }

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
            catch (Exception ex)
            {
                message = name + " " + Messages.NOUSEMESSAGE;
                return false;
            }
        }

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

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Effect
        {
            get
            {
                return effect;
            }
        }
    }
}
