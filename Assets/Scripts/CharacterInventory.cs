using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents player's inventory.
    /// </summary>
    public class CharacterInventory : MonoBehaviour
    {
        #region Сonstants, variables & properties

        private static readonly Dictionary<string, (GameValue value, int count)> Inventory = new (100);
        /// <summary>
        /// Event to be called after inventory has been updated.
        /// </summary>
        public UnityEvent<IEnumerable<(GameValue value, int count)>> inventoryUpdateEvent;
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            // HACK: Hardcode!
            var mine = new GameValue(GameData.MineKey, "Enemies on this mine fly up like crazy frogs",
                GameValueType.Item, RarityLevel.Rare, 50, 1.50f, hpBoostRate: -100f, valueKey: GameData.MineKey);
            PushInInventory(new []{mine, mine, mine});
            
            //updateEvent.Invoke(GetInventoryContent());
        }
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
        public void PushInInventory(IEnumerable<GameValue> values)
        {
            var ordered = GetValuesInGroupsById(values).ToList();
            
            foreach (var group in ordered)
            {
                var addedValue = group.First();
                var addedCount = group.Count();
                
                if (Inventory.ContainsKey(group.Key))
                {
                    var itemInInventory = Inventory[group.Key];
                    itemInInventory.count += addedCount;
                }
                else
                {
                    Inventory[group.Key] = (addedValue, addedCount);
                }
            }
            
            inventoryUpdateEvent.Invoke(GetInventoryContent());
        }

        /// <summary>
        /// Returns one instance of found item from inventory.
        /// </summary>
        /// <param name="nameKey">Item name as key.</param>
        /// <returns>One instance of the found item, or null.</returns>
        [CanBeNull]
        public GameValue PopFromInventory(string nameKey)
        {
            if (!Inventory.ContainsKey(nameKey)) return null;

            var inInventory = Inventory[nameKey];
            if (inInventory.count <= 0) return null;
            
            inInventory.count -= 1;
            Inventory[nameKey] = inInventory;
            
            inventoryUpdateEvent.Invoke(GetInventoryContent());

            return inInventory.value;
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
        private static IEnumerable<(GameValue value, int count)> GetInventoryContent()
        {
            return Inventory.Values.Select(val => (val.value, val.count));
        }
        #endregion

        #endregion
    }
}