#nullable enable
using System;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents a door trigger
    /// </summary>
    public class DoorTrigger : MonoBehaviour
    {
        #region Сonstants, variables & properties

        public delegate void DoorTriggerHandler(Vector3 characterPosition);  
        public event DoorTriggerHandler? CharacterDiscoveryNotify;

        #endregion

        #region Monobehavior methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Character>() == null) return;
            OnNotify(other.transform.forward);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Character>() == null) return;
            OnNotify(Vector3.zero);
        }

        #endregion

        #region Functionality
        #region Event handlers

        private void OnNotify(Vector3 charForwardDirection)
        {
            CharacterDiscoveryNotify?.Invoke(charForwardDirection);
        }

        #endregion
        #endregion
    }
}