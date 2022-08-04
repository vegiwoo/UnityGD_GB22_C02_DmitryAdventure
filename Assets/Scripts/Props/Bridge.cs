using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents bridge.
    /// </summary>
    public class Bridge : LockedMechanism
    {
        #region Сonstants, variables & properties

        // ...

        #endregion

        #region Monobehavior methods

        // ...

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers

        protected override void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform, bool isObjectEnters)
        {
            Debug.Log("Handle!");
            base.OnDiscoveryTriggerHandler(discoveryType, discoveryTransform, isObjectEnters);
            Debug.Log($"{isObjectEnters}");
        }

        #endregion

        #region Other methods

        // ...

        #endregion

        #endregion
    }
}