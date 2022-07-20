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

        [SerializeField, Tooltip("Discoverable types for trigger")]
        private DiscoveryType[] discoverableTypes;
        
        /// <summary>
        /// Discoverable types for trigger.
        /// </summary>
        public DiscoveryType[] DiscoverableTypes
        {
            get => discoverableTypes;
            set => discoverableTypes = value;
        }

        public delegate void DiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoverableTransform, bool entry);  
        public event DiscoveryTriggerHandler? DiscoveryTriggerNotify;

        private const string PlayerTag = "Player";
        private const string EnemyTag = "Enemy";
        
        #endregion

        #region Monobehavior methods

        private void OnTriggerEnter(Collider other)
        {
            if (DiscoverableTypes == null || DiscoverableTypes.Length == 0) return;

            if (DiscoverableTypes.Contains(DiscoveryType.Player) && other.gameObject.CompareTag(PlayerTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Player, other.gameObject.transform, true);
            }

            if (DiscoverableTypes.Contains(DiscoveryType.Enemy) && other.gameObject.CompareTag(EnemyTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Enemy, other.gameObject.transform, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (DiscoverableTypes == null || DiscoverableTypes.Length == 0) return;

            if (DiscoverableTypes.Contains(DiscoveryType.Player) && other.gameObject.CompareTag(PlayerTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Player, other.gameObject.transform, false);
            }
            
            if (DiscoverableTypes.Contains(DiscoveryType.Enemy) && other.gameObject.CompareTag(EnemyTag))
            {
                OnDiscoveryTriggerNotify(DiscoveryType.Enemy, other.gameObject.transform, false);
            }
        }

        #endregion

        #region Functionality

        #region Coroutines
        // ...
        #endregion

        #region Event handlers

        private void OnDiscoveryTriggerNotify(DiscoveryType discoveryType, Transform discoverableTransform, bool entry)
        {
            DiscoveryTriggerNotify?.Invoke(discoveryType, discoverableTransform, entry);
        }

        #endregion

        #region Other methods
        // ...
        #endregion

        #endregion
    }
}