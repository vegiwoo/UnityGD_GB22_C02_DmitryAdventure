using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DmitryAdventure
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private DoorTrigger doorTrigger;
        [SerializeField] private Transform leftDoorHinges;
        [SerializeField] private Transform rightDoorHinges;

        private bool isOpen;

        private void Start()
        {
            doorTrigger.Notify += OpenСloseDoor;
        }

        private void OpenСloseDoor(bool heroInTrigger)
        {
            switch (heroInTrigger)
            {
                case true:
                    if (!isOpen)
                    {
                        // Открыть двери 
                        if(rightDoorHinges != null)
                            rightDoorHinges.RotateAround(rightDoorHinges.position, rightDoorHinges.up, 80);
                        if(leftDoorHinges != null)
                            leftDoorHinges.RotateAround(leftDoorHinges.position, leftDoorHinges.up, -80);
                        
                        
                        isOpen = true;
                    }
                    break;
                case false:
                    if (isOpen)
                    {
                        // Закрыть двери
                        if(rightDoorHinges != null)
                            rightDoorHinges.RotateAround(rightDoorHinges.position, rightDoorHinges.up, -80);
                        if(leftDoorHinges != null)
                            leftDoorHinges.RotateAround(leftDoorHinges.position, leftDoorHinges.up, 80);
                        
                        isOpen = false;
                    }
                    break;
            }
        }

        private void OnDestroy()
        {
            doorTrigger.Notify -= OpenСloseDoor;
        }
    }
}
