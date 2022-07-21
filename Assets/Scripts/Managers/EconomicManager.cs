using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Managers
{
    /// <summary>
    /// Game economy manager.
    /// </summary>
    public class EconomicManager : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private CharacterInventory characterInventory;

        public UnityEvent<List<(GameValue value, int count)>> inventoryContentEvent;

        #endregion

        #region Monobehavior methods
        // ...

        #endregion

        #region Functionality

        #region Coroutines
        // ...
        #endregion

        #region Event handlers

        /// <summary>
        /// Callback when updating the user's inventory.
        /// </summary>
        /// <param name="values">Current collection of items in inventory.</param>
        public void OnCharacterInventoryUpdate(IEnumerable<(GameValue value, int count)> values)
        {
            inventoryContentEvent.Invoke(values.ToList());
        }
        #endregion

        #region Other methods
        
        /// <summary>
        /// Transfers game values to character's inventory.
        /// </summary>
        public void PutInCharacterInventory(List<GameValue> values)
        {
            characterInventory.PushInInventory(values);
        }

        #endregion
        #endregion
    }
}