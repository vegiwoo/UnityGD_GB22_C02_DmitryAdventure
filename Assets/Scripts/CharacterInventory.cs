using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents player's inventory.
    /// </summary>
    public class CharacterInventory : MonoBehaviour
    {
        #region Сonstants, variables & properties

        public float CurrentInventoryCapacity { get; private set; }

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            CurrentInventoryCapacity = GameData.Instance.MaximumInventoryCapacity;
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
        /// Checks if inventory is full.
        /// </summary>
        /// <returns>Check flag.</returns>
        public bool IsInventoryFull()
        {
            return CurrentInventoryCapacity >= GameData.Instance.MaximumInventoryCapacity;
        }

        /// <summary>
        /// Checks added weight capacity.
        /// </summary>
        /// <returns>Check flag.</returns>
        public bool IsItemsPlacedInInventory(float addedWeight)
        {
            return CurrentInventoryCapacity + addedWeight <= GameData.Instance.MaximumInventoryCapacity;
        }

        public void PutInInventory(List<GameValue> values)
        {
            // Добавить предметы в интвентарь
            // Сгенерировать событие и отправить строку
        }

        public void PickUpFromInventory()
        {
            // забрать предметы из интвентаря
            // Сгенерировать событие и отправить строку
            // Если это конкретный
        }

        #endregion

        #endregion
    }
}