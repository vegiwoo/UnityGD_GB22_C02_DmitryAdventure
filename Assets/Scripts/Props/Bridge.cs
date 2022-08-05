using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents bridge.
    /// </summary>
    public class Bridge : LockedMechanism
    {
        #region Functionality

        protected override void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform, bool isObjectEnters)
        {
            base.OnDiscoveryTriggerHandler(discoveryType, discoveryTransform, isObjectEnters);
            OpenCloseMechanism(discoveryTransform, isObjectEnters);
        }
        
        protected override void OpenCloseMechanism(Transform discoveryTransform, bool isItemEnters)
        {
            base.OpenCloseMechanism(discoveryTransform, isItemEnters);

            var spring = hingeJoints[0].spring;
            
            spring.targetPosition = isItemEnters ? 
                openPosition : 
                closePosition;
            MechanismIsOpen = isItemEnters;
            
            hingeJoints[0].spring = spring;
        }

        #endregion
    }
}