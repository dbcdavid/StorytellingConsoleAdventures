using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.Model
{
    /// <summary>
    /// Class created to hold some common aspects between the player and the monster.
    /// In future iterations, the code could evolve to make the monster a kind of player, capable of executing his actions
    /// </summary>
    class Entity
    {
        protected Location location = null;
        protected string name = string.Empty;
        protected int lifePoints = 0;
        protected List<Item> items = null;

        /// <summary>
        /// Adds an item to the list of items the entity is holding.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the item was added (it is not possible to hold two items of the same name, which must be handled during the world creation).
        /// </returns>
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

        /// <summary>
        /// Checks if the entity has the item passed as parameter.
        /// </summary>
        /// <returns>
        /// A bool that indicates if the entity has the item.
        /// </returns>
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

        /// <summary>
        /// Makes the entity lose one point of life.
        /// </summary>
        public void LoseLife()
        {
            lifePoints--;
        }

        /// <summary>
        /// Make the entity lose all his life points.
        /// </summary>
        public void Die()
        {
            lifePoints = 0;
        }

        /// <summary>
        /// Checks if the entity is still alive, i. e., if it has at least one life point.
        /// </summary>
        public bool IsAlive()
        {
            if (lifePoints > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handler of the entity's current life points.
        /// </summary>
        public int LifePoints
        {
            get
            {
                return lifePoints;
            }
        }

        /// <summary>
        /// Handler of the entity's current location.
        /// </summary>
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

        /// <summary>
        /// Handler of the entity's current name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Handler of the entity's current list of items.
        /// </summary>
        public List<Item> Items
        {
            get
            {
                return items;
            }
        }
    }
}
