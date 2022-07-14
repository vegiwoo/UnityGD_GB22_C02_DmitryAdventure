using UnityEngine;

namespace DmitryAdventure
{
    
    public enum DoorPlacement
    {
        Left, Right
    }
    
    public class Door : MonoBehaviour
    {
        [SerializeField, Tooltip("Угол открывания дверей")]
        private float doorOpeningAngle;

        [SerializeField] private DoorTrigger doorTrigger;

        [SerializeField] private DoorPlacement doorPlacement;
        private bool _isOpen = false;

        private Coroutine _doorOpeningCoroutine;

        private void Start()
        {
            doorOpeningAngle = 75;
            doorTrigger.Notify += OpenСloseDoor;
        }

        private void OpenСloseDoor(bool heroInTrigger)
        {
            switch (heroInTrigger)
            {
                // Открыть двери 
                case true:
                    if (!_isOpen)
                    {
                        switch (doorPlacement)
                        {
                            case DoorPlacement.Left:
                                transform.Rotate(0,-doorOpeningAngle,0, Space.Self);
                                break;
                            case DoorPlacement.Right:
                                transform.Rotate(0,doorOpeningAngle,0, Space.Self);
                                break; ;
                        }
                        _isOpen = true;
                    }
                    break;
                // Закрыть двери
                case false:
                    if (_isOpen)
                    {
                        switch (doorPlacement)
                        {
                            case DoorPlacement.Left:
                                transform.Rotate(0,doorOpeningAngle,0, Space.Self);
                                break;
                            case DoorPlacement.Right:
                                transform.Rotate(0,-doorOpeningAngle,0, Space.Self);
                                break;
                        }
                        _isOpen = false;
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

/*
 *  TODO: 1. Открывание дверей по joint
 *  TODO: 2. Открывание дверей в направлении движения персонажа
 */