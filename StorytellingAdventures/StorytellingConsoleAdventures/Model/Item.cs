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
        private string[] effectParameters;
        private Location location = null;

        public string Use()
        {
            bool result = false;
            try
            {
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(effect);
                //string returnMessage = theMethod.Invoke(this, effectParameters);
                //return returnMessage;
            }catch (Exception ex)
            {
                return Messages.NOUSEMESSAGE;
            }

            return String.Empty;
        }
    }
}
