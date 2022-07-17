#nullable enable
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represent field of view.
    /// </summary>
    public sealed class DiscoveryTrigger : MonoBehaviour
    {
        #region Сonstants, variables & properties
        
        public DiscoveryType[] discoverableTypes;
        
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
        // ...
        #endregion

        #endregion

 
    }
}