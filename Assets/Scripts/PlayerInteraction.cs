using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents functionality for player interaction with objects.
    /// </summary>
    public sealed class PlayerInteraction : MonoBehaviour, IDiscovering
    {
        #region Сonstants, variables & properties

        [field: SerializeField, Tooltip("Collection of types tracked by DiscoveryTrigger")]
        public DiscoveryType[] DiscoveryTypes { get; set; }

        [field:SerializeField, Tooltip("Tracking/discovery trigger")]
        public DiscoveryTrigger DiscoveryTrigger { get; set; }

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            DiscoveryTrigger.Init(DiscoveryTypes);
        }

        private void OnEnable()
        {
            DiscoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerHandler;
        }

        private void OnDisable()
        {
            DiscoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerHandler;
        }

        #endregion

        #region Functionality

        private void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform,
            bool isObjectEnters)
        {
            if (DiscoveryTypes.Length == 0 || !DiscoveryTypes.Contains(discoveryType))
            {
                return;
            }

            if (discoveryType == DiscoveryType.Movable && isObjectEnters && discoveryTransform.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(discoveryTransform.forward * 20.0f, ForceMode.Impulse);
            }
        }

        #endregion
    }
}