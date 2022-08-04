using System;
using System.Linq;
using JetBrains.Annotations;
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
            HandlingTriggerEvent(other.gameObject, true);
        }

        private void OnTriggerExit(Collider other)
        {
            HandlingTriggerEvent(other.gameObject, false);
        }

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
        /// Handles an event when an object enters and exits a trigger.
        /// </summary>
        /// <param name="obj">Game object that hit trigger.</param>
        /// <param name="isObjectEnters">An object enters or exits a trigger.</param>
        private void HandlingTriggerEvent(in GameObject obj, in bool isObjectEnters)
        {
            if (DiscoverableTypes is null || DiscoverableTypes.Length == 0) return;
   
            var type = GetDiscoveryTypeFrom(obj);
            if (type == null) return;
            
            var discoveryTransform = obj.transform;
            OnDiscoveryTriggerNotify((DiscoveryType)type, discoveryTransform, isObjectEnters);
        }
        
        /// <summary>
        /// Checks if game object in trigger matches one of discovery types.
        /// </summary>
        /// <param name="obj">Game object that hit trigger.</param>
        /// <returns>Match one of discovery types, or null.</returns>
        [CanBeNull]
        private DiscoveryType? GetDiscoveryTypeFrom(in GameObject obj)
        {
            if (obj.CompareTag(GameData.PlayerTag))
            {
                return DiscoveryType.Player;
            } 
            if (obj.CompareTag(GameData.EnemyTag))
            {
                return DiscoveryType.Enemy;
            } 
            if(obj.TryGetComponent<MoveableObject>(out var movable))
            {
                return DiscoveryType.Movable;
            }
            return null;
        }
        
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

    }
}