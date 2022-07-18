using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Game economy manager
    /// </summary>
    public class EconimicManager : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private CharacterInventory characterInventory;
        
        #endregion

        #region Monobehavior methods

        // ...

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
        /// Transfers game values to character's inventory.
        /// </summary>
        public void PutInCharacterInventory(List<GameValue> values)
        {
            var addedWeight = values.Select(v => v.Weight).Sum();
            if (characterInventory.IsItemsPlacedInInventory(addedWeight))
            {
                characterInventory.PutInInventory(values);
                
                // Отчет в UI - ценности по количеству
                // - аптечек, ключей, мин
                
                
                
                // Отчет по поступлении в UI
            }
            else
            {
                Debug.Log("Found items don`t fit in inventory :(");
            }
        }

        #endregion

        #endregion
    }
}