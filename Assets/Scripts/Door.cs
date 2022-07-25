using System;
using System.Linq;
using DmitryAdventure.Characters;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents the door entity.
    /// </summary>
    [RequireComponent(typeof(HingeJoint)), RequireComponent(typeof(AudioIsPlaying))]
    public class Door : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private string objectName;

        private Rigidbody DoorRigidbody { get; set; }
        private AudioIsPlaying AudioIsPlaying { get; set; }

        /// <summary>
        /// An array of object types to discover.
        /// </summary>
        public DiscoveryType [] discoveryTypes;
        private DiscoveryTrigger _trigger;
        private HingeJoint _doorJoint;

        [field: SerializeField, ReadonlyField, Tooltip("Determines if door is locked")] 
        private bool isLocked;
        
        private bool _isOpen;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            DoorRigidbody = GetComponent<Rigidbody>();
            AudioIsPlaying = GetComponent<AudioIsPlaying>();
            
            _trigger = GetComponentInChildren<DiscoveryTrigger>();
            if (_trigger == null)
            {
                var parent = transform.parent;
                _trigger = parent.GetComponentInChildren<DiscoveryTrigger>();
            }

            _trigger.DiscoverableTypes = discoveryTypes;
            _doorJoint = GetComponent<HingeJoint>();
        }

        private void Start()
        {
            _trigger.DiscoveryTriggerNotify += OnTriggerNotify;

            DoorRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void OnDestroy()
        {
            _trigger.DiscoveryTriggerNotify += OnTriggerNotify;
        }

        #endregion

        #region Functionality
    

        #region Event handlers

        /// <summary>
        /// Handles DiscoveryTrigger fire event.
        /// </summary>
        /// <param name="discoveryType">Type of detected object.</param>
        /// <param name="discoveryTransform">Transform of object..</param>
        /// <param name="isItemEnters">Object enters or exits trigger.</param>
        private void OnTriggerNotify(DiscoveryType discoveryType, Transform discoveryTransform, bool isItemEnters)
        {
            if (!discoveryTypes.Contains(discoveryType)) return;

            var character = discoveryTransform.gameObject.GetComponent<Character>();
            
            if (isLocked)
            {
                var key = character.FindItemInInventory(GameData.KeysKey);
                if (key != null)
                {
                    AudioIsPlaying.PlaySound(SoundType.Positive);
                    
                    OpenCloseDoor(discoveryTransform, isItemEnters);
                    DoorRigidbody.constraints = RigidbodyConstraints.None;
                    isLocked = false;
                }
                else
                {
                    AudioIsPlaying.PlaySound(SoundType.Negative);
                }
            }
            else
            {
                OpenCloseDoor(discoveryTransform, isItemEnters);
            }
        }

        /// <summary>
        /// Opens or closes door.
        /// </summary>
        /// <param name="discoveryTransform">Position of the entity detected by trigger to calculate the direction</param>
        /// <param name="entry"></param>
        private void OpenCloseDoor(Transform discoveryTransform, bool entry)
        {
            var jointSpring = _doorJoint.spring;
            var forward = discoveryTransform.forward;

            switch (entry)
            {
                case true when forward != Vector3.zero && !_isOpen:
                {
                    var a = forward.normalized;
                    var b = transform.forward.normalized;

                    var collinearity = Math.Abs(Vector3.Dot(a, b) - 1) < 0.00001f;

                    if (collinearity)
                    {
                        jointSpring.targetPosition = -90;
                    }
                    else
                    {
                        jointSpring.targetPosition = 90;
                    }

                    _isOpen = true;
                    break;
                }
                case false when forward != Vector3.zero && _isOpen:
                    jointSpring.targetPosition = 0;
                    _isOpen = false;
                    break;
            }

            _doorJoint.spring = jointSpring;
        }

        #endregion

        #region Other methods
        // ...
        #endregion

        #endregion
    }
}