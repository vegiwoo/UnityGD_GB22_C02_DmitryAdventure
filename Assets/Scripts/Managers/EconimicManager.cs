using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DmitryAdventure
{
    /// <summary>
    /// Game economy manager
    /// </summary>
    public class EconimicManager : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private CharacterInventory characterInventory;

        public UnityEvent<List<(GameValue value, int count)>> inventoryContentEvent;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            CallInventoryContent();
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
        /// Requests contents of the hero's inventory
        /// </summary>
        private void CallInventoryContent()
        {
            var inventoryContent = characterInventory.GetInventoryContent();
            inventoryContentEvent.Invoke(inventoryContent.ToList());
        }

        /// <summary>
        /// Transfers game values to character's inventory.
        /// </summary>
        public void PutInCharacterInventory(List<GameValue> values)
        {
            var inventoryContent = characterInventory.PutInInventory(values);
            inventoryContentEvent.Invoke(inventoryContent.ToList());
        }

        #endregion

        #endregion
    }
}