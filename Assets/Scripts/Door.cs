using System;
using System.Linq;
using DmitryAdventure.Characters;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    public enum LockedDoorType
    {
        None, Key, Secret
    }
    
    /// <summary>
    /// Represents the door entity.
    /// </summary>
    [RequireComponent(typeof(AudioIsPlaying))]
    public class Door : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] 
        private LockedDoorType lockedDoorType;
        
        [SerializeField, Tooltip("An array of object types to discover.")]
        public DiscoveryType[] discoveryTypes;

        [SerializeField] 
        private HingeJoint[] hingeJoints;
        private bool _isThereSecondDoor;

        [SerializeField] private float doorSpring;
        [SerializeField] private float doorDamper;
        [SerializeField] private float doorTargetPosition;
        [SerializeField] private float doorBreakForce;
        private float openDoorTargetPosition;
        
        private DiscoveryTrigger _discoveryTrigger;
        private AudioIsPlaying _audioIsPlaying;

        private bool _isOpen;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            _audioIsPlaying = GetComponent<AudioIsPlaying>();
        }

        private void Start()
        {
            doorSpring = 180.0f;
            doorDamper = 90.0f;
            doorTargetPosition = 0.0f;
            openDoorTargetPosition = 90.0f;
            doorBreakForce = 0.0f;

            _isThereSecondDoor = hingeJoints.Length > 1;

            BasicDoorSetting();

            _discoveryTrigger.DiscoverableTypes = discoveryTypes;
            _isOpen = lockedDoorType == LockedDoorType.None; 
        }

        private void OnEnable()
        {
            _discoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerNotify;
        }

        private void OnDisable()
        {
            _discoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerNotify;
        }

        #endregion

        #region Functionality

        private void BasicDoorSetting()
        {
            foreach (var j in hingeJoints)
            {
                j.useSpring = true;
                var jointSpring = j.spring;
                jointSpring.spring = doorSpring;
                jointSpring.damper = doorDamper;
                jointSpring.targetPosition = doorTargetPosition;
                j.spring = jointSpring;
            }
        }
        
        private void OnDiscoveryTriggerNotify(DiscoveryType discoveryType, Transform discoveryTransform, bool isItemEnters)
        {
            if (!discoveryTypes.Contains(discoveryType)) return;

            var character = discoveryTransform.gameObject.GetComponent<Character>();
            
            if (lockedDoorType == LockedDoorType.Key && !_isOpen)
            {
                    var key = character.FindItemInInventory(GameData.KeysKey);
                    if (key != null) 
                    {
                        
                    }
                    else
                    {
                        _audioIsPlaying.PlaySound(SoundType.Negative);
                    }
            }
            else
            {
                OpenCloseDoor(discoveryTransform, isItemEnters);
            }
        }

        #endregion

        #region Other methods

        private void OpenCloseDoor(Transform discoveryTransform, bool isItemEnters)
        {
            if(hingeJoints.Length == 0) return;

            var doorsForward = _discoveryTrigger.transform.forward;
            var characterForward = discoveryTransform.forward;

            var a = doorsForward.normalized;
            var b = characterForward.normalized;
            var collinearity = Math.Abs(Vector3.Dot(a, b) - 1) < 0.00001f;

            var door001JointSpring = hingeJoints[0].spring;
            
            switch (isItemEnters)
            {
                case true:
                   
                    door001JointSpring.targetPosition = collinearity ? -openDoorTargetPosition : openDoorTargetPosition;
                    hingeJoints[0].spring = door001JointSpring;

                    if (_isThereSecondDoor)
                    {
                        var door002JointSpring = hingeJoints[1].spring;
                        door002JointSpring.targetPosition = collinearity ? openDoorTargetPosition : -openDoorTargetPosition;
                        hingeJoints[1].spring = door002JointSpring;
                    }
                    
                    _audioIsPlaying.PlaySound(SoundType.Positive);
                    
                    break;
                case false:
                    foreach (var j in hingeJoints)
                    {
                        var jointSpring = j.spring;
                        jointSpring.targetPosition = doorTargetPosition;
                        j.spring = jointSpring;
                    }
                    break;
            }
        }

        #endregion
    }
}


//         private Rigidbody DoorRigidbody { get; set; }
//         private HingeJoint _doorJoint;

//
//         #region Monobehavior methods
//
//         private void Awake()
//         {
//             DoorRigidbody = GetComponent<Rigidbody>();
//             _doorJoint = GetComponent<HingeJoint>();
//         }
//
//         private void Start()
//         {
//             DoorRigidbody.constraints = RigidbodyConstraints.FreezeAll;
//         }
//
//         #endregion

//         #region Event handlers
//
//         /// <summary>
//         /// Handles DiscoveryTrigger fire event.
//         /// </summary>
//         /// <param name="discoveryType">Type of detected object.</param>
//         /// <param name="discoveryTransform">Transform of object..</param>
//         /// <param name="isItemEnters">Object enters or exits trigger.</param>
//         private void OnTriggerNotify(DiscoveryType discoveryType, Transform discoveryTransform, bool isItemEnters)
//         {
//             if (!discoveryTypes.Contains(discoveryType)) return;
//
//             var character = discoveryTransform.gameObject.GetComponent<Character>();
//             
//             if (isLocked)
//             {
//                 var key = character.FindItemInInventory(GameData.KeysKey);
//                 if (key != null)
//                 {
//                     AudioIsPlaying.PlaySound(SoundType.Positive);
//                     
//                     OpenCloseDoor(discoveryTransform, isItemEnters);
//                     DoorRigidbody.constraints = RigidbodyConstraints.None;
//                     isLocked = false;
//                 }
//                 else
//                 {
//                     AudioIsPlaying.PlaySound(SoundType.Negative);
//                 }
//             }
//             else
//             {
//                 OpenCloseDoor(discoveryTransform, isItemEnters);
//             }
//         }
//
//         /// <summary>
//         /// Opens or closes door.
//         /// </summary>
//         /// <param name="discoveryTransform">Position of the entity detected by trigger to calculate the direction</param>
//         /// <param name="entry"></param>
//         private void OpenCloseDoor(Transform discoveryTransform, bool entry)
//         {
//             var jointSpring = _doorJoint.spring;
//             var forward = discoveryTransform.forward;
//
//             switch (entry)
//             {
//                 case true when forward != Vector3.zero && !_isOpen:
//                 {
//                     var a = forward.normalized;
//                     var b = transform.forward.normalized;
//
//                     var collinearity = Math.Abs(Vector3.Dot(a, b) - 1) < 0.00001f;
//
//                     if (collinearity)
//                     {
//                         jointSpring.targetPosition = -90;
//                     }
//                     else
//                     {
//                         jointSpring.targetPosition = 90;
//                     }
//
//                     _isOpen = true;
//                     break;
//                 }
//                 case false when forward != Vector3.zero && _isOpen:
//                     jointSpring.targetPosition = 0;
//                     _isOpen = false;
//                     break;
//             }
//
//             _doorJoint.spring = jointSpring;
//         }
//
//         #endregion
//
//         #region Other methods
//         // ...
//         #endregion
//
//         #endregion
//     }
// }