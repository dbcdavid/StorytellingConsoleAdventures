using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    class Entity
    {
        protected Location location = null;
        protected string name = string.Empty;

        public Location CurrentLocation
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
