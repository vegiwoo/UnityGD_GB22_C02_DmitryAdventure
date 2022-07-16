#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represent field of view.
    /// </summary>
    public sealed class DiscoveryTrigger : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField, Tooltip("Array of discoverable types for trigger")] 
        private DiscoveryType[] discoverableTypes;
        
        public delegate void DiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoverableTransform);  
        public event DiscoveryTriggerHandler? DiscoveryTriggerNotify;

        private const string PlayerTag = "Player";



        #endregion

        #region Monobehavior methods

        private void OnTriggerEnter(Collider other)
        {
            if (discoverableTypes.Length <= 0) return;
            
            if (other.gameObject.CompareTag(PlayerTag) && other.gameObject.CompareTag(PlayerTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Player, other.gameObject.transform);
            }
            
            // if (discoverableTypes.Contains(DiscoveryType.Enemy) &
            //     (other.gameObject.GetComponents<Enemy>() != null || other.gameObject.GetComponentsInChildren<Enemy>() != null))
            // {
            //     //OnDiscoveryTriggerNotify(DiscoveryType.Enemy, other.gameObject.transform);
            // }
        }

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers

        private void OnDiscoveryTriggerNotify(DiscoveryType discoveryType, Transform discoverableTransform)
        {
            DiscoveryTriggerNotify?.Invoke(discoveryType, discoverableTransform);
        }

        #endregion

        #region Other methods

        /// <summary>
        /// Sets discovered types to trigger.
        /// </summary>
        /// <param name="types">Discovered types.</param>
        public void SetDiscoveryTypes(DiscoveryType[] types)
        {
            discoverableTypes = types; ;
        }

        #endregion

        #endregion

 
    }
}