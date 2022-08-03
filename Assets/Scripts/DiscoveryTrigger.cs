using System;
using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represent field of view.
    /// </summary>
    public sealed class DiscoveryTrigger : MonoBehaviour
    {
        #region Сonstants, variables & properties

        public DiscoveryTrigger(DiscoveryType[] discoverableTypes)
        {
            DiscoverableTypes = discoverableTypes;
        }

        [field:SerializeField, Tooltip("Discoverable types for trigger")]
        public DiscoveryType[] DiscoverableTypes { get; set;}

        private Vector3 originalSize;
        
        
        public delegate void DiscoveryTriggerHandler(DiscoveryType discoveryType,  Transform discoveryTransform, bool entry);  
        public event DiscoveryTriggerHandler? DiscoveryTriggerNotify;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            originalSize = transform.localScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (DiscoverableTypes is null || DiscoverableTypes.Length == 0) return;

            var discoveryTransform = other.gameObject.transform;
            const bool isCharacterEnters = true;
            
            if (DiscoverableTypes.Contains(DiscoveryType.Player) && other.gameObject.CompareTag(GameData.PlayerTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Player, discoveryTransform, isCharacterEnters);
            }

            if (DiscoverableTypes.Contains(DiscoveryType.Enemy) && other.gameObject.CompareTag(GameData.EnemyTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Enemy, discoveryTransform, isCharacterEnters);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (DiscoverableTypes == null || DiscoverableTypes.Length == 0) return;

            var discoveryTransform = other.gameObject.transform;
            const bool isCharacterEnters = false;
            
            if (DiscoverableTypes.Contains(DiscoveryType.Player) && other.gameObject.CompareTag(GameData.PlayerTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Player, discoveryTransform, isCharacterEnters);
            }
            
            if (DiscoverableTypes.Contains(DiscoveryType.Enemy) && other.gameObject.CompareTag(GameData.EnemyTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Enemy, discoveryTransform, isCharacterEnters);
            }
        }

        #endregion

        #region Functionality

        #region Coroutines
        // ...
        #endregion

        #region Event handlers

        /// <summary>
        /// Raises an event if object enters or exits the trigger.
        /// </summary>
        /// <param name="discoveryType">Type of detected object.</param>
        /// <param name="discoveryTransform">Transform of object..</param>
        /// <param name="isItemEnters">Object enters or exits trigger.</param>
        private void OnDiscoveryTriggerNotify(DiscoveryType discoveryType, Transform discoveryTransform, bool isItemEnters)
        {
            DiscoveryTriggerNotify?.Invoke(discoveryType, discoveryTransform, isItemEnters);
        }

        #endregion

        #region Other methods

        /// <summary>
        /// Changes size of the discovery trigger.
        /// </summary>
        /// <param name="increase">Trigger increment/decrement flag.</param>
        /// <param name="factor">Magnification multiplier.</param>
        public void ChangeSizeOfDiscoveryTrigger(bool increase, float? factor = null)
        {
            var currentScale = transform.localScale;
            if (increase && factor != null)
            {
                transform.localScale = (Vector3)(currentScale * factor);
            }
            else
            {
                transform.localScale = originalSize;
            }
        }

        #endregion

        #endregion
    }
}