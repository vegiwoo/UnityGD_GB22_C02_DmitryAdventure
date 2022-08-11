using System.Collections.Generic;
using System.Linq;
using DmitryAdventure.Args;
using UnityEngine;
using UnityEngine.Events;
using DmitryAdventure.Props;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Managers
{
    /// <summary>
    /// Game economy manager.
    /// </summary>
    public class EconomicManager : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] 
        private CharacterInventory characterInventory;

        [SerializeField] 
        private LootBox lootBoxPrefab;
        
        [SerializeField, Tooltip("Loot box placement points")] 
        private Transform[] lootBoxPoints;

        public UnityEvent<IEnumerable<(string,int)>> inventoryContentEvent;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            ArrangeLootBoxesByPoints(3, new Dictionary<int, RarityLevel>()
            {
                {
                    0, RarityLevel.Ordinary
                },
                {
                    1, RarityLevel.Rare
                }
            });
        }

        private void OnEnable()
        {
            characterInventory.inventoryUpdateEvent.AddListener(InventoryUpdateEvent);
        }

        private void OnDisable()
        {
            characterInventory.inventoryUpdateEvent.RemoveAllListeners();
        }

        #endregion

        #region Functionality

        /// <summary>
        /// Fills loot boxes with game values.
        /// </summary>
        /// <param name="boxMaxCapacity">Max loot box capacity.</param>
        /// <param name="permissibleRarity">Rarity levels for valuables in loot box.</param>
        private void ArrangeLootBoxesByPoints(int boxMaxCapacity, IDictionary<int, RarityLevel> permissibleRarity)
        {
            if (lootBoxPoints.Length == 0)
            {
                Debug.LogError("Create placement points for loot boxes");
                return;
            }
            
            foreach (var t in lootBoxPoints)
            {
                var args = new LootBoxArgs(boxMaxCapacity, permissibleRarity);
                var newLootBox = Instantiate(lootBoxPrefab, t.position, t.rotation);
                if (args.GameValues.Length > 0)
                {
                    newLootBox.Init(args);
                }
                newLootBox.HeroFindValuesNotify += PushInCharacterInventory;
            }
        }
        #endregion

        #region Other methods
        /// <summary>
        /// Transfers game values to character's inventory.
        /// </summary>
        private void PushInCharacterInventory(IEnumerable<GameValue> values)
        {
            characterInventory.PushInInventory(values);
        }

        /// <summary>
        /// Handles inventory update event and fires its own event.
        /// </summary>
        /// <param name="values">Game value in inventory.</param>
        public void InventoryUpdateEvent(IEnumerable<GameValueItem> values)
        {
            var items = values.Select(v => (v.Value.ValueKey.ToLower(), v.Count));
            inventoryContentEvent.Invoke(items);
        }
        #endregion
    }
}