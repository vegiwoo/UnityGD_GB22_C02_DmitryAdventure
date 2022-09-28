using System.Collections.Generic;
using System.Linq;
using DmitryAdventure.Args;
using Events;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents player's inventory.
    /// </summary>
    public class CharacterInventory : MonoBehaviour
    {
        #region Links
        [field: SerializeField] private InventoryEvent InventoryEvent { get; set; }
        #endregion
        
        #region Fields
        
        private static readonly Dictionary<string, GameValueItem> Inventory = new (100);
        
        #endregion
        
        #region Monobehavior methods

        private void Start()
        {
            // HACK: Hardcode 
            var mine = new GameValue(GameData.MineKey, "Enemies on this mine fly up like crazy frogs",
                GameValueType.Item, RarityLevel.Rare, 50, 1.50f, hpBoostRate: -100.0f, valueKey: GameData.MineKey);
            var mines = new [] { mine, mine, mine};
            PushInInventory(mines);
            
            Notify();
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
                select new GameValueItem(g.First().ValueKey,g.First(), g.Count());

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

            Notify();
        }

        /// <summary>
        /// Returns one instance of found item from inventory.
        /// </summary>
        /// <param name="nameKey">Item name as key.</param>
        /// <param name="gameValue"></param>
        public void PopFromInventory(string nameKey, out GameValue? gameValue)
        {
            var key = nameKey.ToLower();
            if (!Inventory.ContainsKey(key))
            {
                gameValue = null;
            };

            var inInventory = Inventory[key];
            if (inInventory.Count <= 0)
            {
                gameValue = null;
            }
            
            inInventory.Count--;
            Inventory[key] = inInventory;
            
            gameValue = inInventory.Value;
            
            Notify();
        }

        /// <summary>
        /// Returns the contents of inventory.
        /// </summary>
        /// <returns>Collection of tuples (name of game value, quantity in inventory).</returns>
        private static IEnumerable<GameValueItem> GetInventoryContent()
        {
            return Inventory.Select(el =>  el.Value);
        }

        private void Notify()
        {
            var args = new InventoryArgs(GetInventoryContent().ToList());
            InventoryEvent.Notify(args);
        }
        
        #endregion
    }
}