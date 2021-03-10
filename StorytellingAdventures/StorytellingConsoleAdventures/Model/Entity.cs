using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    class Entity
    {
        protected Location location = null;
        protected string name = string.Empty;
        protected int lifePoints = 0;
        protected List<Item> items = null;

        public bool AddItem(Item newItem)
        {
            foreach (Item item in items)
            {
                if (item.Name.Equals(newItem.Name))
                {
                    return false;
                }
            }

            items.Add(newItem);
            return true;
        }

        public bool HasItem(string itemName)
        {
            foreach (Item item in items)
            {
                if (item.Name.ToLower().Equals(itemName))
                {
                    return true;
                }
            }

            return false;
        }

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

        public void LoseLife()
        {
            lifePoints--;
        }

        public void Die()
        {
            lifePoints = 0;
        }

        public bool IsAlive()
        {
            if (lifePoints > 0)
            {
                return true;
            }

            return false;
        }

        public int LifePoints
        {
            get
            {
                return lifePoints;
            }
        }
    }
}
