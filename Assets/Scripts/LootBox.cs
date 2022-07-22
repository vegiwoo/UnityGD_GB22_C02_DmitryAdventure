using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents a loot box.
    /// </summary>
    public class LootBox : MonoBehaviour
    {
        #region Сonstants, variables & properties

        private DiscoveryTrigger _discoveryTrigger;

        [SerializeField]
        private AudioClip openingSound;
        
        [SerializeField, Tooltip("Possible box capacity"), Range(0,2)]
        private int possibleCapacity = 2;

        [SerializeField, Tooltip("Rarity level of items in the box")]
        private RarityLevel[] rarityLevels;
        
        /// <summary>
        /// Box capacity.
        /// </summary>
        /// <remarks>Randomly generated.</remarks>>
        private int _capacity;
        
        /// <summary>
        /// Collection of game objects in a box.
        /// </summary>
        private List<GameValue> _objectsInBox;

        /// <summary>
        /// Is box empty (hero took loot from box).
        /// </summary>
        private bool IsBoxEmpty { get; set; }

        /// <summary>
        /// An item detection event by main character.
        /// </summary>
        public UnityEvent<List<GameValue>> heroFoundValuesHandler;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            
            _capacity = Random.Range(0, possibleCapacity);
            _objectsInBox = new List<GameValue>(_capacity);
            
            FillingBox();
            
            _discoveryTrigger.DiscoveryTriggerNotify += OnFindingTarget;
        }

        private void OnDestroy()
        {
            _discoveryTrigger.DiscoveryTriggerNotify -= OnFindingTarget;
        }

        #endregion

        #region Functionality
        #region Coroutines
        // ...
        #endregion

        #region Event handlers
        /// <summary>
        /// Event handler from discovery trigger.
        /// </summary>
        private void OnFindingTarget(DiscoveryType type, Transform targetTransform, bool _)
        {
            if(IsBoxEmpty) return;
    
            switch (type)
            {
                case DiscoveryType.Player:
                    AudioSource.PlayClipAtPoint(openingSound, transform.position);
                    heroFoundValuesHandler.Invoke(_objectsInBox);
                    _objectsInBox = null;
                    IsBoxEmpty = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Other methods

        /// <summary>
        /// Filling box with random game values.
        /// </summary>
        private void FillingBox()
        {
            foreach (var rl in rarityLevels)
            {
                var sampling = CollectionExtensions
                    .RandomValues(GameData.Instance.GameValues)
                    .Where(gv => gv.Rarity == rl);
                
                _objectsInBox.Add(sampling.Take(1).First());
            }
            
            IsBoxEmpty = false;
        }

        #endregion
        #endregion
    }
}