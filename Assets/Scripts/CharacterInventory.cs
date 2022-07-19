using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents player's inventory.
    /// </summary>
    public class CharacterInventory : MonoBehaviour
    {
        #region Сonstants, variables & properties

        private static readonly Dictionary<string, (GameValue value, int count)> _inventory = new (100);
        
        #endregion

        #region Monobehavior methods
        

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers

        // ...

        #endregion

        #region Other methods
        
        /// <summary>
        /// Adds a collection of items to inventory.
        /// </summary>
        /// <param name="values">Collection of items</param>
        /// <returns>Contents of inventory.</returns>
        public IEnumerable<(GameValue value, int count)> PutInInventory(IEnumerable<GameValue> values)
        {
            var ordered = GetValuesInGroupsById(values).ToList();
            
            foreach (var group in ordered)
            {
                var addedValue = group.First();
                var addedCount = group.Count();
                
                if (_inventory.ContainsKey(group.Key))
                {
                    var itemInInventory = _inventory[group.Key];
                    itemInInventory.count += addedCount;
                }
                else
                {
                    _inventory[group.Key] = (addedValue, addedCount);
                }
            }
            return GetInventoryContent();
        }

        /// <summary>
        /// Returns one instance of found item from inventory.
        /// </summary>
        /// <param name="nameKey">Item name as key.</param>
        /// <returns>One instance of the found item, or null.</returns>
        [CanBeNull]
        public GameValue PickUpFromInventory(string nameKey)
        {
            if (!_inventory.ContainsKey(nameKey)) return null;

            var inInventory = _inventory[nameKey];

            if (inInventory.count <= 0) return null;
            inInventory.count -= 1;
            
            CleanInventory(nameKey);
            
            return inInventory.value;
        }

        /// <summary>
        /// Removes game values from inventory if quantity is 0.
        /// </summary>
        /// <param name="nameKey">Name of value as a key to look up in inventory.</param>
        private static void CleanInventory(string nameKey)
        {
            if (_inventory.ContainsKey(nameKey) && _inventory[nameKey].count == 0)
            {
                _inventory.Remove(nameKey);
            }
        }
        
        /// <summary>
        /// Groups values of source collection by game value name.
        /// </summary>
        /// <param name="values">Collection of game values.</param>
        /// <returns>Grouped collection.</returns>
        private static IEnumerable<IGrouping<string, GameValue>> GetValuesInGroupsById(IEnumerable<GameValue> values)
        {
            return 
                from item in values 
                group item by item.Name
                into g 
                orderby g.Count() 
                select g;
        }

        /// <summary>
        /// Returns the contents of inventory.
        /// </summary>
        /// <returns>Collection of tuples (name of game value, quantity in inventory).</returns>
        public IEnumerable<(GameValue value, int count)> GetInventoryContent()
        {
            return _inventory.Values.Select(val => (val.value, val.count));
        }

        #endregion

        #endregion
    }
}