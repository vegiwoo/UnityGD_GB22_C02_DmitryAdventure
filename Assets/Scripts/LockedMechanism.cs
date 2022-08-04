using System.Linq;
using DmitryAdventure.Props;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents a closed (closing) mechanism mechanism.
    /// </summary>
    public abstract class LockedMechanism : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [Header("Base variables and references")]
        [SerializeField] 
        protected LockedMechanismType lockedMechanismType;
        
        [SerializeField, Tooltip("An array of object types to discover.")]
        protected DiscoveryType[] discoveryTypes;
        
        [SerializeField, Tooltip("Discovery trigger listening for state change")]
        protected DiscoveryTrigger discoveryTrigger;

        #endregion

        #region Monobehavior methods

        protected virtual void Start()
        {
            discoveryTrigger.DiscoverableTypes = discoveryTypes;
        }

        protected void OnEnable()
        {
            discoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerHandler;
        }

        protected void OnDisable()
        {
            discoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerHandler;
        }

        #endregion

        #region Functionality

        /// <summary>
        /// Trigger signal event handler.
        /// </summary>
        /// <param name="discoveryType">Type of object found on trigger.</param>
        /// <param name="discoveryTransform">Transform of object found on trigger.</param>
        /// <param name="isObjectEnters">Object entered trigger (true) or exited it (false).</param>
        protected virtual void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform,
            bool isObjectEnters)
        {
            if (!discoveryTypes.Contains(discoveryType)) return;
        }
        
        #endregion
    }
}