using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Requirements for an object that uses a discovery trigger.
    /// </summary>
    internal interface IDiscovering
    {
        /// <summary>
        /// Collection of types tracked by the DiscoveryTrigger.
        /// </summary>
        public DiscoveryType[] DiscoveryTypes { get; set; }
        
        /// <summary>
        /// Tracking/discovery trigger.
        /// </summary>
        public DiscoveryTrigger DiscoveryTrigger { get; set; }
        
        /// <summary>
        /// Trigger signal event handler.
        /// </summary>
        /// <param name="discoveryType">Type of object found on trigger.</param>
        /// <param name="discoveryTransform">Transform of object found on trigger.</param>
        /// <param name="isObjectEnters">Object entered trigger (true) or exited it (false).</param>
        public void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform,
            bool isObjectEnters);
    }
}