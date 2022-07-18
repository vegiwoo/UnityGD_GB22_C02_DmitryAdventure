using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DmitryAdventure
{
    public enum LootType
    {
        Health, Key, Mine, None
    }

    /// <summary>
    /// Represents a loot box.
    /// </summary>
    public class LootBox : MonoBehaviour
    {
        #region Сonstants, variables & properties

        private DiscoveryTrigger _discoveryTrigger;
        
        /// <summary>
        /// Possible box capacity.
        /// </summary>
        /// <remarks>Used for random capacitance capacity.</remarks>>
        private int _possibleCapacity = 2;

        /// <summary>
        /// Box capacity.
        /// </summary>
        /// <remarks>Randomly generated.</remarks>>
        private int _capacity;
        
        /// <summary>
        /// Collection of game objects in a box.
        /// </summary>
        private List<LootType> _objectsInBox;

        /// <summary>
        /// Is box empty (hero took loot from box).
        /// </summary>
        public bool IsBoxGutted { get; set; }
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            _discoveryTrigger.DiscoveryTriggerNotify += OnFindingTarget;
            
            _capacity = Random.Range(0, _possibleCapacity);
            _objectsInBox = new List<LootType>(_capacity);

            for (var i = 0; i < _objectsInBox.Capacity; i++)
            {
                _objectsInBox[i] = (LootType)Random.Range(0, 3);
            }
            
            IsBoxGutted = false;
        }

        /// <summary>
        /// Moves enemy when attacking.
        /// </summary>
        private void OnFindingTarget(DiscoveryType type, Transform targetTransform, bool _)
        {
            if(IsBoxGutted) return;
    
            switch (type)
            {
                case DiscoveryType.Player:
                    // Проиграть звук открытия 
                    // Отдать в инвентарь игрока все что было 
                    // Инвентарь пишет что поступило, UI обновляется
                    IsBoxGutted = true;
                    break;
                default:
                    break;
            }
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

        // ...

        #endregion

        #endregion
    }
}