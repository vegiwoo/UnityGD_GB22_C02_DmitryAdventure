using System;
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

        private static readonly Dictionary<string, GameValueItem> Inventory = new (100);
        /// <summary>
        /// Event to be called after inventory has been updated.
        /// </summary>
        public UnityEvent<IEnumerable<GameValueItem>> inventoryUpdateEvent;
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            // HACK: Hardcode 
            var mine = new GameValue(GameData.MineKey, "Enemies on this mine fly up like crazy frogs",
                GameValueType.Item, RarityLevel.Rare, 50, 1.50f, hpBoostRate: -100.0f, valueKey: GameData.MineKey);
            var mines = new GameValue[] { mine, mine, mine};
            PushInInventory(mines);
            inventoryUpdateEvent.Invoke(GetInventoryContent());
        }
        #endregion

        #region Functionality

        /// <summary>
        /// Adds a collection of items to inventory.
        /// </summary>
        /// <param name="values">Collection of items</param>
        /// <returns>Contents of inventory.</returns>
        public void PushInInventory(IEnumerable<GameValue> values)
        {
            var pushingValues = 
                from value in values
                group value by value.ValueKey
                into g
                select new GameValueItem(g.First(), g.Count());

            foreach (var valueItem in pushingValues.ToList())
            {
                var key = valueItem.Value.ValueKey.ToLower();
                if (Inventory.ContainsKey(key))
                {
                    Inventory[key].Count += valueItem.Count;
                }
                else
                {
                    Inventory[key] = valueItem;
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
            var key = nameKey.ToLower();
            if (!Inventory.ContainsKey(key)) return null;

            var inInventory = Inventory[key];
            if (inInventory.Count <= 0) return null;
            
            inInventory.Count--;
            Inventory[key] = inInventory;
            
            inventoryUpdateEvent.Invoke(GetInventoryContent());

            return inInventory.Value;
        }

        /// <summary>
        /// Returns the contents of inventory.
        /// </summary>
        /// <returns>Collection of tuples (name of game value, quantity in inventory).</returns>
        private static IEnumerable<GameValueItem> GetInventoryContent()
        {
            return Inventory.Select(el =>  el.Value);
        }
        #endregion
    }
}