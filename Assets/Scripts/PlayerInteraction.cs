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

        [SerializeField, Range(15f,25f), Tooltip("Force of impulse to push an object")] 
        private float impulsePower;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            DiscoveryTrigger.Init(DiscoveryTypes);
            impulsePower = 20f;
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

        public void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform,
            bool isObjectEnters)
        {
            if (DiscoveryTypes.Length == 0 || !DiscoveryTypes.Contains(discoveryType))
            {
                return;
            }

            if (discoveryType == DiscoveryType.Movable && isObjectEnters && discoveryTransform.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(discoveryTransform.forward * impulsePower, ForceMode.Impulse);
            }
        }

        #endregion
    }
}