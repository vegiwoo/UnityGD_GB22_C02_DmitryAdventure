using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DmitryAdventure
{
    public sealed class DoorTrigger : MonoBehaviour
    {
        public delegate void DoorTriggerHandler(bool heroInTrigger);  
        public event DoorTriggerHandler? Notify;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
            {
                OnNotify(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
            {
                OnNotify(false);
            }
        }

        private void OnNotify(bool herointrigger)
        {
            Notify?.Invoke(herointrigger);
        }
    }
}


